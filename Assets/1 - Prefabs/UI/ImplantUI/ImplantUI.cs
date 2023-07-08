using System.Collections.Generic;
using UnityEngine;

public class ImplantUI : MonoBehaviour
{
    public Transform implantsParent;

     private ImplantUISlot[] slots;

    void Awake()
    {
        slots = implantsParent.GetComponentsInChildren<ImplantUISlot>();
        GameEvents.current.onUpdateImplantGUI += UpdateUI;
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].ClearSlot();
        }
    }

    private void UpdateUI(List<Slot> items)
    {
        // if the id is -1, then it's the empty item
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].item != GameEvents.current.GetEmptyItem())
            {
                slots[i].AddItem(items[i]);
            }
            else
            {
                slots[i].ClearSlot();
            }

        }
    }
}
