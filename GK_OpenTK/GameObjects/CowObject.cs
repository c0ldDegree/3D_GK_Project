using GK_OpenTK.Renderable2;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GK_OpenTK.GameObjects
{
    class CowObject:AGameObject
    {
        public CowObject(ARenderable2 model, Vector4 rotation, Vector4 possition,Vector3 center,float scale=1) : base(model, rotation, possition,center,scale) { } 
    }
}
