using UnityEngine;

public class InventoryUIController : MonoBehaviour
{
    //private MouseLook mouseLook;
    public GameObject inventoryUI;
    
    // Start is called before the first frame update
    void Start()
    {
        //mouseLook = GetComponent<MouseLook>();
    }

    // Update is called once per frame
    void Update()
    {
    }
    
    public void OpenInventory()
    {
        inventoryUI.SetActive(true);
        //Time.timeScale = 0;
        //mouseLook.enabled = false;
        Cursor.lockState = CursorLockMode.None;
    }

    public void CloseInventory()
    {
        inventoryUI.SetActive(false);
        //Time.timeScale = 1;
        //mouseLook.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
