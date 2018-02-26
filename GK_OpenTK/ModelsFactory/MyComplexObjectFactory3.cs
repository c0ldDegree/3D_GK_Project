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
    class FaceVertex2
    {
        public Vector3 Position;
        public Vector3 Normal;
        public Vector2 TextureCoord;
        public String TextName;
        public List<int> textIndecis = new List<int>();

        public FaceVertex2(Vector3 pos, Vector3 norm, Vector2 texcoord,String textName)
        {
            Position = pos;
            Normal = norm;
            TextureCoord = texcoord;
            TextName = textName;
        }
        public override bool Equals(object obj)
        {
            FaceVertex2 fv = (FaceVertex2)obj;
            if (!Position.Equals(fv.Position))
                return false;
            if (!Normal.Equals(fv.Normal))
                return false;
            if (!TextName.Equals(fv.TextName))
                return false;
            if (!TextureCoord.Equals(fv.TextureCoord))
                return false;
            return true;
        }
    }
    class MyComplexObjectFactory3:AModelFactory
    {
        private List<FaceVertex2> indexMap = new List<FaceVertex2>();
        private List<int> inds = new List<int>();
        private List<Tuple<FaceVertex2, FaceVertex2, FaceVertex2>> faces = new List<Tuple<FaceVertex2, FaceVertex2, FaceVertex2>>();

        public override int VertCount { get { return faces.Count * 3; } }

        public override int IndiceCount { get { return faces.Count * 3; } }

        public override int ColorDataCount { get { return faces.Count * 3; } }

        public override int TextureCoordsCount { get { return faces.Count * 3; } }


        public override Vector3[] GetNormals()
        {
            if (base.GetNormals().Length > 0)
            {
                return base.GetNormals();
            }

            List<Vector3> normals = new List<Vector3>();
            if (indexMap.Count != 0)
            {
                foreach (var el in indexMap)
                {
                    normals.Add(el.Normal);
                }
            }
            else
                foreach (var face in faces)
                {
                    normals.Add(face.Item1.Normal);
                    normals.Add(face.Item2.Normal);
                    normals.Add(face.Item3.Normal);
                }

            return normals.ToArray();
        }

        public override int NormalCount
        {
            get
            {
                return faces.Count * 3;
            }
        }
        public Tuple<int[],Dictionary<String,int>> GetTexturesInd(Dictionary<String, Material> materials)
        {
            int tind = 0;
            Dictionary<String, int> mapTextInd = new Dictionary<string, int>();
            foreach(var el in materials)
            {
                mapTextInd.Add(el.Key, tind);
                tind++;
            }
            List<int> tinds = new List<int>();
            if (indexMap.Count != 0)
            {
                foreach(var el in indexMap)
                {
                    int pom;
                    mapTextInd.TryGetValue(el.TextName, out pom);
                    tinds.Add(pom);
                }
            }
            return  new Tuple<int[], Dictionary<string, int>>(tinds.ToArray(),mapTextInd);
        }

        /// <summary>
        /// Get vertice data for this object
        /// </summary>
        /// <returns></returns>
        public override Vector3[] GetVerts()
        {
            List<Vector3> verts = new List<Vector3>();
            if (indexMap.Count != 0)
            {
                foreach (var el in indexMap)
                {
                    verts.Add(el.Position);
                }
            }
            else
                foreach (var face in faces)
                {
                    verts.Add(face.Item1.Position);
                    verts.Add(face.Item2.Position);
                    verts.Add(face.Item3.Position);
                }

            return verts.ToArray();
        }

        /// <summary>
        /// Get indices
        /// </summary>
        /// <param name="offset"></param>
        /// <returns></returns>
        public override int[] GetIndices(int offset = 0)
        {
            if (inds.Count != 0)
                return inds.ToArray();
            return Enumerable.Range(offset, IndiceCount).ToArray();
        }

        /// <summary>
        /// Get color data.
        /// </summary>
        /// <returns></returns>
        public override Vector3[] GetColorData()
        {
            return new Vector3[ColorDataCount];
        }

        /// <summary>
        /// Get texture coordinates.
        /// </summary>
        /// <returns></returns>
        public override Vector2[] GetTextureCoords()
        {
            List<Vector2> coords = new List<Vector2>();
            if (indexMap.Count != 0)
            {
                foreach (var el in indexMap)
                {
                    coords.Add(el.TextureCoord);
                }
            }
            else
                foreach (var face in faces)
                {
                    coords.Add(face.Item1.TextureCoord);
                    coords.Add(face.Item2.TextureCoord);
                    coords.Add(face.Item3.TextureCoord);
                }

            return coords.ToArray();
        }


        /// <summary>
        /// Loads a model from a file.
        /// </summary>
        /// <param name="filename">File to load model from</param>
        /// <returns>ObjVolume of loaded model</returns>
        public static MyComplexObjectFactory3 LoadFromFile(string filename)
        {
            MyComplexObjectFactory3 obj = new MyComplexObjectFactory3();
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
                Console.WriteLine("Error loading file: {0}.\n{1}", filename, e);
            }

            return obj;
        }
        public static MyComplexObjectFactory3 LoadFromString(string obj)
        {
            Dictionary<String, int> materials = new Dictionary<string, int>();
            String curMaterial="";

            NumberStyles style = NumberStyles.Float;
            CultureInfo culture = CultureInfo.CreateSpecificCulture("en-US");

            // Seperate lines from the file
            List<String> lines = new List<string>(obj.Split('\n'));

            // Lists to hold model data
            List<Vector3> verts = new List<Vector3>();
            List<Vector3> normals = new List<Vector3>();
            List<Vector2> texs = new List<Vector2>();
            List<Tuple<TempVertex, TempVertex, TempVertex>> faces = new List<Tuple<TempVertex, TempVertex, TempVertex>>();

            // Base values
            verts.Add(new Vector3());
            texs.Add(new Vector2());
            normals.Add(new Vector3());


            // Read file line by line
            foreach (String line in lines)
            {
                if (line.StartsWith("v ")) // Vertex definition
                {
                    // Cut off beginning of line
                    String temp = line.Substring(2);

                    Vector3 vec = new Vector3();

                    if (temp.Trim().Count((char c) => c == ' ') == 2) // Check if there's enough elements for a vertex
                    {
                        String[] vertparts = temp.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                        // Attempt to parse each part of the vertice
                        bool success = float.TryParse(vertparts[0], style, culture, out vec.X);
                        success |= float.TryParse(vertparts[1], style, culture, out vec.Y);
                        success |= float.TryParse(vertparts[2], style, culture, out vec.Z);

                        // If any of the parses failed, report the error
                        if (!success)
                        {
                            Console.WriteLine("Error parsing vertex: {0}", line);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Error parsing vertex: {0}", line);
                    }

                    verts.Add(vec);
                }
                else if (line.StartsWith("vt ")) // Texture coordinate
                {
                    // Cut off beginning of line
                    String temp = line.Substring(2);

                    Vector2 vec = new Vector2();

                    if (temp.Trim().Count((char c) => c == ' ') > 0) // Check if there's enough elements for a vertex
                    {
                        String[] texcoordparts = temp.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                        // Attempt to parse each part of the vertice
                        bool success = float.TryParse(texcoordparts[0], style, culture, out vec.X);
                        success |= float.TryParse(texcoordparts[1], style, culture, out vec.Y);

                        // If any of the parses failed, report the error
                        if (!success)
                        {
                            Console.WriteLine("Error parsing texture coordinate: {0}", line);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Error parsing texture coordinate: {0}", line);
                    }

                    texs.Add(vec);
                }
                else if (line.StartsWith("vn ")) // Normal vector
                {
                    // Cut off beginning of line
                    String temp = line.Substring(2);

                    Vector3 vec = new Vector3();

                    if (temp.Trim().Count((char c) => c == ' ') == 2) // Check if there's enough elements for a normal
                    {
                        String[] vertparts = temp.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                        // Attempt to parse each part of the vertice
                        bool success = float.TryParse(vertparts[0], style, culture, out vec.X);
                        success |= float.TryParse(vertparts[1], style, culture, out vec.Y);
                        success |= float.TryParse(vertparts[2], style, culture, out vec.Z);

                        // If any of the parses failed, report the error
                        if (!success)
                        {
                            Console.WriteLine("Error parsing normal: {0}", line);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Error parsing normal: {0}", line);
                    }

                    normals.Add(vec);
                }
                else if (line.StartsWith("f ")) // Face definition
                {
                    // Cut off beginning of line
                    String temp = line.Substring(2);

                    Tuple<TempVertex, TempVertex, TempVertex> face = new Tuple<TempVertex, TempVertex, TempVertex>(new TempVertex(), new TempVertex(), new TempVertex());

                    if (temp.Trim().Count((char c) => c == ' ') >= 2) // Check if there's enough elements for a face
                    {
                        String[] faceparts = temp.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        bool success = true;
                        if (temp.Trim().Count((char c) => c == ' ') == 3)
                        {
                            int v1, v2, v3;
                            int v11, v22, v33;
                            int t1, t2, t3;
                            int t11, t22, t33;
                            int n1, n2, n3;
                            int n11, n22, n33;

                            // Attempt to parse each part of the face
                            success = int.TryParse(faceparts[0].Split('/')[0], out v1);
                            success |= int.TryParse(faceparts[1].Split('/')[0], out v2);
                            success |= int.TryParse(faceparts[2].Split('/')[0], out v3);
                            success |= int.TryParse(faceparts[0].Split('/')[0], out v11);
                            success |= int.TryParse(faceparts[2].Split('/')[0], out v22);
                            success |= int.TryParse(faceparts[3].Split('/')[0], out v33);
                            if (faceparts[0].Count((char c) => c == '/') >= 2)
                            {
                                success |= int.TryParse(faceparts[0].Split('/')[1], out t1);
                                success |= int.TryParse(faceparts[1].Split('/')[1], out t2);
                                success |= int.TryParse(faceparts[2].Split('/')[1], out t3);
                                success |= int.TryParse(faceparts[0].Split('/')[1], out t11);
                                success |= int.TryParse(faceparts[2].Split('/')[1], out t22);
                                success |= int.TryParse(faceparts[3].Split('/')[1], out t33);
                                success |= int.TryParse(faceparts[0].Split('/')[2], out n1);
                                success |= int.TryParse(faceparts[1].Split('/')[2], out n2);
                                success |= int.TryParse(faceparts[2].Split('/')[2], out n3);
                                success |= int.TryParse(faceparts[0].Split('/')[2], out n11);
                                success |= int.TryParse(faceparts[2].Split('/')[2], out n22);
                                success |= int.TryParse(faceparts[3].Split('/')[2], out n33);
                            }
                            else { t1 = t2 = t3 = t11 = t22 = t33 = n1 = n2 = n3 = n11 = n22 = n33 = 0; }
                            // If any of the parses failed, report the error
                            if (!success)
                            {
                                Console.WriteLine("Error parsing face: {0}", line);
                            }
                            else
                            {
                                TempVertex tv1 = new TempVertex(v1, n1, t1,curMaterial);
                                TempVertex tv2 = new TempVertex(v2, n2, t2,curMaterial);
                                TempVertex tv3 = new TempVertex(v3, n3, t3,curMaterial);
                                TempVertex tv11 = new TempVertex(v11, n11, t11, curMaterial);
                                TempVertex tv22 = new TempVertex(v22, n22, t22, curMaterial);
                                TempVertex tv33 = new TempVertex(v33, n33, t33, curMaterial);
                                face = new Tuple<TempVertex, TempVertex, TempVertex>(tv1, tv2, tv3);
                                faces.Add(face);
                                face = new Tuple<TempVertex, TempVertex, TempVertex>(tv11, tv22, tv33);
                                faces.Add(face);
                            }
                        }
                        else {
                            int v1, v2, v3;
                            int t1, t2, t3;
                            int n1, n2, n3;

                            // Attempt to parse each part of the face
                            success = int.TryParse(faceparts[0].Split('/')[0], out v1);
                            success |= int.TryParse(faceparts[1].Split('/')[0], out v2);
                            success |= int.TryParse(faceparts[2].Split('/')[0], out v3);

                            if (faceparts[0].Count((char c) => c == '/') >= 2)
                            {
                                success |= int.TryParse(faceparts[0].Split('/')[1], out t1);
                                success |= int.TryParse(faceparts[1].Split('/')[1], out t2);
                                success |= int.TryParse(faceparts[2].Split('/')[1], out t3);
                                success |= int.TryParse(faceparts[0].Split('/')[2], out n1);
                                success |= int.TryParse(faceparts[1].Split('/')[2], out n2);
                                success |= int.TryParse(faceparts[2].Split('/')[2], out n3);
                            }
                            else
                            {
                                if (texs.Count > v1 && texs.Count > v2 && texs.Count > v3)
                                {
                                    t1 = v1;
                                    t2 = v2;
                                    t3 = v3;
                                }
                                else
                                {
                                    t1 = 0;
                                    t2 = 0;
                                    t3 = 0;
                                }


                                if (normals.Count > v1 && normals.Count > v2 && normals.Count > v3)
                                {
                                    n1 = v1;
                                    n2 = v2;
                                    n3 = v3;
                                }
                                else
                                {
                                    n1 = 0;
                                    n2 = 0;
                                    n3 = 0;
                                }

                            }
                            // If any of the parses failed, report the error
                            if (!success)
                            {
                                Console.WriteLine("Error parsing face: {0}", line);
                            }
                            else
                            {
                                TempVertex tv1 = new TempVertex(v1, n1, t1,curMaterial);
                                TempVertex tv2 = new TempVertex(v2, n2, t2,curMaterial);
                                TempVertex tv3 = new TempVertex(v3, n3, t3,curMaterial);
                                face = new Tuple<TempVertex, TempVertex, TempVertex>(tv1, tv2, tv3);
                                faces.Add(face);
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Error parsing face: {0}", line);
                    }
                }
               else if(line.StartsWith("usemtl "))
                {
                    String temp = line.Substring(7);
                    curMaterial = temp;
                }
            }

            // Create the ObjVolume
            MyComplexObjectFactory3 vol = new MyComplexObjectFactory3();

            foreach (var face in faces)
            {
                FaceVertex2 v1 = new FaceVertex2(verts[face.Item1.Vertex], normals[face.Item1.Normal], texs[face.Item1.Texcoord],face.Item1.matName);
                FaceVertex2 v2 = new FaceVertex2(verts[face.Item2.Vertex], normals[face.Item2.Normal], texs[face.Item2.Texcoord],face.Item2.matName);
                FaceVertex2 v3 = new FaceVertex2(verts[face.Item3.Vertex], normals[face.Item3.Normal], texs[face.Item3.Texcoord],face.Item3.matName);

                vol.faces.Add(new Tuple<FaceVertex2, FaceVertex2, FaceVertex2>(v1, v2, v3));
            }
            vol.mapIndices(vol.faces);
            return vol;
        }
        private void mapIndices(List<Tuple<FaceVertex2, FaceVertex2, FaceVertex2>> faces)
        {
            int pIndex = 0;
            Dictionary<FaceVertex2, int> pDict = new Dictionary<FaceVertex2, int>();
            foreach (var f in faces)
            {
                pIndex = addEl(f.Item1, pIndex, pDict);
                pIndex = addEl(f.Item2, pIndex, pDict);
                pIndex = addEl(f.Item3, pIndex, pDict);
            }
        }
        private int addEl(FaceVertex2 el, int pIndex, Dictionary<FaceVertex2, int> pDict)
        {
            if (!pDict.ContainsKey(el))
            {
                pDict.Add(el, pIndex);
                indexMap.Add(el);
                inds.Add(pIndex);
                pIndex++;
            }
            else
            {
                int value = -1;
                pDict.TryGetValue(el, out value);
                if (value == -1)
                    throw new ArgumentException("Something with dictionary in mapIndices method");
                inds.Add(value);
            }
            return pIndex;
        }
        private class TempVertex
        {
            public int Vertex;
            public int Normal;
            public int Texcoord;
            public String matName;
            public TempVertex(int vert = 0, int norm = 0, int tex = 0,String mat="")
            {
                Vertex = vert;
                Normal = norm;
                Texcoord = tex;
                matName = mat;
            }
        }
    }
}
