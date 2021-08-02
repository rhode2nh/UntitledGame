using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    private Inventory inventory;
    private InputRaycast inputRaycast;

    // Start is called before the first frame update
    void Start()
    {
        inventory = GetComponent<Inventory>();
        inputRaycast = GetComponentInChildren<InputRaycast>();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
            inventory.PrintItems();
        if (Input.GetKeyDown(KeyCode.F))
            inventory.AddItem(new Copper());
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (inputRaycast.isHitting && inputRaycast.hit.transform.tag == "WIBase")
            {
                Destroy(inputRaycast.hit.transform.gameObject);
                Debug.Log("Picked up item");
            }
        }
    }
}
