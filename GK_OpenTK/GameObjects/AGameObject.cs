using OpenTK;
using OpenTK.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;
using GK_OpenTK.Camera;
using GK_OpenTK.Renderable2;

namespace GK_OpenTK.GameObjects
{
    abstract class AGameObject
    {
        public bool rot_dir =false;
        protected ARenderable2 _model;
        protected Vector4 _possition;
        protected Vector4 _rotation;
        protected Vector3 _direction = new Vector3(0, 0, -1);
        protected float _speed = 1f;
        protected float _scale = 1;
        protected Quaternion quat;
        protected Vector3 _center;
        protected Matrix4 _modelMatrix;
        protected Matrix4 _viewMatrix;
        protected Matrix4 _scaleMatrix;
        public ARenderable2 model => _model;
        public Vector4 possition => _possition;
        public Vector4 rotation => _rotation;
        public Vector3 direction => _direction;
        public Matrix4 viewMatrix => _viewMatrix;
        public Matrix4 modelMatrix => _modelMatrix;
        public float ang = 0;
        public float pom_angle = 0;
        public float camera_angle = 0;

        public AGameObject(ARenderable2 model, Vector4 rotation, Vector4 possition, Vector3 center, float scale)
        {
            _model = model;
            _rotation = rotation;
            _possition = possition;
            _scale = scale;
            quat = Quaternion.Identity;
            _center = center * scale;
            _center = new Vector3(_center.X + possition.X, _center.Y + possition.Y, _center.Z + possition.Z);
        }
        public virtual void Render(ICamera camera)
        {
            _model.Bind();
            var t2 = Matrix4.CreateTranslation(_possition.X, _possition.Y, _possition.Z);
            var s = Matrix4.CreateScale(_scale);
            // var r = Matrix4.CreateFromQuaternion(quat);
            var p1 = Matrix4.CreateTranslation(-_center);
            var r = Matrix4.CreateRotationY(ang);
            var p2 = Matrix4.CreateTranslation(_center);
            r = p1 * r * p2;
            _modelMatrix = s * t2 * r;
            _viewMatrix = camera.LookAtMatrix;
            GL.UniformMatrix4(20, false, ref _modelMatrix);
            GL.UniformMatrix4(21, false, ref _viewMatrix);
            _model.Render();
        }
        public void Rotate(float angle,List<Light> lights)
        {
            //  quat = Quaternion.FromAxisAngle(GL., angle * (float)Math.PI / 180f);

            if (rot_dir)
            {
                var r = Matrix3.CreateRotationY(-angle * (float)Math.PI / 180);
                _direction = Vector3.Normalize(r * _direction);
                camera_angle += -angle * (float)Math.PI / 180;
                //  var s = Matrix4.CreateScale(_scale);
                // var r = Matrix4.CreateFromQuaternion(quat);
                var t2 = Matrix4.CreateTranslation(_possition.X, _possition.Y, _possition.Z);
                var p1 = Matrix4.CreateTranslation(-_center);
                var r_l = Matrix4.CreateRotationY((-angle * (float)Math.PI / 180) / 20);
                var p2 = Matrix4.CreateTranslation(_center);
                r_l = p1 * r_l * p2;
                var _modelMatrix = t2 * r_l;
                for (int i = 0; i < 2; i++)
                {
                    lights[i].position = new Matrix3(_modelMatrix) * lights[i].position;
                }

            }
            angle = ang * 180 / (float)Math.PI + angle;
            ang = angle * (float)Math.PI / 180;
        }

        public void Move(List<Light> lights)
        {
            _center = _center + _direction * _speed;
            _possition = _possition + new Vector4(_direction * _speed);
            for (int i = 0; i < 3; i++)
                lights[i].position += direction * _speed;
        }
        public void SetPosition(Vector4 position)
        {
            _possition = position;
        }
    }
}
