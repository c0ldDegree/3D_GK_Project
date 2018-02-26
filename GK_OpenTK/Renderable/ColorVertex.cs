using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GK_OpenTK
{
   public struct ColorVertex
    {
        public const int Size = (2 + 3) * 4;
        private readonly Vector2 _position;
        private readonly Vector3 _color;
        public ColorVertex(Vector2 position,Vector3 Color)
        {
            _position = position;
            _color = Color;
        }
    }
}
