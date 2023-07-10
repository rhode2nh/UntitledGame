using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputRaycast : MonoBehaviour
{
    float maxDistance;
    public bool isHitting;
    public RaycastHit hit;

    private void Start()
    {
        isHitting = false;
        maxDistance = 10.0f;
    }

    private void FixedUpdate()
    {
        Vector3 fwd = transform.TransformDirection(Vector3.forward);
        isHitting = Physics.Raycast(transform.position, fwd, out hit, maxDistance);
        Debug.DrawRay(transform.position, fwd * maxDistance, Color.green);
        DisplayHoverText();
    }

    public void DisplayHoverText()
    {
        if (isHitting) {
            var tag = hit.transform.gameObject.tag;
            if (tag == Constants.WORLD_ITEM) {
                Debug.Log("Here");
                var objectHit = hit.transform.gameObject.GetComponent<WorldItem>();
                GameEvents.current.UpdateHoverText("Pick up " + objectHit.item.name);
            } else if (tag == Constants.BUTTON) {
                GameEvents.current.UpdateHoverText("Press E to execute");
            }
        } else {
            GameEvents.current.UpdateHoverText("");
        }
    }
}
