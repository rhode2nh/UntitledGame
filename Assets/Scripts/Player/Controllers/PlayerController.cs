using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public InventoryObject inventoryObject;
    private InputRaycast inputRaycast;
    private DropItem dropItemSpawner;
    public bool isDebug;

    // Start is called before the first frame update
    void Start()
    {
        inputRaycast = GetComponentInChildren<InputRaycast>();
        dropItemSpawner = GetComponentInChildren<DropItem>();
    }

    public void PickUpInventoryItem()
    {
        if (inputRaycast.isHitting && inputRaycast.hit.transform.tag == Constants.WORLD_ITEM)
        {
            var item = inputRaycast.hit.transform.gameObject.GetComponent<WorldItem>();
            inventoryObject.AddItem(item.item, item.count);
            if (isDebug)
            {
                PrintPickUpItem(item);
            }
            Destroy(inputRaycast.hit.transform.gameObject);
        }
    }

    public void DropLastInventoryItem()
    {
        if (inventoryObject.Size() > 0)
        {
            ItemObject item = inventoryObject.RemoveLastItem();
            GameObject itemToDrop = item.prefab;
            dropItemSpawner.DropInventoryItem(itemToDrop);
            if (isDebug)
            {
                PrintDropItem(item.prefab.GetComponent<WorldItem>());
            }
        }
    }

    private void OnApplicationQuit()
    {
        inventoryObject.inventory.Clear();
    }

    #region Debug Functions
    private void PrintDropItem(WorldItem item)
    {
        string debugString = "---DROPPED ITEM---\n";
        debugString += "\tName: " + item.item.Name + "\n";
        Debug.Log(debugString);
    }
    
    private void PrintPickUpItem(WorldItem item)
    {
        string debugString = "---PICKED UP ITEM---\n";
        debugString += "\tName: " + item.item.Name + "\n";
        Debug.Log(debugString);
    }
    #endregion
}
