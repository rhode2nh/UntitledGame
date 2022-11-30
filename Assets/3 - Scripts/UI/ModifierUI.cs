using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModifierUI : MonoBehaviour
{
    public GameObject modifierUISlot;
    public Transform modifiersParent; 

    private List<ModifierUISlot> slots = new List<ModifierUISlot>();

    // Start is called before the first frame update
    void Start()
    {
        GameEvents.current.onUpdateModifierGUI += UpdateUI;
    }

    void UpdateUI(List<Modifier> modifiers, List<int> modifierSlotIndices, int maxSlots)
    {
        foreach (Transform child in modifiersParent.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        for (int i = 0; i < maxSlots; i++)
        {
            GameObject instantiatedSlot = Instantiate(modifierUISlot);
            ModifierUISlot slot = instantiatedSlot.GetComponent<ModifierUISlot>();
            instantiatedSlot.transform.SetParent(modifiersParent, false);
            Image slotImage = instantiatedSlot.GetComponentInChildren<Image>();
            bool found = false;
            for (int j = 0; j < modifierSlotIndices.Count; j++)
            {
                if (i == modifierSlotIndices[j])
                {
                    found = true;
                    slotImage.enabled = true;
                    slotImage.sprite = modifiers[j].sprite;
                    slot.slotUIIndex = modifierSlotIndices[j];
                    slot.modifierIndex = j;
                    break;
                }
            }
            if (!found)
            {
                slotImage.enabled = false;
                slot.slotUIIndex = -1;
                slot.modifierIndex = -1;
            }
            slots.Add(instantiatedSlot.GetComponentInChildren<ModifierUISlot>());
        }
    }
}
