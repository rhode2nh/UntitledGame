using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Inventory inventoryObject;
    private InventoryUIController inventoryUIController;
    private InputRaycast inputRaycast;
    private DropItem dropItemSpawner;
    public bool isDebug;

    // Movement
    public float speed = 5.0f;
    public float movementMultiplier = 10.0f;
    public float drag = 6.0f;

    private Rigidbody _body;
    private float _horizontalMovement;
    private float _verticalMovement;
    private Vector3 _moveDirection;

    // Start is called before the first frame update
    void Start()
    {
        inputRaycast = GetComponentInChildren<InputRaycast>();
        dropItemSpawner = GetComponentInChildren<DropItem>();
        _body = GetComponent<Rigidbody>();
        _body.freezeRotation = true;
        inventoryUIController = GetComponent<InventoryUIController>();
    }

    // Movement
    private void Update()
    {
        _horizontalMovement = Input.GetAxisRaw("Horizontal") * Time.deltaTime;
        _verticalMovement = Input.GetAxisRaw("Vertical") * Time.deltaTime;
        ControlDrag();
        _moveDirection = transform.forward * _verticalMovement + transform.right * _horizontalMovement;
    }

    private void FixedUpdate()
    {
        _body.AddForce(_moveDirection.normalized * movementMultiplier * speed, ForceMode.Acceleration);
    }

    private void ControlDrag()
    {
        _body.drag = drag;
    }

    public void PickUpItem(WorldItem item)
    {
        inventoryObject.AddItem(item.item, item.count);
        if (isDebug)
        {
            PrintPickUpItem(item);
        }
        Destroy(inputRaycast.hit.transform.gameObject);
    }

    public void DropLastItemInInventory()
    {
        if (inventoryObject.NumUniqueItems() > 0)
        {
            Item item = inventoryObject.RemoveLastItem();
            GameObject itemToDrop = item.prefab;
            dropItemSpawner.DropInventoryItem(itemToDrop);
            if (isDebug)
            {
                PrintDropItem(item.prefab.GetComponent<WorldItem>());
            }
        }
    }

    public void DropItemInInventory(Item item)
    {
        if (inventoryObject.NumUniqueItems() > 0)
        {
            Item removedItem = inventoryObject.RemoveItem(item);
            GameObject itemToDrop = removedItem.prefab;
            dropItemSpawner.DropInventoryItem(itemToDrop);
            if (isDebug)
            {
                PrintDropItem(removedItem.prefab.GetComponent<WorldItem>());
            }
        }
    }

    public void HandleInteractable()
    {
        if (inputRaycast.isHitting)
        {
            var interactable = inputRaycast.hit.transform.gameObject;
            switch (interactable.tag)
            {
                case Constants.WORLD_ITEM:
                    PickUpItem(interactable.GetComponent<WorldItem>());
                    break;
                default:
                    Debug.Log("I don't know what to do with this: " + inputRaycast.hit.transform.gameObject.tag);
                    break;
            }
        }
    }

    public void Jump()
    {
        
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
