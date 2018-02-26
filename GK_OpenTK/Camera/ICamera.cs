using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GK_OpenTK.Camera
{
    interface ICamera
    {
        Matrix4 LookAtMatrix { get; }
        Vector3 offset { set; get; }
        void Update(double time, double dt);
    }
}
