using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EquipmentContainer : MonoBehaviour
{
    public int curEquipmentIndex;
    public Mesh equipmentMesh;
    public EquipmentManager equipmentManager;
    public GameObject equipmentContainer;
    public Transform projectileSpawner;

    private Vector3[] meshVertices;
    public List<TestModifier> modifiers;
    private bool isAttacking;
    private bool coroutineStarted;

    // Start is called before the first frame update
    void Start()
    {
        GameEvents.current.onUpdateEquipmentContainer += UpdateEquipmentContainer;
        modifiers = new List<TestModifier>();
        curEquipmentIndex = 0;
        isAttacking = false;
        coroutineStarted = false;
        UpdateEquipmentContainer();
    }

    void Update()
    {
        if (isAttacking && coroutineStarted == false)
        {
            coroutineStarted = true;
            StartCoroutine(Attack());
        }
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

    IEnumerator Attack()
    {
        if (equipmentManager.equipmentInventory.items.Count == 0)
        {
            yield return null;
        }
        else
        {
            var test = equipmentManager.equipmentInventory.items[curEquipmentIndex].item as IWeapon;
            float time = test.RechargeTime;
            Debug.Log("Recharge Time: " + time);
            yield return new WaitForSeconds(time);
        }
        coroutineStarted = false;
        //if (modifiers.Count == 0)
        //{
        //    var test = equipmentManager.equipmentInventory.items[curEquipmentIndex].item as IWeapon;
        //    float time = test.RechargeTime;
        //    yield return new WaitForSeconds(time);
        //}
    }

    public void setIsAttacking(bool isAttacking)
    {
        this.isAttacking = isAttacking;
    }
}
