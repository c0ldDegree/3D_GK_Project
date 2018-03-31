using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GK_OpenTK
{
   public class Light
    {
        public Vector3 position { get; set; }
        public int location { get; }//location in a shader program
        public Light(int loc,Vector3 pos)
        {
            location = loc;
            position = pos;
        }

      //public  Vector3 intensities;//a.k.a. the color of the light
      //public  float attenuation;//rozpuszczenie
      //public  float ambientCoefficient;
      //public  float coneAngle;
      //public  Vector3 coneDirection;
    }
}
