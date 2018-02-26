using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GK_OpenTK.ModelsFactory
{
    public abstract class AModelFactory
    {
        protected Vector3[] vertices;
        protected Vector3[] colors=new Vector3[0];
        protected Vector2[] textureCoordinates;
        protected Vector3[] normals=new Vector3[0];

        public Vector3[] Vertices => vertices;
        public Vector3[] Colors => colors;
        public Vector2[] TextureCoordinates => textureCoordinates;
        public Vector3[] Normals => normals;
        public abstract int[] GetIndices(int offset = 0);
        public virtual int NormalCount { get { return Normals.Length; } }
        public virtual int VertCount { get; set; }
        public virtual int IndiceCount { get; set; }
        public virtual int ColorDataCount { get; set; }
        public virtual int TextureCoordsCount { get; set; }
        public abstract Vector3[] GetVerts();
        public abstract Vector3[] GetColorData();
        public abstract Vector2[] GetTextureCoords();
        

        public virtual Vector3[] GetNormals()
        {
            return normals;
        }

        public virtual void CalculateNormals()
        {
            Vector3[] verts = GetVerts();
            int[] inds = GetIndices();
            Vector3[] normals = new Vector3[VertCount];

            // Compute normals for each face
            for (int i = 0; i < inds.Length; i += 3)
            {
                Vector3 v1 = verts[inds[i]];
                Vector3 v2 = verts[inds[i + 1]];
                Vector3 v3 = verts[inds[i + 2]];

                // The normal is the cross-product of two sides of the triangle
                normals[inds[i]] += Vector3.Cross(v2 - v1, v3 - v1);
                normals[inds[i + 1]] += Vector3.Cross(v2 - v1, v3 - v1);
                normals[inds[i + 2]] += Vector3.Cross(v2 - v1, v3 - v1);
            }

            for (int i = 0; i < normals.Length; i++)
            {
                normals[i] = normals[i].Normalized();
            }

            this.normals = normals;
        }
        public Vector3 CalculateObjectCenter()
        {
            Vector3[] vertices = this.GetVerts();
            float maxX=vertices[0].X, maxY=vertices[0].Y, maxZ=vertices[0].Z, minX=vertices[0].X, minY=vertices[0].Y, minZ=vertices[0].Z;
            foreach(var v in vertices)
            {
                if (v.X > maxX) maxX = v.X;
                if (v.Y > maxY) maxY = v.Y;
                if (v.Z > maxZ) maxZ = v.Z;
                if (v.X < minX) minX = v.X;
                if (v.Y < minY) minY = v.Y;
                if (v.Z < minZ) minZ = v.Z;
            }
            return new Vector3((minX + maxX) / 2, (minY + maxY) / 2, (minZ + maxZ) / 2);
        }

    }
}
