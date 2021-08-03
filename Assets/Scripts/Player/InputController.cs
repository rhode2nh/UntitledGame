using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    private Inventory inventory;
    private InputRaycast inputRaycast;
    private DropItem dropItemSpawner;

    // Start is called before the first frame update
    void Start()
    {
        inventory = GetComponent<Inventory>();
        inputRaycast = GetComponentInChildren<InputRaycast>();
        dropItemSpawner = GetComponentInChildren<DropItem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
            inventory.PrintItems();
        if (Input.GetKeyDown(KeyCode.F))
            inventory.AddItem(new Copper());
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (inputRaycast.isHitting && inputRaycast.hit.transform.tag == Constants.WORLD_ITEM)
            {
                inventory.AddItem(inputRaycast.hit.transform.parent.gameObject.GetComponent<WorldItem>().itemName);
                Destroy(inputRaycast.hit.transform.parent.gameObject);
                Debug.Log("Picked up item");
            }
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            if (inventory.isItemInInventory(Constants.SPHERE))
            {
                InventoryItem item = inventory.RemoveItem(Constants.SPHERE);
                GameObject itemToDrop = Resources.Load<GameObject>("Prefabs/WI_" + item.Name);
                dropItemSpawner.DropInventoryItem(itemToDrop);
            }
        }
    }
}
