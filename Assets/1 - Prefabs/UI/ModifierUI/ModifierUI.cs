using System.Collections.Generic;
using UnityEngine;

public class ModifierUI : MonoBehaviour
{
    public GameObject modifierUISlot;
    public Transform modifiersParent; 
    public int activeSlots;
    public int totalSlots;

    private List<ModifierUISlot> slots = new List<ModifierUISlot>();

    // Start is called before the first frame update
    void Start()
    {
        GameEvents.current.onUpdateModifierGUI += UpdateUI;
        activeSlots = 0;
        totalSlots = 0;
    }

    void UpdateUI(List<Slot> modifiers, List<int> modifierSlotIndices, int maxSlots)
    {
        if (activeSlots != maxSlots)
        {
            if (activeSlots < maxSlots)
            {
                IncreaseActiveSlots(maxSlots);
            }
            else 
            {
                DecreaseActiveSlots(maxSlots);
            }
        }

        for (int i = 0; i < totalSlots; i++)
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

    public void IncreaseActiveSlots(int maxSlots)
    {
        if (modifiersParent.childCount != 0)
        {
            for (int i = activeSlots - 1; i < maxSlots; i++)
            {
                Debug.Log("max slots: " + maxSlots + " active slots: " + activeSlots);
                slots[i].gameObject.SetActive(true);
            }
        }
        
        if (totalSlots < maxSlots)
        {
            totalSlots = maxSlots;
            for (int i = 0; i < totalSlots - activeSlots; i++)
            {
                GameObject instantiatedUISlot = Instantiate(modifierUISlot);
                instantiatedUISlot.transform.SetParent(modifiersParent, false);
                instantiatedUISlot.GetComponent<ModifierUISlot>().slot = GameEvents.current.GetEmptySlot();
                slots.Add(instantiatedUISlot.GetComponentInChildren<ModifierUISlot>());
            }
            activeSlots = maxSlots;

        }
    }

    public void DecreaseActiveSlots(int maxSlots)
    {
        for (int i = maxSlots; i >= totalSlots - maxSlots - 1; i--)
        {
            modifiersParent.GetChild(i).gameObject.SetActive(false);
        }
        activeSlots = maxSlots == 0 ? 1 : maxSlots;
    }
}
