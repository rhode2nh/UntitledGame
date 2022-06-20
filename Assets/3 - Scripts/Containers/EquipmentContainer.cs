using UnityEngine;

public class EquipmentContainer : MonoBehaviour
{
    public int curEquipmentIndex;
    public Mesh equipmentMesh;
    public EquipmentManager equipmentManager;
    public GameObject equipmentContainer;

    // Start is called before the first frame update
    void Start()
    {
        GameEvents.current.onUpdateEquipmentContainer += UpdateEquipmentContainer;
        curEquipmentIndex = 0;
        UpdateEquipmentContainer();
    }

    public void SwitchEquipment(int index)
    {
        if (index < equipmentManager.equipmentInventory.maxSize)
        {
            curEquipmentIndex = index;
            UpdateEquipmentContainer();
        }
    }

    void UpdateEquipmentContainer()
    {
        if (equipmentManager.equipmentInventory.items.Count > 0 && curEquipmentIndex < equipmentManager.equipmentInventory.items.Count)
        {
            equipmentMesh = equipmentManager.equipmentInventory.items[curEquipmentIndex].item.equipmentMesh;
            equipmentContainer.GetComponent<MeshFilter>().mesh = equipmentMesh;
        }
        else
        {
            equipmentMesh = null;
            equipmentContainer.GetComponent<MeshFilter>().mesh = null;
        }
    }
}
