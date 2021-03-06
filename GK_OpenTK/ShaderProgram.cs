﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;
using System.IO;
using OpenTK;

namespace GK_OpenTK
{
    public class ShaderProgram:IDisposable
    {
        private List<int> shaders=new List<int>();
        private int _program;
        public int program => _program;
        public ShaderProgram()
        {
            _program = GL.CreateProgram();
        }
        public void AddShader(ShaderType type,string address)
        {
            var shader = GL.CreateShader(type);
            string src = File.ReadAllText(address);
            GL.ShaderSource(shader, src);
            GL.CompileShader(shader);
            int status;
            GL.GetShader(shader, ShaderParameter.CompileStatus, out status);
            if (status != 1)
            {
                Console.WriteLine("Shader Compiles Failed");
            }
            string log = GL.GetShaderInfoLog(shader);
            int len;
            GL.GetShader(shader, ShaderParameter.InfoLogLength, out len);
            if (len > 1)
            {
                log = GL.GetShaderInfoLog(shader);
            }
            shaders.Add(shader);
        }
        public void Link()
        {
            foreach (var sh in shaders)
                GL.AttachShader(_program, sh);
            GL.BindFragDataLocation(_program, 0, "outColor");
            GL.BindAttribLocation(_program, 1, "position_modelspace");
            GL.BindAttribLocation(_program, 2, "textCoord");
            GL.BindAttribLocation(_program, 3, "normal_modelspace");
            GL.BindAttribLocation(_program, 4, "texture_index");
            GL.LinkProgram(_program);
            //   GL.UseProgram(shaderProgram);
            foreach (var sh in shaders)
            {
                GL.DetachShader(_program, sh);
                GL.DeleteShader(sh);
            }
        }
        public void SetUniform(int location,float value)
        {
            GL.Uniform1(location, value);
        }
        public void SetUniform(int location,Vector2 value)
        {
            GL.Uniform2(location, value);
        }
        public void SetUniform(int location, Vector3 value)
        {
            GL.Uniform3(location, value);
        }
        public void SetUniform(int location, Vector4 value)
        {
            GL.Uniform4(location, value);
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                GL.DeleteProgram(_program);
            }
        }
    }
}
