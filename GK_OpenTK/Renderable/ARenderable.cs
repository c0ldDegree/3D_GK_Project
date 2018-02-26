using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GK_OpenTK
{
    public abstract class ARenderable:IDisposable
    {
        protected int _program;
        private int _vao;
        private int _vbo;
        private int _veo;
        protected int _vertexCount;
        public int Program => _program;
        public ARenderable(int program,int vertexCount)
        {
            _program = program;
            _vertexCount = vertexCount;
            //create vao, vbo, veo
            GL.GenVertexArrays(1,out _vao);
            GL.GenBuffers(1, out _vbo);
            GL.GenBuffers(1,out _veo);
            //Make vao, vbo, veo alive
            GL.BindVertexArray(_vao);
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _veo);
        }
        public virtual void Bind()
        {
            //switch rendering to this object and with it`s own buffer data to use
            GL.UseProgram(_program);
            GL.BindVertexArray(_vao);
        }

        public void Render()
        {
            GL.DrawElements(BeginMode.Triangles, _vertexCount, DrawElementsType.UnsignedInt, 0);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        public virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                GL.DeleteVertexArray(_vao);
                GL.DeleteBuffer(_vbo);
                GL.DeleteBuffer(_veo);
            }
        }
    }
}
