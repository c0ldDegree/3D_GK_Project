using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace GK_OpenTK.Camera
{
    class StaticCamera : ICamera
    {
        public Matrix4 LookAtMatrix
        {
            get;
        }

        public Vector3 offset
        {
            get;

            set;
        }
        public StaticCamera()
        {
            Vector3 possition;
            possition.X = possition.Y = 0;
            possition.Z = 0;
            LookAtMatrix = Matrix4.LookAt(possition, -Vector3.UnitZ, Vector3.UnitY);
        }
        public void Update(double time, double dt)
        {
      
        }
    }
}
