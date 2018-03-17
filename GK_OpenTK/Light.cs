using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GK_OpenTK
{
   public struct Light
    {
      public Vector4 position;
      public  Vector3 intensities;//a.k.a. the color of the light
      public  float attenuation;//rozpuszczenie
      public  float ambientCoefficient;
      public  float coneAngle;
      public  Vector3 coneDirection;
    }
}
