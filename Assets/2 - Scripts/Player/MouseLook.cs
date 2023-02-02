using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float mouseSensitivity = 150.0f;

    private float mouseX;
    private float mouseY;
    private float xRotation;
    private float yRotation;
    private float multiplier = 1.0f;
    private Camera playerCamera; 
    private Rigidbody _rb;
    
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        playerCamera = GetComponentInChildren<Camera>();
        _rb = GetComponent<Rigidbody>();

    }

    private void Update()
    {
        MyInput();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        _rb.MoveRotation(Quaternion.Euler(_rb.transform.localRotation.eulerAngles));
        transform.rotation = Quaternion.Euler(0f, yRotation, 0f);
    }

    private void MyInput()
    {
        mouseX = Input.GetAxisRaw("Mouse X");
        mouseY = Input.GetAxisRaw("Mouse Y");

        yRotation += mouseX * mouseSensitivity * multiplier * Time.deltaTime;
        xRotation -= mouseY * mouseSensitivity * multiplier * Time.deltaTime;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
    }
}
