using System.Collections;
using ExtensionMethods;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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

    public void OnDropItem(Slot slot, bool useMousePos = false) {
        if (useMousePos) {
            Vector3 mousePos = Mouse.current.position.ReadValue();
            mousePos.z = Camera.main.nearClipPlane + 1;
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
            GameObject itemToDrop = DatabaseManager.instance.GetPrefabItem(slot.item.Id);
            GameObject instantiatedItem = Instantiate(itemToDrop, worldPos, new Quaternion());
            instantiatedItem.GetComponent<Rigidbody>().AddForce((worldPos - Camera.main.transform.position) * 2.0f, ForceMode.Impulse);
            instantiatedItem.GetComponent<WorldItem>().properties = slot.properties.CopyProperties();

        } else {
            GameObject itemToDrop = DatabaseManager.instance.GetPrefabItem(slot.item.Id);
            GameObject instantiatedItem = Instantiate(itemToDrop, transform.position, new Quaternion());
            instantiatedItem.GetComponent<Rigidbody>().AddForce(transform.forward * 2.0f, ForceMode.Impulse);
            instantiatedItem.GetComponent<WorldItem>().properties = slot.properties.CopyProperties();
        }
    }
}
