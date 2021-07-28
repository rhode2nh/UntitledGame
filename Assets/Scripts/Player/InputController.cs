using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    private Inventory inventory;

    // Start is called before the first frame update
    void Start()
    {
        inventory = GetComponent<Inventory>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
            inventory.PrintItems();
        if (Input.GetKeyDown(KeyCode.F))
            inventory.AddItem(new Copper());
    }
}
