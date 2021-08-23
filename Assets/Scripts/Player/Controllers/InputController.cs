using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    private PlayerController playerController;
    private InventoryUIController inventoryUIController;

    void Start()
    {
        inventoryUIController = GetComponent<InventoryUIController>();
        playerController = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
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
}
