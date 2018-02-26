using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;
using OpenTK;

namespace GK_OpenTK.Renderable
{
    public struct TextureVertex
    {
        public const int Size = (2 + 2) * 4;
        private Vector2 _coord;
        private Vector2 _textCoord;
        public TextureVertex(Vector2 coord,Vector2 textCoord)
        {
            _coord = coord;
            _textCoord = textCoord;
        }
    }
}
