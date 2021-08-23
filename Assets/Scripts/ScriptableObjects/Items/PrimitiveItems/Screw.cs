using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Screw : PrimitiveItemObject
{
    private void Awake()
    {
        this.Id = Constants.SCREW_ID;
        this.Name = Constants.SCREW;
    }
}
