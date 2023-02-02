using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItem : MonoBehaviour
{
    public SpawnCommand spawnCommand;
    private void Update()
    {
        spawnCommand.position = transform.position;
        spawnCommand.rotation = transform.rotation;
    }
    public void DropInventoryItem(GameObject item)
    {
        Instantiate(item, transform.position, transform.rotation);
    }
}
