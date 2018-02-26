using GK_OpenTK.Renderable;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GK_OpenTK
{
    public class ObjectFactory
    {
        public static ColorVertex[] CreateColoredRectangle()
        {
            ColorVertex[] vertecis = {new ColorVertex(new Vector2(-0.1f, 0.1f),new Vector3(1.0f, 0.0f, 0.0f)),
                                      new ColorVertex(new Vector2( 0.1f,0.1f),new Vector3(0.0f, 1.0f, 0.0f)),
                                      new ColorVertex(new Vector2( 0.1f, -0.1f),new Vector3(0.0f, 0.0f, 1.0f)),
                                      new ColorVertex(new Vector2( -0.1f, -0.1f),new Vector3(1.0f, 1.0f, 1.0f))};
            return vertecis;
        }
        public static TextureVertex[] CreateTexturedRectangle()
        {
            TextureVertex[] vertecis = {new TextureVertex(new Vector2(-1.0f, 1.0f),new Vector2(0.0f, 0.0f)),
                                      new TextureVertex(new Vector2( 1.0f,1.0f),new Vector2(1.0f, 0.0f)),
                                      new TextureVertex(new Vector2( 1.0f, -1.0f),new Vector2(1.0f,1.0f)),
                                      new TextureVertex(new Vector2( -1.0f, -1.0f),new Vector2(0.0f, 1.0f))};
            return vertecis;
        }
        public static float[] CreateVeticesForTR2()
        {
            float[] vert =
            {
                -0.5f, 0.5f,
                 0.5f,0.5f,
                 0.5f, -0.5f,
                 -0.5f, -0.5f
            };
            return vert;
        }
        public static float[] CreateTextCoordForTR2()
        {
            float[] coord =
            {
                0.0f, 0.0f,
                1.0f, 0.0f,
                1.0f,1.0f,
                0.0f, 1.0f
            };
            return coord;
        }
        public static float[] CreateColorsForTR2()
        {
            float[] col =
        {
                0.0f, 0.0f,1.0f,
                1.0f, 0.0f,0.0f,
                1.0f,1.0f,0.0f,
                0.0f, 1.0f,1.0f
            };
            return col;
        }
        public static uint[] CreateElementsIndexesForRectangle()
        {
            uint[] elements = {
                                0, 1, 2,
                                2, 3, 0
                              };
            return elements;
        }
        public static Vector3[] CreateTriangle()
        {
            Vector3[] tr = { new Vector3(-0.5f, 0.5f, -2.7f), new Vector3(0.5f, 0.5f, -2.7f), new Vector3(0.5f, -0.5f, -2.7f) };
            return tr;
        }
        public static uint[] CreateElementsIndexesForTr()
        {
            uint[] elements = {
                                0, 1, 2, };
            return elements;
        }
    }

 }

