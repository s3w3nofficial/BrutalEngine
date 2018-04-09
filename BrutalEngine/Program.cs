using System;
using System.ComponentModel;
using BrutalEngine.input;
using BrutalEngine.loader;
using BrutalEngine.model;
using BrutalEngine.render;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace BrutalEngine
{
    internal static class Program
    {
        static void Main(string[] args)
        {
            Game game = new Game();
            game.Run(30);
        }

    }

    class Game : GameWindow
    {
        public static Game Instance;
        private bool _paused;
        private Vector2 lastMousePos;
        private readonly Camera _camera;
        private KeybordInput _keuyInput;
        private Loader _loader;
        private Renderer _renderer;
        private Shader _cubeShader;
        private int textureID;
        private RawModel verModel;

        public Game()
        {
            Instance = this;

            CursorVisible = false;

            _camera = new Camera();
            _keuyInput = new KeybordInput(this);

            _loader = new Loader();
            _renderer = new Renderer();

            _cubeShader = new Shader("cube");

            var verticies = new float[]
            {
                1, 1, 0,
                1, 0, 0,
                0, 0, 0,
                0, 1, 0,

                1, 1, 1,
                1, 0, 1,
                1, 0, 0,
                1, 1, 0,

                0, 1, 1,
                0, 0, 1,
                1, 0, 1,
                1, 1, 1,

                0, 1, 0,
                0, 0, 0,
                0, 0, 1,
                0, 1, 1,

                0, 1, 0,
                0, 1, 1,
                1, 1, 1,
                1, 1, 0,

                0, 0, 1,
                0, 0, 0,
                1, 0, 0,
                1, 0, 1
            };
            var indicies = new int[]
            {
                0,1,3,
                3,1,2,
                4,5,7,
                7,5,6,
                8,9,11,
                11,9,10,
                12,13,15,
                15,13,14,
                16,17,19,
                19,17,18,
                20,21,23,
                23,21,22
            };
            var UVs = new float[]
            {
                0, 0,
                0, 1,
                1, 1,
                1, 0,

                0, 0,
                0, 1,
                1, 1,
                1, 0,

                0, 0,
                0, 1,
                1, 1,
                1, 0,

                0, 0,
                0, 1,
                1, 1,
                1, 0,

                0, 0,
                0, 1,
                1, 1,
                1, 0,

                0, 0,
                0, 1,
                1, 1,
                1, 0
            };

            textureID = _loader.LoadTexture("textures/dirt.png");
            verModel = _loader.LoadToVao(verticies, indicies, UVs);
        }

        private void handleMouse()
        {
            if (!_paused)
            {
                var mouse = Mouse.GetState();
                var current = new Vector2(mouse.X, mouse.Y);
                var delta = current - lastMousePos;

                _camera.Pitch += delta.Y / 1000f;
                _camera.Yaw += delta.X / 1000f;
                lastMousePos = current;
            }
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            _cubeShader.Unbind();

            handleMouse();

            if (_keuyInput.IsKeyDown(Key.D))
                _camera.Pos.Xz += -_camera.Left * (float)e.Time;

            if (_keuyInput.IsKeyDown(Key.A))
                _camera.Pos.Xz += _camera.Left * (float)e.Time;

            if (_keuyInput.IsKeyDown(Key.W))
                _camera.Pos.Xz += _camera.Forward * (float)e.Time;

            if (_keuyInput.IsKeyDown(Key.S))
                _camera.Pos.Xz += -_camera.Forward * (float)e.Time;

            _camera.UpdateViewMatrix();

            _renderer.prepare();

            //RENDER
            _cubeShader.Bind();
            _cubeShader.LoadProjectionMat(_camera.Projection);
            _cubeShader.LoadViewMat(_camera.View);

            for (float i = 0f; i > -100f; i--)
            {
                _cubeShader.LoadTransformMat(Matrix4.Translation(Vector3.One * new Vector3(i, -0.5f, 1f)));
                _renderer.render(verModel, textureID);
            }

            _cubeShader.Unbind();

            //SWAP BUFFERS
            SwapBuffers();
        }

        protected override void OnResize(EventArgs e)
        {
            GL.Viewport(ClientRectangle);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(0, ClientRectangle.Width, ClientRectangle.Height, 0, Camera.NearPlane, Camera.FarPlane);

            _camera.UpdateProjectionMatrix();
        }

        protected override void OnKeyDown(KeyboardKeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (e.Alt && e.Key == Key.F4)
                Close();

            if (e.Key == Key.Escape)
                _paused = CursorVisible = !CursorVisible;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            _loader.CleanUp();
        }
    }
}
