using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IImplant
{
   BodyPart BodyPart { get; }
   int QualityLevel { get; set; }

   // float Strength { get; set; }
   // float RunSpeed { get; set; } 
}
