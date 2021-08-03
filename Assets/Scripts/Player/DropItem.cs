using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItem : MonoBehaviour
{
    public void DropInventoryItem(GameObject item)
    {
        Instantiate(item, transform.position, transform.rotation);
    }
}
