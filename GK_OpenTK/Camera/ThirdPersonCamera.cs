using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using GK_OpenTK.GameObjects;

namespace GK_OpenTK.Camera
{
    class ThirdPersonCamera : ICamera
    {
        private float cRadius = 10f;
        public Matrix4 LookAtMatrix
        {
            get; private set;
        }

        public Vector3 offset
        {
            set
            {
                _offset = value;
            }
            get { return _offset; }
        }

        private AGameObject _target;
        private Vector3 _offset;

        public ThirdPersonCamera(AGameObject target, Vector3 offset)
        {
            _target = target;
            _offset = offset;
            Vector3 possition;
            possition.X = possition.Y = possition.Z = 0;
            LookAtMatrix = Matrix4.LookAt(possition, -Vector3.UnitZ, Vector3.UnitY);
        }
        public ThirdPersonCamera(AGameObject target) : this(target, Vector3.Zero) { }
        public void Update(double time, double dt)
        {
          //  Matrix4.CreateTranslation(0f, 0f, -cRadius);
            LookAtMatrix = Matrix4.LookAt(new Vector3(_target.possition) + (_offset * new Vector3(0,0,-1)), new Vector3(_target.possition), Vector3.UnitY);
        }
    }
}
