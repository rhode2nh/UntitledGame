using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    private PlayerController playerController;
    private InventoryUIController inventoryUIController;
    public GameObject developerConsole;
    private bool isInUI = false;

    void Start()
    {
        inventoryUIController = GetComponent<InventoryUIController>();
        playerController = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isInUI)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                playerController.HandleInteractable();
            }
            if (Input.GetKeyDown(KeyCode.X))
            {
                playerController.DropLastItemInInventory();
            }
            if (Input.GetKeyDown(KeyCode.I))
            {
                inventoryUIController.OpenInventory();
            }
        }
        if (Input.GetKeyDown(KeyCode.BackQuote))
        {
            isInUI = !isInUI;
            developerConsole.GetComponent<DeveloperConsoleBehavior>().Toggle();
        }
        if (Input.GetKeyDown(KeyCode.UpArrow) && isInUI)
        {
            developerConsole.GetComponent<DeveloperConsoleBehavior>().PreviousCommand();
        }
        if (Input.GetKeyDown(KeyCode.DownArrow) && isInUI)
        {
            developerConsole.GetComponent<DeveloperConsoleBehavior>().NextCommand();
        }
    }
}
