using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public InventoryObject inventoryObject;
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
    }

    // Movement
    private void Update()
    {
        _horizontalMovement = Input.GetAxisRaw("Horizontal");
        _verticalMovement = Input.GetAxisRaw("Vertical");
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

    public void PickUpItem()
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

    public void DropLastItemInInventory()
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
