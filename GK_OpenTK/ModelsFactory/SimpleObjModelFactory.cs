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
    class SimpleObjModelFactory:AModelFactory
    {
        List<Tuple<uint, uint, uint>> faces = new List<Tuple<uint, uint, uint>>();

        public override int[] GetIndices(int offset = 0)
        {
            List<int> temp = new List<int>();

            foreach (var face in faces)
            {
                temp.Add((int)(face.Item1 + offset));
                temp.Add((int)(face.Item2 + offset));
                temp.Add((int)(face.Item3 + offset));
            }
            return temp.ToArray();
        }
        public static SimpleObjModelFactory LoadFromFile(string filename)
        {
            SimpleObjModelFactory obj = new SimpleObjModelFactory();
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

        public static SimpleObjModelFactory LoadFromString(string obj)
        {
            NumberStyles style = NumberStyles.Float;
            CultureInfo culture = CultureInfo.CreateSpecificCulture("en-US");

            // Seperate lines from the file
            List<String> lines = new List<string>(obj.Split('\n'));

            // Lists to hold model data
            List<Vector3> verts = new List<Vector3>();
            List<Vector3> colors = new List<Vector3>();
            List<Vector2> texs = new List<Vector2>();
            List<Tuple<uint, uint, uint>> faces = new List<Tuple<uint, uint, uint>>();

            // Read file line by line
            foreach (String line in lines)
            {
                if (line.StartsWith("v ")) // Vertex definition
                {
                    // Cut off beginning of line
                    String temp = line.Substring(2);

                    Vector3 vec = new Vector3();

                    if (temp.Count((char c) => c == ' ') == 2) // Check if there's enough elements for a vertex
                    {
                        String[] vertparts = temp.Split(' ');

                        // Attempt to parse each part of the vertice
                        bool success = float.TryParse(vertparts[0],style,culture,out vec.X);
                        success |= float.TryParse(vertparts[1], style, culture, out vec.Y);
                        success |= float.TryParse(vertparts[2], style, culture, out vec.Z);

                        // Dummy color/texture coordinates for now
                        colors.Add(new Vector3((float)Math.Sin(vec.Z), (float)Math.Sin(vec.Z), (float)Math.Sin(vec.Z)));
                        texs.Add(new Vector2((float)Math.Sin(vec.Z), (float)Math.Sin(vec.Z)));

                        // If any of the parses failed, report the error
                        if (!success)
                        {
                            Console.WriteLine("Error parsing vertex: {0}", line);
                        }
                    }

                    verts.Add(vec);
                }
                else if (line.StartsWith("f ")) // Face definition
                {
                    // Cut off beginning of line
                    String temp = line.Substring(2);

                    Tuple<uint, uint, uint> face = new Tuple<uint, uint, uint>(0, 0, 0);

                    if (temp.Count((char c) => c == ' ') == 2) // Check if there's enough elements for a face
                    {
                        String[] faceparts = temp.Split(' ');

                        int i1, i2, i3;

                        // Attempt to parse each part of the face
                        bool success = int.TryParse(faceparts[0], out i1);
                        success |= int.TryParse(faceparts[1], out i2);
                        success |= int.TryParse(faceparts[2], out i3);

                        // If any of the parses failed, report the error
                        if (!success)
                        {
                            Console.WriteLine("Error parsing face: {0}", line);
                        }
                        else
                        {
                            // Decrement to get zero-based vertex numbers
                            face = new Tuple<uint, uint, uint>((uint)(i1 - 1),(uint)(i2 - 1),(uint)(i3 - 1));
                            faces.Add(face);
                        }
                    }
                }
            }

            // Create the ObjVolume
            SimpleObjModelFactory vol = new SimpleObjModelFactory();
            vol.vertices = verts.ToArray();
            vol.faces = new List<Tuple<uint, uint, uint>>(faces);
            vol.colors = colors.ToArray();
            vol.textureCoordinates = texs.ToArray();

            return vol;
        }

        public override Vector3[] GetVerts()
        {
            throw new NotImplementedException();
        }

        public override Vector3[] GetColorData()
        {
            throw new NotImplementedException();
        }

        public override Vector2[] GetTextureCoords()
        {
            throw new NotImplementedException();
        }
    }
}
