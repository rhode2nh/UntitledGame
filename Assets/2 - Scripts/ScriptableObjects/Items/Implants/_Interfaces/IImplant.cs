using System;

public interface IImplant
{
   BodyPart BodyPart { get; }
   int QualityLevel { get; set; }
   TestStats TestStats { get; set; }

   // float Strength { get; set; }
   // float RunSpeed { get; set; } 
}
