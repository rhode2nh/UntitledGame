using System.Collections.Generic;
using UnityEngine;

public class EquipmentContainer : MonoBehaviour
{
    public int curEquipmentIndex;
    public Mesh equipmentMesh;
    public EquipmentManager equipmentManager;
    public GameObject equipmentContainer;
    public Transform projectileSpawner;

    private Vector3[] meshVertices;
    public List<TestModifier> modifiers;

    // Start is called before the first frame update
    void Start()
    {
        GameEvents.current.onUpdateEquipmentContainer += UpdateEquipmentContainer;
        modifiers = new List<TestModifier>();
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
            meshVertices = equipmentMesh.vertices;
            for (int i = 0; i < meshVertices.Length; i++)
            {
                if (projectileSpawner.localPosition.x < meshVertices[i].x)
                {
                    projectileSpawner.localPosition = meshVertices[i];
                }
            }
            equipmentContainer.GetComponent<MeshFilter>().mesh = equipmentMesh;
            modifiers = (List<TestModifier>)equipmentManager.equipmentInventory.items[curEquipmentIndex].properties[Constants.P_W_MODIFIERS_LIST];
            
        }
        else
        {
            projectileSpawner.localPosition = new Vector3(0, 0, 0);
            equipmentMesh = null;
            equipmentContainer.GetComponent<MeshFilter>().mesh = null;
            modifiers.Clear();
        }
    }

    void Shoot()
    {

    }
}
