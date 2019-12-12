using System.Collections.Generic;
using System.IO;
using Fusee.Base.Common;
using Fusee.Base.Core;
using Fusee.Math.Core;
using System.Runtime.InteropServices;
using SharpFontManaged;
using System;
using System.Linq;
using System.Numerics;

namespace Fusee.Base.Imp.WebAsm
{
    /// <summary>
    /// Font implementation using SharpFont
    /// https://github.com/MikePopoloski/SharpFont
    /// this is NOT the SharpFont freetype wrapper but a C# native library
    /// </summary>
    public class FontImp : IFontImp
    {

        internal FontFace _face;
        private bool _useKerning;
        private uint _pixelHeight;

        public FontImp(Stream stream)
        {
            _face = new FontFace(stream);
            _pixelHeight = 18;
            UseKerning = false;
        }

        public bool UseKerning
        {
            get { return _useKerning; }
            set
            {
                _useKerning = value;
            }
        }


        public uint PixelHeight
        {
            get
            {
                return _pixelHeight;
            }
            set
            {
                _pixelHeight = value;
            }
        }

        
        public Curve GetGlyphCurve(uint c)
        {

            var curve = new Curve
            {
                CurveParts = new List<CurvePart>()
            };
            var glyph = _face.GetGlyphUnscaled(new CodePoint((char)c));
            var orgPointCoords = glyph.Points.Select(pt => new PointF(new Vector2((int)pt.X, (int)pt.Y), pt.Type)).ToArray();
            var ptTypeAsList = orgPointCoords.Select(pt => pt.Type).ToList();
            var ptTypeAsByteArray = new List<byte>();
                   
            foreach (var pt in ptTypeAsList)
            {
                switch (pt)
                {
                    case PointType.OnCurve:
                        ptTypeAsByteArray.AddRange(new byte[] { 1 });
                        break;
                    case PointType.Quadratic:
                        ptTypeAsByteArray.AddRange(new byte[] { 0 });
                        break;
                    case PointType.Cubic:
                        throw new NotImplementedException("Cubic pattern not yet implemented and/or available (ttf)");
                    default:
                        break;
                }
            }

            var pointTags = ptTypeAsByteArray.ToArray();
            if (orgPointCoords == null) return curve;

            //Freetype contours are defined by their end points.
            var curvePartEndPoints = glyph.ContourEndpoints.Select(pt => (short)pt).ToArray();

            var partTags = new List<byte>();
            var partVerts = new List<float3>();

            //Writes points of a freetyp contour into a CurvePart,
            for (var i = 0; i <= orgPointCoords.Length; i++)
            {
                //If a certain index of outline points is in array of contour end points - create new CurvePart and add it to Curve.CurveParts
                if (!curvePartEndPoints.Contains((short)i)) continue;

                partVerts.Clear();
                partTags.Clear();

                var part = SplitToCurvePartHelper.CreateCurvePart(orgPointCoords, pointTags, curvePartEndPoints, i,
                    partVerts, partTags);
                curve.CurveParts.Add(part);

                var segments = SplitToCurveSegmentHelper.SplitPartIntoSegments(part, partTags, partVerts);
                SplitToCurveSegmentHelper.CombineCurveSegmentsAndAddThemToCurvePart(segments, part);
            }

            return curve;

        }

        /// <summary>
        /// Get glyph info from letter
        /// </summary>
        /// <param name="c">letter char</param>
        /// <returns></returns>
        public GlyphInfo GetGlyphInfo(uint c)
        {
            GlyphInfo ret;

            var cp = new CodePoint((char)c);
            var glyph = _face.GetGlyph(cp, _pixelHeight);

            if (glyph == null)
            {
                return new GlyphInfo
                { Height = 1, Width = 1 };
            }

            ret.CharCode = c;
            ret.AdvanceX = glyph.HorizontalMetrics.Advance;
            ret.AdvanceY = glyph.VerticalMetrics.Advance;

            ret.Width = glyph.Width;
            ret.Height = glyph.Height;

            return ret;
        }

        public float GetKerning(uint leftC, uint rightC)
        {
            var kern = _face.GetKerning(new CodePoint((char)leftC), new CodePoint((char)rightC), PixelHeight);
            return kern;
        }

        public float GetUnscaledAdvance(uint c)
        {
            var unscaledGlyph = _face.GetGlyphUnscaled(new CodePoint((char)c));
            
            var unscaledAdv = unscaledGlyph.Advance;
            return unscaledAdv;
        }

        public float GetUnscaledKerning(uint leftC, uint rightC)
        {
            // TODO: Implement unscaled kerning
            return _face.GetKerning(new CodePoint((char)leftC), new CodePoint((char)rightC), PixelHeight);
        }

        public IImageData RenderGlyph(uint c, out int bitmapLeft, out int bitmapTop)
        {
            var surface = RenderGlyph(_face, (char)c);
            var glyph = _face.GetGlyph(new CodePoint((char)c), PixelHeight);
            var metric = _face.GetFaceMetrics(PixelHeight);

            var ret = new ImageData(new byte[surface.Height * surface.Width], surface.Width, surface.Height, new ImagePixelFormat(ColorFormat.Intensity));

            bitmapLeft = 0;
            bitmapTop = 0;

            if (surface.Bits == IntPtr.Zero) return ret;

            var imgBytes = new byte[surface.Height * surface.Width];

            unsafe
            {
                var idx = 0;

                for (int y = 0; y < surface.Height; y++)
                {
                    var src = (byte*)surface.Bits + (y * surface.Pitch);

                    for (int x = 0; x < surface.Width; x++)
                    {
                        imgBytes[idx++] = *src++;
                    }
                }
            }

            ret.PixelData = imgBytes;

            bitmapTop = (int)(glyph.Height + glyph.VerticalMetrics.Bearing.Y);
            bitmapLeft = 0;


            return ret;
        }

        private unsafe Surface RenderGlyph(FontFace typeface, char c)
        {
            var glyph = typeface.GetGlyph(c, PixelHeight);
            if (glyph == null)
            {
                return new Surface
                {
                    Height = 1,
                    Width = 1
                };
            }

            var surface = new Surface
            {
                Bits = Marshal.AllocHGlobal(glyph.RenderWidth * glyph.RenderHeight),
                Width = glyph.RenderWidth,
                Height = glyph.RenderHeight,
                Pitch = glyph.RenderWidth
            };

            var stuff = (byte*)surface.Bits;

            for (int i = 0; i < surface.Width * surface.Height; i++)
                *stuff++ = 0;

            glyph.RenderTo(surface);

            return surface;
        }


        internal class SplitToCurvePartHelper
        {
            #region Methodes
            public static void CurvePartVertice(CurvePart cp, int j, PointF[] orgPointCoords, List<float3> partVerts)
            {
                var vert = new float3(orgPointCoords[j].P.X, orgPointCoords[j].P.Y, 0);
                partVerts.Add(vert);
            }

            public static CurvePart CreateCurvePart(PointF[] orgPointCoords, byte[] pointTags, short[] curvePartEndPoints, int i, List<float3> partVerts, List<byte> partTags)
            {
                var index = Array.IndexOf(curvePartEndPoints, (short)i);
                var cp = new CurvePart
                {
                    IsClosed = true,
                    CurveSegments = new List<CurveSegment>()
                };

                //Marginal case - first contour ( 0 to contours[0] ). 
                if (index == 0)
                {
                    for (var j = 0; j <= i; j++)
                    {
                        CurvePartVertice(cp, j, orgPointCoords, partVerts);
                        partTags.Add(pointTags[j]);
                    }
                    //The start point is the first point in the outline.Points array.
                    cp.StartPoint = new float3(orgPointCoords[0].P.X, orgPointCoords[0].P.Y, 0);
                }

                //contours[0]+1 to contours[1]
                else
                {
                    for (var j = curvePartEndPoints[index - 1] + 1; j <= curvePartEndPoints[index]; j++)
                    {
                        CurvePartVertice(cp, j, orgPointCoords, partVerts);
                        partTags.Add(pointTags[j]);
                    }

                    //The index in outline.Points which describes the start point is given by the index of the foregone outline.contours index +1.
                    cp.StartPoint = new float3(orgPointCoords[curvePartEndPoints[index - 1] + 1].P.X, orgPointCoords[curvePartEndPoints[index - 1] + 1].P.Y, 0);
                }
                return cp;
            }
            #endregion
        }
    }
}