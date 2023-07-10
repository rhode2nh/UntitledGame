using System.Collections.Generic;
using UnityEngine;

public class ModifierUI : MonoBehaviour
{
    public GameObject modifierUISlot;
    public Transform modifiersParent; 
    public int totalSlots;

    private List<ModifierUISlot> slots = new List<ModifierUISlot>();

    // Start is called before the first frame update
    void Awake()
    {
        GameEvents.current.onUpdateModifierGUI += UpdateUI;
        totalSlots = 0;
    }

    void UpdateUI(List<Slot> modifiers, List<int> modifierSlotIndices, int maxSlots)
    {
        if (maxSlots > totalSlots)
        {
            for (int i = 0; i < maxSlots - totalSlots; i++)
            {
                var newSlot = Instantiate(modifierUISlot);
                newSlot.transform.SetParent(modifiersParent, false);
                newSlot.GetComponent<ModifierUISlot>().slot = GameEvents.current.GetEmptySlot();
                slots.Add(newSlot.GetComponent<ModifierUISlot>());
            }
            totalSlots = maxSlots;
        }
        else if(maxSlots < totalSlots)
        {
            for (int i = slots.Count - 1; i > maxSlots - 1; i--)
            {
                slots[i].GetComponent<ModifierUISlot>().slot = GameEvents.current.GetEmptySlot();
                slots[i].gameObject.SetActive(false);
            }
        }
        for (int i = 0; i < maxSlots; i++)
        {
            slots[i].gameObject.SetActive(true);
            slots[i].index = i;
        }
        for (int i = 0; i < maxSlots; i++)
        {
            if (modifiers[i].item != GameEvents.current.GetEmptyItem())
            {
                slots[i].AddItem(modifiers[i]);
                slots[i].modifierIndex = i;
            }
            else
            {
                slots[i].ClearSlot();
                slots[i].modifierIndex = -1;
            }
        }
    }

}
