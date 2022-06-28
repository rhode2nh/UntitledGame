using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public struct Output
{
    public Modifier projectile;

    public Output(Modifier projectile)
    {
        this.projectile = projectile;
    }
}

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
    public bool isRecharging;
    public int curModifierIndex;
    public int curProjectileIndex;
    public float totalCastDelay;
    public float totalRechargeTime;
    public List<Output> curOutput;
    public List<Output> lastOutput;
    public List<int> usedCastXIds;
    public List<int> usedModifierIds;
    public int projectilesToGroup;

    public bool coroutineStarted;
    private InventorySlot currentItem;

    // Start is called before the first frame update
    void Start()
    {
        GameEvents.current.onUpdateEquipmentContainer += UpdateEquipmentContainer;
        
        modifiers = new List<Modifier>();
        curOutput = new List<Output>();
        lastOutput = new List<Output>();
        usedCastXIds = new List<int>();
        usedModifierIds = new List<int>();
        curEquipmentIndex = 0;
        isAttacking = false;
        isRecharging = false;
        curModifierIndex = 0;
        curProjectileIndex = 0;
        coroutineStarted = false;
        currentItem = null;
        projectilesToGroup = 1;
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
            if (isRecharging)
            {
                curModifierIndex = 0;
                curProjectileIndex = 0;
                usedCastXIds.Clear();
                usedModifierIds.Clear();
            }
            StopAllCoroutines();
            coroutineStarted = false;
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
            modifiers = new List<Modifier>((List<Modifier>)currentItem.properties[Constants.P_W_MODIFIERS_LIST]);
            totalCastDelay = TotalCastDelay();
            totalRechargeTime = TotalRechargeTime();
            isRecharging = false;
        }
        else
        {
            projectileSpawner.localPosition = new Vector3(0, 0, 0);
            currentItem = null;
            equipmentMesh = null;
            equipmentContainer.GetComponent<MeshFilter>().mesh = null;
            modifiers.Clear();
            totalCastDelay = 0.0f;
            totalRechargeTime = 0.0f;
            isRecharging = false;
            curModifierIndex = 0;
            curProjectileIndex = 0;
        }
    }

    IEnumerator Attack()
    {
        var weapon = currentItem.item as IWeapon;
        projectilesToGroup = 1;

        if (modifiers.Count == 0)
        {
            yield return new WaitForSeconds(totalRechargeTime);
            coroutineStarted = false;
            yield break;
        }

        bool hasWrapped = false;
        while (projectilesToGroup != 0)
        {
            var modifier = modifiers[curModifierIndex];
            if (modifier is IProjectile)
            {
                curOutput.Add(new Output(modifier));
            }
            
            else if (modifier is ICastX)
            {
                if (usedCastXIds.Contains(curModifierIndex))
                {
                    usedCastXIds.Clear();
                    projectilesToGroup = 1;
                    break;
                }
                var castX = modifier as ICastX;
                projectilesToGroup += castX.ModifiersPerCast;
                usedCastXIds.Add(curModifierIndex);
            }

            else
            {
                if (!usedModifierIds.Contains(curModifierIndex))
                {
                    usedModifierIds.Add(curModifierIndex);
                }
                curModifierIndex++;
                continue;
            }

            if (!usedModifierIds.Contains(curModifierIndex))
            {
                usedModifierIds.Add(curModifierIndex);
            }

            curModifierIndex++;
            projectilesToGroup--;

            if (curModifierIndex >= modifiers.Count())
            {
                curModifierIndex = curModifierIndex % modifiers.Count();
                hasWrapped = true;
            }
        }

        InstantiateOutput(curOutput);
        lastOutput = new List<Output>(curOutput);

        if (usedModifierIds.Count() >= modifiers.Count())
        {
            usedModifierIds.Clear();
            usedCastXIds.Clear();
            isRecharging = true;
            curOutput.Clear();
            yield return new WaitForSeconds(totalRechargeTime);
            projectilesToGroup = 1;
            isRecharging = false;
            coroutineStarted = false;
        }
        else
        {
            if (hasWrapped)
            {
                hasWrapped = false;
                curModifierIndex = 0;
            }
            curOutput.Clear();
            usedModifierIds.Clear();
            usedCastXIds.Clear();
            yield return new WaitForSeconds(totalCastDelay);
            coroutineStarted = false;
        }
    }

    private void InstantiateOutput(List<Output> output)
    {
        foreach (var modifier in output)
        {
            var projectile = modifier.projectile as IProjectile;
            var instantiatedProjectile = Instantiate(projectile.ProjectilePrefab, projectileSpawner.position, projectileSpawner.rotation);
            instantiatedProjectile.GetComponent<Rigidbody>().AddForce(projectileSpawner.transform.right * 1000);
        }
    }

    private float TotalCastDelay()
    {
        float totalCastDelay = 0.0f;
        if (currentItem.item is IWeapon)
        {
            var weapon = currentItem.item as IWeapon;
            totalCastDelay = weapon.CastDelay;
            foreach (var modifier in modifiers)
            {
                totalCastDelay += modifier.castDelay;
            }
        }
        return totalCastDelay < 0.0f ? 0.01f : totalCastDelay;
    }

    private float TotalRechargeTime()
    {
        float totalRechargeTime = 0.0f;
        if (currentItem.item is IWeapon)
        {
            var weapon = currentItem.item as IWeapon;
            totalRechargeTime = weapon.RechargeTime;
            foreach (var modifier in modifiers)
            {
                totalRechargeTime += modifier.rechargeDelay;
            }
        }
        return totalRechargeTime < 0.0f ? 0.01f : totalRechargeTime;
    }

    public void setIsAttacking(bool isAttacking)
    {
        this.isAttacking = isAttacking;
    }
}
