using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Inventory inventory;
    private InputRaycast inputRaycast;
    private DropItem dropItemSpawner;
    public bool isDebug;

    // Start is called before the first frame update
    void Start()
    {
        inventory = GetComponent<Inventory>();
        inputRaycast = GetComponentInChildren<InputRaycast>();
        dropItemSpawner = GetComponentInChildren<DropItem>();
    }
    // Test methods
    public void AddItemToInventory(InventoryItem item)
    {
        //inventory.AddItem(new Copper());
    }
    
    public void PrintInventoryItems()
    {
        inventory.PrintItems();
    }

    public void PickUpInventoryItem()
    {
        if (inputRaycast.isHitting && inputRaycast.hit.transform.tag == Constants.WORLD_ITEM)
        {
            var item = inputRaycast.hit.transform.parent.gameObject.GetComponent<WorldItem>();
            inventory.AddItem(Instantiate(item.itemReference));
            if (isDebug)
            {
                PrintPickUpItem(item);
            }
            Destroy(inputRaycast.hit.transform.parent.gameObject);
        }
    }

    public void DropLastInventoryItem()
    {
        if (inventory.Count() > 0)
        {
            Item item = inventory.RemoveLastItem();
            GameObject itemToDrop = Resources.Load<GameObject>("Prefabs/WI_" + item.Name);
            itemToDrop.GetComponent<WorldItem>().UpdateItemReference(item);
            dropItemSpawner.DropInventoryItem(itemToDrop);
            if (isDebug)
            {
                PrintDropItem(itemToDrop.GetComponent<WorldItem>());
            }
        }
    }

    private void PrintDropItem(WorldItem item)
    {
        string debugString = "---DROPPED ITEM---\n";
        debugString += "\tName: " + item.itemReference.Name + "\n";
        debugString += "\tCount: " + item.itemReference.Count + "\n";
        debugString += "\tisStackable: " + item.itemReference.IsStackable + "\n";
        Debug.Log(debugString);
    }
    
    private void PrintPickUpItem(WorldItem item)
    {
        string debugString = "---PICKED UP ITEM---\n";
        debugString += "\tName: " + item.itemReference.Name + "\n";
        debugString += "\tCount: " + item.itemReference.Count + "\n";
        debugString += "\tisStackable: " + item.itemReference.IsStackable + "\n";
        Debug.Log(debugString);
    }
}
