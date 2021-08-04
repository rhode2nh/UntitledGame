using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    private PlayerController playerController;

    void Start()
    {
        playerController = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
            playerController.PrintInventoryItems();
        if (Input.GetKeyDown(KeyCode.F))
        {
            //playerController.AddItemToInventory(new Copper());
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            playerController.PickUpInventoryItem();
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            playerController.DropLastInventoryItem();
        }
    }
}
