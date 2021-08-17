using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUIController : MonoBehaviour
{
    private MouseLook mouseLook;
    private bool isInUI = false;
    public GameObject canvas;

    // Start is called before the first frame update
    void Start()
    {
        mouseLook = GetComponent<MouseLook>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void OpenInventory()
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
