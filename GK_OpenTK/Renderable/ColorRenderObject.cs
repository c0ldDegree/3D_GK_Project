using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GK_OpenTK
{
    class ColorRenderObject:ARenderable
    {

        public ColorRenderObject(int program,int vertexCount,ColorVertex[] vertices,uint[] elements):base(program,vertexCount)
        {
            GL.BufferData(BufferTarget.ArrayBuffer, ColorVertex.Size * vertices.Length, vertices, BufferUsageHint.StaticDraw);
            GL.BufferData(BufferTarget.ElementArrayBuffer, vertexCount * sizeof(uint), elements, BufferUsageHint.StaticDraw);

            int posAtr = GL.GetAttribLocation(program, "position");
            GL.EnableVertexAttribArray(posAtr);
            GL.VertexAttribPointer(posAtr, 2, VertexAttribPointerType.Float, false, ColorVertex.Size, 0);
            int colAtr = GL.GetAttribLocation(program, "color");
            GL.EnableVertexAttribArray(colAtr);
            GL.VertexAttribPointer(colAtr, 3, VertexAttribPointerType.Float, false, ColorVertex.Size, 8);

           // colorUniform = GL.GetUniformLocation(shaderProgram, "triangleColor");
            //          GL.Uniform3(loc, new Vector3(1.0f, 1.0f, 0.0f));
            //  GL.UseProgram(shaderProgram);
           // GL.UseProgram(program);
        }
    }
}
