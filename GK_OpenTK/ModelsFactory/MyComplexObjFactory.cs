using OpenTK;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GK_OpenTK.ModelsFactory
{
    class MyComplexObjFactory : AModelFactory
    {
        private int[] vertInd;

        public override int[] GetIndices(int offset = 0)
        {
            return vertInd;
        }
        public Vector3[] GetVertices()
        {
            return vertices;              
        }

        public static MyComplexObjFactory LoadFile(string filename)
        {
            MyComplexObjFactory obj = new MyComplexObjFactory();
            try
            {
                using (StreamReader reader = new StreamReader(new FileStream(filename, FileMode.Open, FileAccess.Read)))
                {
                    obj = LoadFromString(reader.ReadToEnd());
                }
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine("File not found: {0}", filename);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error loading file: {0}", filename);
            }
            return obj;
        }
        public static MyComplexObjFactory LoadFromString(string content)
        {
            NumberStyles style = NumberStyles.Float;
            CultureInfo culture = CultureInfo.CreateSpecificCulture("en-US");
            // Seperate lines from the file
            List<String> lines = new List<string>(content.Split('\n'));
            List<Vector3> vertices = new List<Vector3>();
            List<Vector2> textCoord = new List<Vector2>();
            List<float> normals = new List<float>();
            List<int> vertIndi = new List<int>();
            List<int> normInd = new List<int>();
            List<int> textInd = new List<int>();

            foreach (String line in lines)
            {
                if (line.StartsWith("v ")) // Vertex definition
                {
                    String temp = line.Substring(2);
                    String[] vertparts = temp.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                    // Attempt to parse each part of the vertice
                    Vector3 v = new Vector3();
                    float.TryParse(vertparts[0], style, culture, out v.X);
                    float.TryParse(vertparts[1], style, culture, out v.Y);
                    float.TryParse(vertparts[2], style, culture, out v.Z);
                    vertices.Add(v);
                    //float.TryParse(vertparts[0], style, culture, out vec.X);
                    //float.TryParse(vertparts[1], style, culture, out vec.Y);
                    //float.TryParse(vertparts[2], style, culture, out vec.Z);
                }
                if (line.StartsWith("vn ")) // Vertex definition
                {
                    String temp = line.Substring(2);
                    String[] vertparts = temp.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                    // Attempt to parse each part of the vertice
                    for (int i = 0; i < 3; i++)
                    {
                        float el;
                        float.TryParse(vertparts[i], style, culture, out el);
                        normals.Add(el);
                    }
                }
                if (line.StartsWith("vt ")) // Vertex definition
                {
                    String temp = line.Substring(2);
                    String[] vertparts = temp.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                    // Attempt to parse each part of the vertice
                    Vector2 t = new Vector2();
                    float.TryParse(vertparts[0], style, culture, out t.X);
                    float.TryParse(vertparts[1], style, culture, out t.Y);
                 //   t.Y = 1 - t.Y;
                    textCoord.Add(t);
                }
                if (line.StartsWith("f ")) // Vertex definition
                {

                    String temp = line.Substring(2);
                    String[] vertparts = temp.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    int i = 0;
                    foreach (var l in vertparts)
                    {
                        if (i > 2)
                            break;
                        bool success;
                        int el;
                        string[] elems = l.Split('/');
                        if (!(success = int.TryParse(elems[0], out el)))
                            break;
                        vertIndi.Add(int.Parse(elems[0])-1);
                        textInd.Add(int.Parse(elems[1])-1);
                        normInd.Add(int.Parse(elems[2])-1);
                        i++;
                    }
                    // Attempt to parse each part of the vertice
                }
            }
            MyComplexObjFactory obj = new MyComplexObjFactory();
            obj.vertices = CompliteVert(vertices, vertIndi);
            obj.vertInd = Enumerable.Range(0, obj.vertices.Length).ToArray();
            obj.textureCoordinates = CompliteTextCoord(textCoord,textInd);
            return obj;
        }

        private static Vector3[] CompliteVert(List<Vector3> vert,List<int> indices)
        {
            Vector3[] outVert = new Vector3[indices.Count];
            int i = 0;
            foreach(var ind in indices)
            {
                outVert[i++] = vert[ind];
            }
            return outVert;
        }
        private static Vector2[] CompliteTextCoord(List<Vector2> textCoord, List<int> indices)
        {
            Vector2[] outTextCoord = new Vector2[indices.Count];
            int i = 0;
            foreach (var ind in indices)
            {
                outTextCoord[i++] = textCoord[ind];
            }
            return outTextCoord;
        }

        public override Vector3[] GetVerts()
        {
            return vertices;
        }

        public override Vector3[] GetColorData()
        {
            throw new NotImplementedException();
        }

        public override Vector2[] GetTextureCoords()
        {
            return textureCoordinates;
        }
    }
}
