using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GK_OpenTK.Renderable
{
    class TextureRenderObject:ARenderable
    {
        private int _texture;
        private int[] textures;
        public TextureRenderObject(int program,TextureVertex[] vertices,uint[] elementsInd,string path) : base(program, elementsInd.Length)
        {
            GL.BufferData(BufferTarget.ArrayBuffer, TextureVertex.Size * vertices.Length, vertices, BufferUsageHint.StaticDraw);
            GL.BufferData(BufferTarget.ElementArrayBuffer, _vertexCount * sizeof(uint), elementsInd, BufferUsageHint.StaticDraw);

            int posAtr = GL.GetAttribLocation(program,"position");
            GL.EnableVertexAttribArray(posAtr);
            GL.VertexAttribPointer(posAtr,2, VertexAttribPointerType.Float, false, TextureVertex.Size, 0);
            int colAtr = GL.GetAttribLocation(program, "textCoord");
            GL.EnableVertexAttribArray(colAtr);
            GL.VertexAttribPointer(colAtr,2, VertexAttribPointerType.Float, false, TextureVertex.Size, 8);

            InitTexture(path);
        }
        public TextureRenderObject(int program, TextureVertex[] vertices, uint[] elementsInd, string[] paths,string[] names) : base(program, elementsInd.Length)
        {
            GL.BufferData(BufferTarget.ArrayBuffer, TextureVertex.Size * vertices.Length, vertices, BufferUsageHint.StaticDraw);
            GL.BufferData(BufferTarget.ElementArrayBuffer, _vertexCount * sizeof(uint), elementsInd, BufferUsageHint.StaticDraw);

            int posAtr = GL.GetAttribLocation(program, "position");
            GL.EnableVertexAttribArray(posAtr);
            GL.VertexAttribPointer(posAtr, 2, VertexAttribPointerType.Float, false, TextureVertex.Size, 0);
            int colAtr = GL.GetAttribLocation(program, "textCoord");
            GL.EnableVertexAttribArray(colAtr);
            GL.VertexAttribPointer(colAtr, 2, VertexAttribPointerType.Float, false, TextureVertex.Size, 8);

            InitTexture(paths,names);
        }
        public void InitTexture(string[] paths,string[] names)
        {
            textures = new int[paths.Length];
            GL.GenTextures(paths.Length, textures);
            int k = (int)TextureUnit.Texture0;
            for (int i = 0; i < paths.Length; i++)
            {
                int width, height;
                float[] data = LoadTexture(paths[i], out width, out height);
                GL.ActiveTexture((TextureUnit)k);
                //GL.GenTextures(1, out _texture);
                GL.BindTexture(TextureTarget.Texture2D, textures[i]);
                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba32f, width, height, 0, PixelFormat.Rgba, PixelType.Float, data);
                GL.Uniform1(GL.GetUniformLocation(_program, names[i]), i);
                GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (float)TextureWrapMode.ClampToEdge);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (float)TextureWrapMode.ClampToEdge);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (float)TextureMinFilter.Linear);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (float)TextureMagFilter.Linear);
              
                k++;
            }


        }
        public void InitTexture(string path)
        {
            int width, height;
            float[] data = LoadTexture(path, out width, out height);
       
            GL.GenTextures(1, out _texture);
            GL.BindTexture(TextureTarget.Texture2D, _texture);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba32f, width, height, 0, PixelFormat.Rgba, PixelType.Float, data);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS,(float)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (float)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.Texture2D,TextureParameterName.TextureMinFilter,(float)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (float)TextureMagFilter.Linear);
            // GL.TexPara
            //GL.CreateTextures(TextureTarget.Texture2D, 1, out _texture);
            //GL.TextureStorage2D(_texture, 1, SizedInternalFormat.Rgba32f, width, height);
            //GL.BindTexture(TextureTarget.Texture2D,_texture);
            //GL.TextureSubImage2D(_texture, 0, 0, 0, width, height, PixelFormat.Rgba, PixelType.Float, data);

        }
        public override void Bind()
        {
            base.Bind();
            //   GL.BindTexture(TextureTarget.Texture2D, _texture);
           // GL.BindTextures(1, 2, textures);
        }
        private float[] LoadTexture(string path,out int width,out int height)
        {
            float[] data;
            using (var bmp = (Bitmap)Image.FromFile(path))
            {
                width = bmp.Width;
                height = bmp.Height;
                data = new float[width * height * 4];
                int k = 0;
                for(int i = 0; i < height; i++)
                {
                    for(int j = 0; j <width; j++)
                    {
                        data[k++] = bmp.GetPixel(j, i).R / 255f;
                        data[k++] = bmp.GetPixel(j, i).G / 255f;
                        data[k++] = bmp.GetPixel(j, i).B / 255f;
                        data[k++] = bmp.GetPixel(j, i).A / 255f;
                    }
                }
            }
            return data;
        }
    }
}
