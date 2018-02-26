using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using System.IO;
using GK_OpenTK.Renderable;
using GK_OpenTK.Renderable2;

using GK_OpenTK.Camera;
using GK_OpenTK.GameObjects;
using GK_OpenTK.ModelsFactory;
using OpenTK.Input;

namespace GK_OpenTK
{
    class GameWindow : OpenTK.GameWindow
    {
        private float _time = 0;
        ARenderable prost;
        ARenderable2 prost2;
        ShaderProgram program;
        private int uniTime;
        private Matrix4 _projectionMatrix;
        private float _fov = 60f;
        AGameObject cow;
        AGameObject earth;
        ICamera camera;
        float rot = 0;
        private Vector3 offsetCamera = Vector3.Zero;
        int k;
        List<AGameObject> gameObjects = new List<AGameObject>();
        public GameWindow() : base(1280, 720, GraphicsMode.Default, "My GK Program", GameWindowFlags.Default,
            DisplayDevice.Default, 4, 5, GraphicsContextFlags.ForwardCompatible)
        { }
        protected override void OnResize(EventArgs e)
        {
            GL.Viewport(0, 0, Width, Height);
        }
        private void InitGame()
        {
            ShaderProgram programPong = new ShaderProgram();
            programPong.AddShader(ShaderType.VertexShader, @"Shaders\VertexShaderPong.c");
            programPong.AddShader(ShaderType.FragmentShader, @"Shaders\FragmentShaderPong.c");
            programPong.Link();

            ShaderProgram programSimple = new ShaderProgram();
            programSimple.AddShader(ShaderType.VertexShader, @"Shaders\SimpleVertexShader.c");
            programSimple.AddShader(ShaderType.FragmentShader, @"Shaders\SimpleFragmentShader.c");
            programSimple.Link();
            ShaderProgram programGouround = new ShaderProgram();
            programGouround.AddShader(ShaderType.VertexShader, @"Shaders\VertexShaderGouraud.c");
            programGouround.AddShader(ShaderType.FragmentShader, @"Shaders\FragmentShaderGouraud.c");
            programGouround.Link();

            //   program = programSimple;

            bool isTextured = true;
            camera = new StaticCamera();
            CreateProjection();

            // GameObjectsFactory.city(program,new Vector4(0,0,-1000,0),false);
             gameObjects.Add(GameObjectsFactory.bus(programSimple, new Vector4(0, 60, -1000f, 0), isTextured));
          //    gameObjects.Add(GameObjectsFactory.f16(programSimple, new Vector4(0, 0, -100, 0), isTextured));
            // gameObjects.Add(GameObjectsFactory.earth(programSimple, new Vector4(0, 0, -1000, 0), isTextured));
           // gameObjects.Add(GameObjectsFactory.f16(programSimple, new Vector4(0, 60, -1000, 0), isTextured));
           gameObjects.Add(GameObjectsFactory.city(programSimple, new Vector4(0, 0, -1000, 0), isTextured));
       
            //  uniTime = GL.GetUniformLocation(program.program, "time");
        }
        protected override void OnLoad(EventArgs e)
        {
            InitGame();
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
            GL.PatchParameter(PatchParameterInt.PatchVertices, 3);
            //GL.PointSize(3);
            
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.CullFace);
           // GL.Enable(EnableCap.Texture2D);
        }
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            _time += (float)e.Time;
            //  GL.Uniform3(colorUniform, new Vector3(_time, 1.0f, 0.0f));
           // gameObjects.First().SetPosition(new Vector4(gameObjects.First().possition.X + 0.1f, gameObjects.First().possition.Y, gameObjects.First().possition.Z,0));
            GL.Uniform1(uniTime, _time);
            camera.Update(_time, e.Time);
            // k = k > (int)(PolygonMode.Fill) ? (int)PolygonMode.Point : k;
            // GL.PolygonMode(MaterialFace.FrontAndBack,(PolygonMode)(k++));
            HandleKeyboard(e.Time);
        }
        private void HandleKeyboard(double dt)
        {
            var keyState = Keyboard.GetState();
            float change = 10f;

            if (keyState.IsKeyDown(Key.Escape))
            {
                Exit();
            }
            if (keyState.IsKeyDown(Key.Number1))
            {
                offsetCamera = new Vector3(0, 110,150);
                camera = new FirstPersonCamera(gameObjects.First(), offsetCamera);
            }
            if (keyState.IsKeyDown(Key.Number2))
            {
                offsetCamera = new Vector3(0, 0, 3);
                camera = new StaticCamera();
            }
            if (keyState.IsKeyDown(Key.Number3))
            {
                offsetCamera = new Vector3(0, 110, 150);
                camera = new ThirdPersonCamera(gameObjects.First(), offsetCamera);
            }
            if (keyState.IsKeyDown(Key.Space))
            {
                offsetCamera.Z -= change;
                camera.offset = offsetCamera;
            }
            if (keyState.IsKeyDown(Key.AltLeft))
            {
                offsetCamera.Z += change;
                camera.offset = offsetCamera;
            }
            if (keyState.IsKeyDown(Key.Left))
            {
                // phi--;
                //offsetCamera.X += change;
                //camera.offset = offsetCamera;
                gameObjects.First().Rotate(0.1f);
            }
            if (keyState.IsKeyDown(Key.Right))
            {
                //offsetCamera.X -= change;
                //camera.offset = offsetCamera;
                gameObjects.First().Rotate(-0.1f);
            }
            if (keyState.IsKeyDown(Key.Up))
            {
                // phi--;
                // offsetCamera.Y += change;
                // camera.offset = offsetCamera;
                gameObjects.First().Move();
            }
            if (keyState.IsKeyDown(Key.Down))
            {
                offsetCamera.Y -= change;
                camera.offset = offsetCamera;
            }


        }
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.ClearColor(0.0f, 0.0f, 0.0f, 1.0f);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.Uniform3(23, new Vector3(0, 2, -100f));
            int lastProg = -1;
            foreach (var o in gameObjects)
            {
                if(o.model.Program!=lastProg)
                    GL.UniformMatrix4(22, false, ref _projectionMatrix);
                // GL.UseProgram(o.model.Program);
                o.Render(camera);
                lastProg = o.model.Program;
            }
            //earth.Render(camera);
            SwapBuffers();
        }
        protected override void OnClosed(EventArgs e)
        {
            GL.DeleteProgram(program.program);
            Exit();
        }
        public override void Exit()
        {
            // _gameObjectFactory.Dispose();
            foreach (var mod in gameObjects)
                mod.model.Dispose();
            program.Dispose();
            //  _texturedProgram.Dispose();
            base.Exit();
        }

        private void CreateProjection()
        {

            var aspectRatio = (float)Width / Height;
            _projectionMatrix = Matrix4.CreatePerspectiveFieldOfView(
                _fov * ((float)Math.PI / 180f), // field of view angle, in radians
                aspectRatio,                // current window aspect ratio
                0.1f,                      // near plane
                4000f);                     // far plane
        }

    }
}
