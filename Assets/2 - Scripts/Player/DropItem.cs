using System.Collections;
using ExtensionMethods;
using System.Collections.Generic;
using UnityEngine;

public class DropItem : MonoBehaviour
{
    public SpawnCommand spawnCommand;
    private void Awake() {
        GameEvents.current.onDropItem += OnDropItem;
    }
    private void Update()
    {
        spawnCommand.position = transform.position;
        spawnCommand.rotation = transform.rotation;
    }
    public void DropInventoryItem(GameObject item)
    {
        Instantiate(item, transform.position, transform.rotation);
    }

    public void OnDropItem(Slot slot) {
        GameObject itemToDrop = DatabaseManager.instance.GetPrefabItem(slot.item.Id);
        GameObject instantiatedItem = Instantiate(itemToDrop, transform.position, new Quaternion());
        instantiatedItem.GetComponent<Rigidbody>().AddForce(transform.forward * 2.0f, ForceMode.Impulse);
        instantiatedItem.GetComponent<WorldItem>().properties = slot.properties.CopyProperties();
    }
}
