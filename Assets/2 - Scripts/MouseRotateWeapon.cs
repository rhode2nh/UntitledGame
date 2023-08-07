using UnityEngine;
using UnityEngine.InputSystem;

public class MouseRotateWeapon : MonoBehaviour
{
    public float swayMultiplier;
    public float smooth;

    // Update is called once per frame
    void Update()
    {
        float mouseX = (Mouse.current.delta.x.ReadValue() / 2) * (swayMultiplier * GameEvents.current.GetMouseSense()); 
        float mouseY = (Mouse.current.delta.y.ReadValue() / 2) * (swayMultiplier * GameEvents.current.GetMouseSense()); 

        Quaternion rotationX = Quaternion.AngleAxis(mouseY, Vector3.forward);
        Quaternion rotationY = Quaternion.AngleAxis(mouseX, Vector3.up);

        Quaternion targetRotation = rotationX * rotationY;

        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, smooth * Time.deltaTime);
    }
}
