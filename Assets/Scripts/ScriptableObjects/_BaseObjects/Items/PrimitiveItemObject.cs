using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PrimitiveItemObject : Item
{
    private void Awake()
    {
        this.Id = Constants.PRIMITIVE_ITEM_BASE_ID;
        this.Name = Constants.PRIMITIVE_ITEM_BASE;
        this.type = ItemType.PRIMITIVE;
    }
}
