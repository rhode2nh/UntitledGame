using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentUI : MonoBehaviour
{
    public GameObject equipmentUISlot;
    public Transform equipmentParent; 

    private List<EquipmentUISlot> slots = new List<EquipmentUISlot>();

    // Start is called before the first frame update
    void Start()
    {
        GameEvents.current.onUpdateWeaponGUI += UpdateUI;
    }

    void UpdateUI(List<Item> equipment, int maxSlots)
    {
        foreach (Transform child in equipmentParent.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        for (int i = 0; i < maxSlots; i++)
        {
            GameObject instantiatedSlot = Instantiate(equipmentUISlot);
            EquipmentUISlot slot = instantiatedSlot.GetComponent<EquipmentUISlot>();
            instantiatedSlot.transform.SetParent(equipmentParent, false);
            Image slotImage = instantiatedSlot.GetComponentInChildren<Image>();
            if (i < equipment.Count)
            {
                slotImage.enabled = true;
                slotImage.sprite = equipment[i].sprite;
                slot.equipmentIndex = i;
            }
            else 
            {
                slotImage.enabled = false;
                slot.slotUIIndex = -1;
                slot.equipmentIndex = -1;
            }
        }

        //for (int i = 0; i < maxSlots; i++)
        //{
        //    GameObject instantiatedSlot = Instantiate(equipmentUISlot);
        //    EquipmentUISlot slot = instantiatedSlot.GetComponent<EquipmentUISlot>();
        //    instantiatedSlot.transform.SetParent(equipmentParent, false);
        //    Image slotImage = instantiatedSlot.GetComponentInChildren<Image>();
        //    bool found = false;
        //    for (int j = 0; j < equipmentSlotIndices.Count; j++)
        //    {
        //        if (i == equipmentSlotIndices[j])
        //        {
        //            found = true;
        //            slotImage.enabled = true;
        //            slotImage.sprite = equipment[j].sprite;
        //            slot.slotUIIndex = equipmentSlotIndices[j];
        //            slot.equipmentIndex = j;
        //            break;
        //        }
        //    }
        //    if (!found)
        //    {
        //        slotImage.enabled = false;
        //        slot.slotUIIndex = -1;
        //        slot.equipmentIndex = -1;
        //    }
        //    slots.Add(instantiatedSlot.GetComponentInChildren<EquipmentUISlot>());
        //}
    }
}
