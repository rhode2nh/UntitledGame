using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    private PlayerController playerController;
    private MouseLook mouseLook;
    private bool isInUI = false;
    public GameObject canvas;

    void Start()
    {
        playerController = GetComponent<PlayerController>();
        mouseLook = GetComponent<MouseLook>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            playerController.PickUpItem();
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            playerController.DropLastItemInInventory();
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            isInUI = !isInUI;
            if (isInUI)
            {
                canvas.SetActive(true);
                Time.timeScale = 0;
                mouseLook.enabled = false;
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                canvas.SetActive(false);
                Time.timeScale = 1;
                mouseLook.enabled = true;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
    }
}
