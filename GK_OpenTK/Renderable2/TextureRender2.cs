using GK_OpenTK.Renderable2;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GK_OpenTK.Renderable
{
    class TextureRender2 : ARenderable2
    {
        public int _texture = -1;
        private int[] textures;
        private List<int> _textures = new List<int>();
        public TextureRender2(int program, float[] vertices, float[] textCoord, float[] colors, uint[] indices, string path) : base(program, indices.Length)
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo[0]);
            GL.BufferData(BufferTarget.ArrayBuffer, sizeof(float) * vertices.Length, vertices, BufferUsageHint.StaticDraw);
            GL.BufferData(BufferTarget.ElementArrayBuffer, _vertexCount * sizeof(uint), indices, BufferUsageHint.StaticDraw);

            int posAtr = GL.GetAttribLocation(program, "pposition_modelspace");
            GL.EnableVertexAttribArray(posAtr);
            GL.VertexAttribPointer(posAtr, 2, VertexAttribPointerType.Float, false, 2 * sizeof(float), 0);

            if (textCoord != null)
            {
                GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo[1]);
                GL.BufferData(BufferTarget.ArrayBuffer, sizeof(float) * textCoord.Length, textCoord, BufferUsageHint.StaticDraw);
                int colAtr = GL.GetAttribLocation(program, "textCoord");
                GL.EnableVertexAttribArray(colAtr);
                GL.VertexAttribPointer(colAtr, 2, VertexAttribPointerType.Float, false, 2 * sizeof(float), 0);
            }
            if (colors != null)
            {
                GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo[2]);
                GL.BufferData(BufferTarget.ArrayBuffer, sizeof(float) * colors.Length, colors, BufferUsageHint.StaticDraw);
                int colAtr = GL.GetAttribLocation(program, "color");
                GL.EnableVertexAttribArray(colAtr);
                GL.VertexAttribPointer(colAtr, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            }

            InitTexture(path);
        }
        public TextureRender2(int program, Vector3[] vertices, int[] indices) : base(program, indices.Length)
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo[0]);
            GL.BufferData(BufferTarget.ArrayBuffer, 3 * sizeof(float) * vertices.Length, vertices, BufferUsageHint.StaticDraw);
            GL.BufferData(BufferTarget.ElementArrayBuffer, _vertexCount * sizeof(int), indices, BufferUsageHint.StaticDraw);

            int posAtr = GL.GetAttribLocation(program, "position_modelspace");
            GL.EnableVertexAttribArray(posAtr);
            GL.VertexAttribPointer(posAtr, 3, VertexAttribPointerType.Float, true, 3 * sizeof(float), 0);
        }
        public TextureRender2(int program, Vector3[] vertices, Vector2[] textureCoord, Vector3[] normals, int[] indices, string path) : base(program, indices.Length)
        {
            //  textureCoord = multipleTextCoord(textureCoord,path);
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo[0]);
            GL.BufferData(BufferTarget.ArrayBuffer, 3 * sizeof(float) * vertices.Length, vertices, BufferUsageHint.StaticDraw);
            GL.BufferData(BufferTarget.ElementArrayBuffer, _vertexCount * sizeof(int), indices, BufferUsageHint.StaticDraw);

            int posAtr = GL.GetAttribLocation(program, "position_modelspace");
            GL.EnableVertexAttribArray(posAtr);
            GL.VertexAttribPointer(posAtr, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);

            GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo[1]);
            GL.BufferData(BufferTarget.ArrayBuffer, 2 * sizeof(float) * textureCoord.Length, textureCoord, BufferUsageHint.StaticDraw);
            int colAtr = GL.GetAttribLocation(program, "textCoord");
            GL.EnableVertexAttribArray(colAtr);
            GL.VertexAttribPointer(colAtr, 2, VertexAttribPointerType.Float, false, 2 * sizeof(float), 0);

            GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo[2]);
            GL.BufferData(BufferTarget.ArrayBuffer, 3 * sizeof(float) * normals.Length, normals, BufferUsageHint.StaticDraw);
            int normAtr = GL.GetAttribLocation(program, "normal_modelspace");
            GL.EnableVertexAttribArray(normAtr);
            GL.VertexAttribPointer(normAtr, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);


            InitTexture(path);

        }
        public TextureRender2(int program, Vector3[] vertices, Vector2[] textureCoord, Vector3[] normals, int[] indices, int[] textIndices, string[] paths) : base(program, indices.Length)
        {
            //  textureCoord = multipleTextCoord(textureCoord,path);
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo[0]);
            GL.BufferData(BufferTarget.ArrayBuffer, 3 * sizeof(float) * vertices.Length, vertices, BufferUsageHint.StaticDraw);
            GL.BufferData(BufferTarget.ElementArrayBuffer, _vertexCount * sizeof(int), indices, BufferUsageHint.StaticDraw);

            int posAtr = GL.GetAttribLocation(program, "position_modelspace");
            GL.EnableVertexAttribArray(posAtr);
            GL.VertexAttribPointer(posAtr, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);

            GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo[1]);
            GL.BufferData(BufferTarget.ArrayBuffer, 2 * sizeof(float) * textureCoord.Length, textureCoord, BufferUsageHint.StaticDraw);
            int colAtr = GL.GetAttribLocation(program, "textCoord");
            GL.EnableVertexAttribArray(colAtr);
            GL.VertexAttribPointer(colAtr, 2, VertexAttribPointerType.Float, false, 2 * sizeof(float), 0);

            GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo[2]);
            GL.BufferData(BufferTarget.ArrayBuffer, 3 * sizeof(float) * normals.Length, normals, BufferUsageHint.StaticDraw);
            int normAtr = GL.GetAttribLocation(program, "normal_modelspace");
            GL.EnableVertexAttribArray(normAtr);
            GL.VertexAttribPointer(normAtr, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);

            GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo[3]);
            GL.BufferData(BufferTarget.ArrayBuffer, sizeof(int) * textIndices.Length, textIndices, BufferUsageHint.StaticDraw);
            int textIndLoc=GL.GetAttribLocation(program,"texture_index");
            GL.EnableVertexAttribArray(textIndLoc);
            GL.VertexAttribPointer(textIndLoc, 1, VertexAttribPointerType.Int, false, sizeof(int), 0);

            InitTexture(paths);

        }
        private Vector2[] multipleTextCoord(Vector2[] textCoord, string path)
        {
            Vector2[] nText = new Vector2[textCoord.Length];
            int width;
            int height;
            using (var bmp = (Bitmap)Image.FromFile(path))
            {
                width = bmp.Width;
                height = bmp.Height;
            }
            int i = 0;
            foreach (var t in textCoord)
            {
                nText[i++] = new Vector2(t.X * (width - 1), t.Y * (height - 1));
            }
            return nText;
        }
        public void InitTexture(string path)
        {
            int width, height;
            float[] data = LoadTexture(path, out width, out height);
            GL.GenTextures(1, out _texture);
            GL.BindTexture(TextureTarget.Texture2D, _texture);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba32f, width, height, 0, PixelFormat.Rgba, PixelType.Float, data);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (float)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (float)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (float)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (float)TextureMagFilter.Linear);
           // GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba32f, width, height, 0, PixelFormat.Rgba, PixelType.Float, data);
          ///  GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
            // GL.TexPara
            //GL.CreateTextures(TextureTarget.Texture2D, 1, out _texture);
            //GL.TextureStorage2D(_texture, 1, SizedInternalFormat.Rgba32f, width, height);
            //GL.BindTexture(TextureTarget.Texture2D,_texture);
            //GL.TextureSubImage2D(_texture, 0, 0, 0, width, height, PixelFormat.Rgba, PixelType.Float, data);

        }
        private float[] LoadWithoutPath(Vector3 color, int width, int height)
        {

            float[] data;
            using (var bmp = new Bitmap(width, height))
            {
                data = new float[width * height * 4];
                int k = 0;
                for (int i = 0; i < height; i++)
                {
                    for (int j = 0; j < width; j++)
                    {
                        data[k++] = color.X;
                        data[k++] = color.Y;
                        data[k++] = color.Z;
                        data[k++] = 1;
                    }
                }
            }
            return data;
        }
        public void InitTexture(string[] paths)
        {
            //int width=0, height=0;
            //float[] data;
            //for (int i = 0; i < paths.Length; i++)
            //{
            //    if (paths[i] == "")
            //        data = LoadWithoutPath(new Vector3(0.258824f, 0.258824f, 0.258824f), width, height);
            //    else
            //      data = LoadTexture(paths[i], out width, out height);
            //    _textures.Add(GenText(data, width, height,i));
            //}


            //for (int i = 0; i < paths.Length; i++)
            //{
            //    GL.Uniform1(GL.GetUniformLocation(Program, "images"), paths.Length, _textures.ToArray());
            //}

            int width = 0, height = 0;
            float[] data;
            textures = new int[paths.Length];
            GL.GenTextures(paths.Length, textures);

            for (int i = 0; i < paths.Length; i++)
            {
                if (paths[i] == "")
                    data = LoadWithoutPath(new Vector3(0.258824f, 0.258824f, 0.258824f), width, height);
                else
                    data = LoadTexture(paths[i], out width, out height);
                GenText(data, width, height, i, textures[i]);

            }
          //  GL.Uniform1(GL.GetUniformLocation(Program, "images"), paths.Length,textures);

            //for(int i = 0; i < paths.Length; i++)
            //{
            //    string name = "image[" + i.ToString() + "]";
            //    int loc = GL.GetUniformLocation(Program,name);
            //    GL.Uniform1(loc, i);
            //}

            //GL.GenTextures(1, out _texture);
            //GL.ActiveTexture(TextureUnit.Texture0);
            //GL.BindTexture(TextureTarget.Texture2DArray, _texture);
            //GL.TexStorage3D(TextureTarget3d.Texture2DArray, 1, SizedInternalFormat.Rgba32f, width, height, paths.Length);
            //GL.TexParameter(TextureTarget.Texture2DArray, TextureParameterName.TextureWrapS, (float)TextureWrapMode.ClampToEdge);
            //GL.TexParameter(TextureTarget.Texture2DArray, TextureParameterName.TextureWrapT, (float)TextureWrapMode.ClampToEdge);
            //GL.TexParameter(TextureTarget.Texture2DArray, TextureParameterName.TextureMinFilter, (float)TextureMinFilter.Linear);
            //GL.TexParameter(TextureTarget.Texture2DArray, TextureParameterName.TextureMagFilter, (float)TextureMagFilter.Linear);
            //for (int i = 0; i < paths.Length; i++)
            //{
            //    GL.ActiveTexture(TextureUnit.Texture0);
            //    GL.BindTexture(TextureTarget.Texture2DArray, _texture);
            //    if (paths[i] == "")
            //        data = LoadWithoutPath(new Vector3(0.258824f, 0.258824f, 0.258824f), width, height);
            //    else
            //        data = LoadTexture(paths[i], out width, out height);
            //    GL.TexSubImage3D(TextureTarget.Texture2DArray, 0, 0, 0, i, width, height, 1, PixelFormat.Rgba, PixelType.Float, data);
            //}
            //GL.BindTexture(TextureTarget.Texture2DArray, _texture);
            //  GL.TexParameterI(TextureTarget.Texture2DArray,TextureParameterName.TextureMinFilter,)
            // GL.GenerateMipmap(GenerateMipmapTarget.Texture2DArray);
        }
        public void GenText(float[] data, int width, int height,int layout,int text_ind)
        {
            GL.ActiveTexture(TextureUnit.Texture0 + layout);
            GL.BindTexture(TextureTarget.Texture2D, text_ind);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba32f, width, height, 0, PixelFormat.Rgba, PixelType.Float, data);
            int loc = GL.GetUniformLocation(Program, "images[" + layout.ToString() + "]");
            GL.Uniform1(loc, layout);

            //int text;
            //GL.GenTextures(1, out text);
            //GL.ActiveTexture(TextureUnit.Texture0 + layout);
            //GL.BindTexture(TextureTarget.Texture2D, text);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (float)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (float)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (float)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (float)TextureMagFilter.Linear);
            //GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba32f, width, height, 0, PixelFormat.Rgba, PixelType.Float, data);
            //return text;

        }
        public override void Bind()
        {
            base.Bind();
            if (_texture > 0)
            {
                  GL.ActiveTexture(TextureUnit.Texture0);
                  GL.Enable(EnableCap.Texture2D);
                GL.BindTexture(TextureTarget.Texture2D, _texture);
               // GL.Disable(EnableCap.Texture2D);
            }
            if (textures!=null && textures.Length > 0)
            {
               // GL.UseProgram(Program);
               for(int i = 0; i < textures.Length; i++)
                {
                    GL.ActiveTexture(TextureUnit.Texture0 + i);
                    GL.BindTexture(TextureTarget.Texture2D, textures[i]);
                }
            }
            // GL.BindTextures(1, 2, textures);
        }
        private float[] LoadTexture(string path, out int width, out int height)
        {
            float[] data;
            using (var bmp = (Bitmap)Image.FromFile(path))
            {
                width = bmp.Width;
                height = bmp.Height;
                data = new float[width * height * 4];
                int k = 0;
                for (int i = 0; i < height; i++)
                {
                    for (int j = 0; j < width; j++)
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
