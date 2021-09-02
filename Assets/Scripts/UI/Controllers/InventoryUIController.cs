using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUIController : MonoBehaviour
{
    //private MouseLook mouseLook;
    private bool isInUI = false;
    public GameObject inventoryUI;
    public GameObject TradeUI;

    // Start is called before the first frame update
    void Start()
    {
        //mouseLook = GetComponent<MouseLook>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void OpenTrade(ChestInventory otherInventory)
    {
        TradeUI.GetComponentInChildren<OtherInventoryUI>().otherInventory = otherInventory;
        isInUI = !isInUI;
        if (isInUI)
        {
            TradeUI.SetActive(true);
            //Time.timeScale = 0;
            //mouseLook.enabled = false;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            TradeUI.SetActive(false);
            Time.timeScale = 1;
            //mouseLook.enabled = true;
            Cursor.lockState = CursorLockMode.Locked;
        }
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
