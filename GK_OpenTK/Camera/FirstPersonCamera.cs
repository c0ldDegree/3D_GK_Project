using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using GK_OpenTK.GameObjects;

namespace GK_OpenTK.Camera
{
    class FirstPersonCamera : ICamera
    {
        private readonly AGameObject _target;
        private Vector3 _offset;
        public Matrix4 LookAtMatrix
        {
            get; private set;
        }

        public Vector3 offset
        {
            get
            {
                return _offset;
            }

            set
            {
                _offset = value;
            }
        }
        public FirstPersonCamera(AGameObject target, Vector3 offset)
        {
            _target = target;
            _offset = offset;
            Vector3 possition;
            possition.X = _target.possition.X; possition.Y = _target.possition.Y;possition.Z = _target.possition.Z;
            LookAtMatrix = Matrix4.LookAt(new Vector3(_target.possition) + _offset, new Vector3(_target.possition), Vector3.UnitY);
        }
        public FirstPersonCamera(AGameObject target) : this(target, Vector3.Zero) { }

        public void Update(double time, double dt)
        {
       
        LookAtMatrix = Matrix4.LookAt(new Vector3(_target.possition) + _offset, new Vector3(_target.possition), Vector3.UnitY);// good one//
    
        }
    }
}
