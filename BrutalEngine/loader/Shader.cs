using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace BrutalEngine.model
{
    public class Shader
    {
        private int _vshId;
        private int _fshId;

        private readonly int _program;

        private readonly int _locProjection;
        private readonly int _locView;
        private readonly int _locTransformation;

        public Shader(string name)
        {
            LoadShader(name);

            //creates and ID for this _program
            _program = GL.CreateProgram();

            //attaches shaders to this _program
            GL.AttachShader(_program, _vshId);
            GL.AttachShader(_program, _fshId);

            GL.BindAttribLocation(_program, 0, "position");
            GL.BindAttribLocation(_program, 1, "uv");

            GL.LinkProgram(_program);
            GL.ValidateProgram(_program);

            _locProjection = GL.GetUniformLocation(_program, "projection");
            _locView = GL.GetUniformLocation(_program, "view");
            _locTransformation = GL.GetUniformLocation(_program, "transformation");
        }

        private void LoadShader(string shaderName)
        {
            string vertexShaderCode = File.ReadAllText($"shader/{shaderName}.vsh");
            string fragmentShaderCode = File.ReadAllText($"shader/{shaderName}.fsh");

            _vshId = GL.CreateShader(ShaderType.VertexShader);
            _fshId = GL.CreateShader(ShaderType.FragmentShader);

            GL.ShaderSource(_vshId, vertexShaderCode);
            GL.ShaderSource(_fshId, fragmentShaderCode);

            GL.CompileShader(_vshId);
            GL.CompileShader(_fshId);
        }

        public void LoadProjectionMat(Matrix4 mat)
        {
            GL.UniformMatrix4(_locProjection, false, ref mat);
        }

        public void LoadTransformMat(Matrix4 mat)
        {
            GL.UniformMatrix4(_locTransformation, false, ref mat);
        }

        public void LoadViewMat(Matrix4 mat)
        {
            GL.UniformMatrix4(_locView, false, ref mat);
        }

        public void Bind()
        {
            GL.UseProgram(_program);
        }

        public void Unbind()
        {
            GL.UseProgram(0);
        }

    }
}