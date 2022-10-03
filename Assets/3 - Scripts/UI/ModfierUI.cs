using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModfierUI : MonoBehaviour
{
    public GameObject modifierUISlot;
    public Transform modifiersParent; 

    private List<ModifierUISlot> slots = new List<ModifierUISlot>();

    // Start is called before the first frame update
    void Start()
    {
        GameEvents.current.onUpdateModifierGUI += UpdateUI;
    }

    void UpdateUI(List<Modifier> items, int maxSlots)
    {
        foreach (Transform child in modifiersParent.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        for (int i = 0; i < maxSlots; i++)
        {
            GameObject instantiatedSlot = Instantiate(modifierUISlot);
            instantiatedSlot.transform.SetParent(modifiersParent);
            Image slotImage = instantiatedSlot.GetComponentInChildren<Image>();
            if (i < items.Count)
            {
                slotImage.enabled = true;
                slotImage.sprite = items[i].sprite;
            }
            else
            {
                slotImage.enabled = false;
            }
            slots.Add(instantiatedSlot.GetComponentInChildren<ModifierUISlot>());
        }
    }
}
