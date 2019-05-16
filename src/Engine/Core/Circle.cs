﻿using Fusee.Math.Core;
using Fusee.Serialization;
using System.Collections.Generic;

namespace Fusee.Engine.Core
{
    public class Circle : Mesh
    {        
        public Circle(bool fill = false, int segments = 20, float fillPercentage = 100, float thickness = 0.1f)
        {            
            var verts = new List<float3>();
            var uvs = new List<float2>();
            var triangles = new List<ushort>();
            var normals = new List<float3>();

            var angleByStep = fillPercentage / 100f * (float)(System.Math.PI * 2f) / segments;
            var currentAngle = 0f;
            var cos = (float)System.Math.Cos(currentAngle);
            var sin = (float)System.Math.Sin(currentAngle);
            var radOuter = 0.5f;
            var radInner = radOuter - thickness;

            if (fill)
            {
                verts.Add(float3.Zero);
                var uv = new float2(Normalize(0, 1, -1), Normalize(0, 1, -1));
                uvs.Add(uv);
                normals.Add(new float3(0, 0, -1));
            }

            for (int i = 0; i <= segments; i++)
            {                
                cos = (float)System.Math.Cos(currentAngle);
                sin = (float)System.Math.Sin(currentAngle);

                if (i < segments)
                {
                    var vert0 = new float3(radOuter * cos, radOuter * sin, 0);
                    verts.Add(vert0);
                    var uv0 = new float2(Normalize(vert0.x, 1, -1), Normalize(vert0.y, 1, -1));
                    uvs.Add(uv0);
                    normals.Add(new float3(0, 0, -1));

                    if (!fill)
                    {
                        var vert1 = new float3(radInner * cos, radInner * sin, 0);
                        verts.Add(vert1);
                        var uv1 = new float2(Normalize(vert1.x, 1, -1), Normalize(vert1.y, 1, -1));
                        uvs.Add(uv1);
                        normals.Add(new float3(0, 0, -1));
                    }
                }

                var lastVertIndex = verts.Count-1;                

                if (i > 0 && i != segments) //odd
                {
                    if (!fill)
                    {
                        triangles.Add((ushort)(lastVertIndex - 3)); //0
                        triangles.Add((ushort)(lastVertIndex - 1)); //2
                        triangles.Add((ushort)(lastVertIndex - 2)); //1

                        triangles.Add((ushort)(lastVertIndex - 2)); //1
                        triangles.Add((ushort)(lastVertIndex - 1)); //2
                        triangles.Add((ushort)(lastVertIndex));//3
                    }
                    else
                    {
                        triangles.Add(0); //0
                        triangles.Add((ushort)(lastVertIndex - 1)); //1
                        triangles.Add((ushort)lastVertIndex); //2
                    }
                    
                }
                else if (i == segments)
                {
                    if (!fill)
                    {
                        triangles.Add((ushort)(lastVertIndex - 1)); //4
                        triangles.Add(0);
                        triangles.Add(1);

                        triangles.Add((ushort)(lastVertIndex - 1)); //4
                        triangles.Add(1);
                        triangles.Add((ushort)lastVertIndex); //5
                    }
                    else
                    {
                        triangles.Add(0); 
                        triangles.Add((ushort)lastVertIndex); //3
                        triangles.Add(1);
                        
                    }

                }
                currentAngle += angleByStep;
            }

            Vertices = verts.ToArray();
            Normals = normals.ToArray();
            Triangles = triangles.ToArray();
            UVs = uvs.ToArray();
        }
        
        private float Normalize(float input, float max, float min)
        {
            return (input - min) / (max - min);
        }
    }
}
