using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldItem : MonoBehaviour
{
    public string itemName;
    private Transform childObject;

    // Start is called before the first frame update
    void Start()
    {
        childObject = transform.GetChild(0);
        childObject.tag = Constants.WORLD_ITEM;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
