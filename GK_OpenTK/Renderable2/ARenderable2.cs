using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GK_OpenTK.Renderable2
{
    class ARenderable2
    {
        protected int _program;
        private int _vao;
        protected int[] _vbo;
        private int _veo;
        protected int _vertexCount;
        public int Program { get { return _program; } set{ _program = value; } }
        public ARenderable2(int program,int vertexCount)
        {
            _program = program;
            _vertexCount = vertexCount;
            _vbo = new int[4];
            //create vao, vbo, veo
            GL.GenVertexArrays(1, out _vao);
            GL.GenBuffers(4,_vbo);
            GL.GenBuffers(1, out _veo);
            //Make vao, vbo, veo alive
            GL.BindVertexArray(_vao);
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
           // GL.DrawArrays(PrimitiveType.Triangles, 0, _vertexCount);
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
                GL.DeleteBuffers(2,_vbo);
                GL.DeleteBuffer(_veo);
            }
        }
    }
}
