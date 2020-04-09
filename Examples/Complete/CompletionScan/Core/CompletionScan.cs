using Fusee.Base.Common;
using Fusee.Base.Core;
using Fusee.Engine.Common;
using Fusee.Engine.Core;
using Fusee.Engine.Core.Scene;
using Fusee.Engine.Core.ShaderShards;
using Fusee.Engine.GUI;
using Fusee.Math.Core;
using Fusee.Xene;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Fusee.Engine.Core.Input;
using static Fusee.Engine.Core.Time;

namespace Fusee.Examples.CompletionScan.Core
{
    [FuseeApplication(Name = "FUSEE Simple Example", Description = "A very simple example.")]
    public class CompletionScan : RenderCanvas
    {
        private SceneContainer _rocketScene;
        private SceneRendererForward _sceneRenderer;

        private SceneRendererForward _guiRenderer;
        private SceneContainer _gui;
        private SceneInteractionHandler _sih;
        private readonly CanvasRenderMode _canvasRenderMode = CanvasRenderMode.SCREEN;

        // Cam variables
        private Transform _camTransform;
        private Transform _guiCamTransform;
        private Transform _sndCamTransform;
        private readonly Camera _cam = new Camera(ProjectionMethod.Perspective, 5, 100, M.PiOver4);
        private readonly Camera _guiCam = new Camera(ProjectionMethod.Orthographic, 1, 1000, M.PiOver4);
        private readonly Camera _sndCam = new Camera(ProjectionMethod.Perspective, 1, 1000, M.PiOver4);

        // Angle variables
        private float _angleHorz, _angleVert, _angleVelHorz, _angleVelVert;
        private float _angleHorzSnd, _angleVertSnd, _angleVelHorzSnd, _angleVelVertSnd;
        private float3 _rotAxis;
        private float3 _rotPivot;

        private const float Damping = 0.003f;

        // Init is called on startup.
        public override async Task<bool> Init()
        {
            _cam.Viewport = new float4(0, 0, 100, 100);
            _cam.BackgroundColor = new float4(1, 1, 1, 1);
            _cam.Layer = -1;

            _camTransform = _guiCamTransform = new Transform()
            {
                Rotation = float3.Zero,
                Translation = new float3(0, 1, -30),
                Scale = new float3(1, 1, 1)
            };
            _sndCamTransform = new Transform()
            {
                Rotation = new float3(M.PiOver6, 0, 0),
                Translation = new float3(10, 40, -60),
                Scale = float3.One
            };

            var cam = new SceneNode()
            {
                Name = "Cam",
                Components = new List<SceneComponent>()
                {
                    _camTransform,
                    _cam,
                    ShaderCodeBuilder.MakeShaderEffect(new float4(1,0,0,1), float4.One, 10),
                    new Cube(),

                },
                Children = new ChildList()
                {
                    new SceneNode()
                    {
                        Components = new List<SceneComponent>()
                        {
                            new Transform()
                            {
                                Scale = new float3(0.5f, 0.5f, 1f),
                                Translation = new float3(0,0, 1f)
                            },
                            new Cube()
                        }
                    }
                }
            };
            var cam1 = new SceneNode()
            {
                Name = "SecondCam",
                Components = new List<SceneComponent>()
                {
                    _sndCamTransform,
                    _sndCam,
                }
            };

            _angleHorzSnd = _sndCamTransform.Rotation.y;
            _angleVertSnd = _sndCamTransform.Rotation.x;
            _angleHorz = _camTransform.Rotation.y;
            _angleVert = _camTransform.Rotation.x;

            _gui = CreateGui();

            // Create the interaction handler
            _sih = new SceneInteractionHandler(_gui);

            // Load the rocket model
            _rocketScene = AssetStorage.Get<SceneContainer>("FUSEERocket.fus");

            _rocketScene.Children.Add(cam);
            _rocketScene.Children.Add(cam1);

            // Wrap a SceneRenderer around the model.
            _sceneRenderer = new SceneRendererForward(_rocketScene);
            _guiRenderer = new SceneRendererForward(_gui);

            _rotAxis = float3.UnitY * float4x4.CreateRotationYZ(new float2(M.PiOver4, M.PiOver4));
            _rotPivot = _rocketScene.Children[1].GetComponent<Transform>().Translation;

            return true;
        }

        // RenderAFrame is called once a frame
        public override void RenderAFrame()
        {
            if (Mouse.LeftButton)
            {
                _angleVelHorz = Mouse.XVel * Damping * DeltaTime;
                _angleVelVert = Mouse.YVel * Damping * DeltaTime;

                _angleHorz += _angleVelHorz;
                _angleVert += _angleVelVert;

                _angleVelHorz = _angleVelVert = 0;

                _camTransform.FpsView(_angleHorz, _angleVert, Keyboard.WSAxis, Keyboard.ADAxis, DeltaTime * 10);
            }
            else if (Mouse.RightButton)
            {
                _angleVelHorzSnd = Mouse.XVel * Damping * DeltaTime;
                _angleVelVertSnd = Mouse.YVel * Damping * DeltaTime;

                _angleHorzSnd += _angleVelHorzSnd;
                _angleVertSnd += _angleVelVertSnd;

                _angleVelHorzSnd = _angleVelVertSnd = 0;

                _sndCamTransform.FpsView(_angleHorzSnd, _angleVertSnd, Keyboard.WSAxis, Keyboard.ADAxis, DeltaTime * 10);
            }

            _sceneRenderer.Render(RC);
            _guiRenderer.Render(RC);

            if (!Mouse.Desc.Contains("Android"))
                _sih.CheckForInteractiveObjects(RC, Mouse.Position, Width, Height);
            if (Touch.GetTouchActive(TouchPoints.Touchpoint_0) && !Touch.TwoPoint)
                _sih.CheckForInteractiveObjects(RC, Touch.GetPosition(TouchPoints.Touchpoint_0), Width, Height);

            // Swap buffers: Show the contents of the backbuffer (containing the currently rendered frame) on the front buffer.
            Present();
        }

        private SceneContainer CreateGui()
        {
            var vsTex = AssetStorage.Get<string>("texture.vert");
            var psTex = AssetStorage.Get<string>("texture.frag");

            var canvasWidth = Width / 100f;
            var canvasHeight = Height / 100f;

            var btnFuseeLogo = new GUIButton
            {
                Name = "Canvas_Button"
            };
            btnFuseeLogo.OnMouseEnter += BtnLogoEnter;
            btnFuseeLogo.OnMouseExit += BtnLogoExit;
            btnFuseeLogo.OnMouseDown += BtnLogoDown;

            var guiFuseeLogo = new Texture(AssetStorage.Get<ImageData>("FuseeText.png"));
            var fuseeLogo = new TextureNode(
                "fuseeLogo",
                vsTex,
                psTex,
                //Set the albedo texture you want to use.
                guiFuseeLogo,
                //Define anchor points. They are given in percent, seen from the lower left corner, respectively to the width/height of the parent.
                //In this setup the element will stretch horizontally but stay the same vertically if the parent element is scaled.
                UIElementPosition.GetAnchors(AnchorPos.TOP_TOP_LEFT),
                //Define Offset and therefor the size of the element.
                UIElementPosition.CalcOffsets(AnchorPos.TOP_TOP_LEFT, new float2(0, canvasHeight - 0.5f), canvasHeight, canvasWidth, new float2(1.75f, 0.5f))
                );
            fuseeLogo.AddComponent(btnFuseeLogo);

            var fontLato = AssetStorage.Get<Font>("Lato-Black.ttf");
            var guiLatoBlack = new FontMap(fontLato, 24);

            var text = new TextNode(
                "FUSEE Simple Example",
                "ButtonText",
                vsTex,
                psTex,
                UIElementPosition.GetAnchors(AnchorPos.STRETCH_HORIZONTAL),
                UIElementPosition.CalcOffsets(AnchorPos.STRETCH_HORIZONTAL, new float2(canvasWidth / 2 - 4, 0), canvasHeight, canvasWidth, new float2(8, 1)),
                guiLatoBlack,
                ColorUint.Tofloat4(ColorUint.Greenery),
                HorizontalTextAlignment.CENTER,
                VerticalTextAlignment.CENTER);

            var canvas = new CanvasNode(
                "Canvas",
                _canvasRenderMode,
                new MinMaxRect
                {
                    Min = new float2(-canvasWidth / 2, -canvasHeight / 2f),
                    Max = new float2(canvasWidth / 2, canvasHeight / 2f)
                })
            {
                Children = new ChildList()
                {
                    //Simple Texture Node, contains the fusee logo.
                    fuseeLogo,
                    text
                }
            };

            var cam = new SceneNode()
            {
                Name = "GUICam",
                Components = new List<SceneComponent>()
                {
                    _guiCamTransform,
                    _guiCam
                }
            };

            return new SceneContainer
            {
                Children = new List<SceneNode>
                {
                    canvas
                }
            };
        }

        public void BtnLogoEnter(CodeComponent sender)
        {
            _gui.Children.FindNodes(node => node.Name == "fuseeLogo").First().GetComponent<ShaderEffect>().SetEffectParam(UniformNameDeclarations.AlbedoColor, new float4(0.8f, 0.8f, 0.8f, 1f));
        }

        public void BtnLogoExit(CodeComponent sender)
        {
            _gui.Children.FindNodes(node => node.Name == "fuseeLogo").First().GetComponent<ShaderEffect>().SetEffectParam(UniformNameDeclarations.AlbedoColor, float4.One);
        }

        public void BtnLogoDown(CodeComponent sender)
        {
            OpenLink("http://fusee3d.org");
        }
    }
}