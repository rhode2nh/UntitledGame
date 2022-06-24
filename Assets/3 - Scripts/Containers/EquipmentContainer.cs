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
    public List<Modifier> modifiers;
    public bool isAttacking;
    public int curModifierIndex;
    private bool coroutineStarted;
    private InventorySlot currentItem;

    // Start is called before the first frame update
    void Start()
    {
        GameEvents.current.onUpdateEquipmentContainer += UpdateEquipmentContainer;
        
        modifiers = new List<Modifier>();
        curEquipmentIndex = 0;
        isAttacking = false;
        curModifierIndex = 0;
        coroutineStarted = false;
        currentItem = null;
        UpdateEquipmentContainer();
    }

    void Update()
    {
        if (isAttacking && coroutineStarted == false && currentItem != null)
        {
            coroutineStarted = true;
            StartCoroutine(Attack());
        }
    }

    public void SwitchEquipment(int index)
    {
        if (index < equipmentManager.MaxSize())
        {
            curEquipmentIndex = index;
            UpdateEquipmentContainer();
        }
    }

    void UpdateEquipmentContainer()
    {
        if (!equipmentManager.IsEmpty() && curEquipmentIndex < equipmentManager.Count())
        {
            currentItem = equipmentManager.GetItem(curEquipmentIndex);
            equipmentMesh = currentItem.item.equipmentMesh;
            meshVertices = equipmentMesh.vertices;
            for (int i = 0; i < meshVertices.Length; i++)
            {
                if (projectileSpawner.localPosition.x < meshVertices[i].x)
                {
                    projectileSpawner.localPosition = meshVertices[i];
                }
            }
            equipmentContainer.GetComponent<MeshFilter>().mesh = equipmentMesh;
            modifiers = (List<Modifier>)currentItem.properties[Constants.P_W_MODIFIERS_LIST];
            
        }
        else
        {
            projectileSpawner.localPosition = new Vector3(0, 0, 0);
            currentItem = null;
            equipmentMesh = null;
            equipmentContainer.GetComponent<MeshFilter>().mesh = null;
            modifiers.Clear();
        }
    }

    IEnumerator Attack()
    {
        var weapon = currentItem.item as IWeapon;
        float finalRechargeTime = weapon.RechargeTime;

        if (modifiers.Count == 0)
        {
            Debug.Log("Recharge Time: " + finalRechargeTime);
            yield return new WaitForSeconds(finalRechargeTime);
            coroutineStarted = false;
            yield break;
        }
        
        for (int i = curModifierIndex; i < modifiers.Count; i++)
        {
            var finalCastDelay = modifiers[curModifierIndex].castDelay + weapon.CastDelay;
            var projectile = modifiers[curModifierIndex] as IProjectile;
            var instantiatedProjectile = Instantiate(projectile.ProjectilePrefab, projectileSpawner.position, projectileSpawner.rotation);
            instantiatedProjectile.GetComponent<Rigidbody>().AddForce(projectileSpawner.transform.right * 1000);
            Debug.Log("Cast Delay: " + finalCastDelay);
            curModifierIndex++;
            if (curModifierIndex == modifiers.Count)
            {
                break;
            }
            yield return new WaitForSeconds(finalCastDelay);
            if (!isAttacking)
            {
                coroutineStarted = false;
                yield break;
            }

        }

        Debug.Log("Recharge Time: " + finalRechargeTime);
        yield return new WaitForSeconds(finalRechargeTime);
        coroutineStarted = false;
        curModifierIndex = 0;
    }

    public void setIsAttacking(bool isAttacking)
    {
        this.isAttacking = isAttacking;
    }
}
