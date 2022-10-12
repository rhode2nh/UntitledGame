using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public struct Output
{
    public Modifier projectile;
    public List<Output> postModifiers;

    public Output(Modifier projectile)
    {
        this.projectile = projectile;
        postModifiers = new List<Output>();
    }
}

public class EquipmentContainer : MonoBehaviour
{
    public int curEquipmentIndex;
    public Mesh equipmentMesh;
    public EquipmentManager equipmentManager;
    public GameObject equipmentContainer;
    public Transform projectileSpawner;
    public Camera mainCamera;
    public LayerMask layerMask;

    public List<Modifier> modifiers;
    public List<int> modifierSlotIndices;
    public List<Output> lastOutput;
    public bool isAttacking;
    public bool isRecharging;
    public bool coroutineStarted;
    public int curModifierIndex;
    public int curGroupIndex;
    public int maxSlots;
    public float gunCastDelay;
    public float gunRechargeTime;
    public float gunXSpread;
    public float gunYSpread;
    public float totalXSpread;
    public float totalYSpread;
    public float totalCastDelay;
    public float totalRechargeTime;
    public AudioSource gunShotAudio;

    private Vector3[] _meshVertices;
    private InventorySlot _currentItem;
    private List<int> _usedCastXIds;
    private List<int> _usedModifierIds;
    private int _projectilesToGroup;
    private List<Output> _curOutput;

    // Start is called before the first frame update
    void Start()
    {
        GameEvents.current.onUpdateEquipmentContainer += UpdateEquipmentContainer;
        GameEvents.current.onRemoveModifierFromWeapon += RemoveModifierFromWeapon;
        GameEvents.current.onGetCurrentWeapon += GetCurrentWeapon;
        GameEvents.current.onUpdateCurrentWeapon += UpdateCurrentWeapon;
        
        modifiers = new List<Modifier>();
        modifierSlotIndices = new List<int>();
        _curOutput = new List<Output>();
        lastOutput = new List<Output>();
        _usedCastXIds = new List<int>();
        _usedModifierIds = new List<int>();
        curEquipmentIndex = 0;
        maxSlots = 0;
        isAttacking = false;
        isRecharging = false;
        curModifierIndex = 0;
        curGroupIndex = 0;
        coroutineStarted = false;
        _currentItem = null;
        _projectilesToGroup = 1;
        UpdateEquipmentContainer();
    }

    void Update()
    {
        if (isAttacking && coroutineStarted == false && _currentItem != null)
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
                curGroupIndex = 0;
                _usedCastXIds.Clear();
                _usedModifierIds.Clear();
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
            _currentItem = equipmentManager.GetItem(curEquipmentIndex);
            equipmentMesh = _currentItem.item.equipmentMesh;
            _meshVertices = equipmentMesh.vertices;
            for (int i = 0; i < _meshVertices.Length; i++)
            {
                if (projectileSpawner.localPosition.x < _meshVertices[i].x)
                {
                    projectileSpawner.localPosition = _meshVertices[i];
                }
            }
            equipmentContainer.GetComponent<MeshFilter>().mesh = equipmentMesh;
            modifiers = new List<Modifier>((List<Modifier>)_currentItem.properties[Constants.P_W_MODIFIERS_LIST]);
            modifierSlotIndices = new List<int>((List<int>)_currentItem.properties[Constants.P_W_MODIFIER_SLOT_INDICES]);
            maxSlots = (int)_currentItem.properties[Constants.P_W_MAX_SLOTS];
            totalCastDelay = TotalCastDelay();
            totalRechargeTime = TotalRechargeTime();
            totalXSpread = TotalXSpread();
            totalYSpread = TotalYSpread();
            isRecharging = false;
            curModifierIndex = 0;
            curGroupIndex = 0;
            string[] test = { totalCastDelay.ToString("0.0"), totalRechargeTime.ToString("0.0"), totalXSpread.ToString("0.0"), totalYSpread.ToString("0.0")}; 
            GameEvents.current.UpdateWeaponStatsGUI(test);
        }
        else
        {
            projectileSpawner.localPosition = new Vector3(0, 0, 0);
            _currentItem = null;
            equipmentMesh = null;
            equipmentContainer.GetComponent<MeshFilter>().mesh = null;
            modifiers.Clear();
            modifierSlotIndices.Clear();
            gunCastDelay = 0.0f;
            gunRechargeTime = 0.0f;
            totalCastDelay = 0.0f;
            totalRechargeTime = 0.0f;
            totalXSpread = 0.0f;
            totalYSpread = 0.0f;
            isRecharging = false;
            curModifierIndex = 0;
            curGroupIndex = 0;
            maxSlots = 0;
            GameEvents.current.UpdateWeaponStatsGUI(new string[] {"0.0", "0.0", "0.0", "0.0"});
        }
        GameEvents.current.UpdateModifierGUI(modifiers, modifierSlotIndices, maxSlots);
    }

    IEnumerator Attack()
    {
        List<List<Output>> firstPass = CalculateFirstPass();
        List<List<Output>> secondPass = CalculateSecondPass(firstPass);
        Debug.Log(secondPass[0][0].postModifiers.Count);
        secondPass = RemoveNonProjectiles(secondPass);

        // No projectiles are in the weapon
        if (secondPass.Count == 0)
        {
            GameEvents.current.RechargeDelayBarLoading();
            yield return new WaitForSeconds(totalRechargeTime);
            coroutineStarted = false;
            yield break;
        }

        InstantiateOutput(secondPass[curGroupIndex]);
        curGroupIndex++;

        if (curGroupIndex == secondPass.Count)
        {
            isRecharging = true;
            yield return new WaitForEndOfFrame();
            GameEvents.current.RechargeDelayBarLoading();
            yield return new WaitForSeconds(totalRechargeTime);
            isRecharging = false;
            coroutineStarted = false;
            curGroupIndex = 0;
        }
        else
        {
            yield return new WaitForEndOfFrame();
            GameEvents.current.CastDelayBarLoading();
            yield return new WaitForSeconds(totalCastDelay);
            coroutineStarted = false;
        }
    }

    private List<List<Output>> CalculateFirstPass()
    {
        List<List<Output>> firstPass = new List<List<Output>>();
        List<Output> currentGroup = new List<Output>();
        List<int> potentialWrapModifiers = new List<int>();
        bool hasWrapped = false;
        int projectilesToGroup = 1;

        for (int i = 0; i < modifiers.Count; i++)
        {
            if (modifiers[i] is IProjectile)
            {
                if (modifiers[i] is ITrigger)
                {
                    currentGroup.Add(new Output(modifiers[i]));
                    potentialWrapModifiers.Add(i);
                }
                else
                {
                    currentGroup.Add(new Output(modifiers[i]));
                    projectilesToGroup--;
                }
            }

            else if (modifiers[i] is ICastX)
            {
                var castX = modifiers[i] as ICastX;
                projectilesToGroup += castX.ModifiersPerCast;
                currentGroup.Add(new Output(modifiers[i]));
                potentialWrapModifiers.Add(i);
                projectilesToGroup--;
            }
            
            // TODO: Figure out what to do with the rest of modifiers
            else
            {
                firstPass.Add(new List<Output>());
            }

            // We've reached the end of the list, check if we need to wrap
            if (i == modifiers.Count - 1 && projectilesToGroup > 0)
            {
                firstPass.Add(new List<Output>(currentGroup));
                hasWrapped = true;
                break;
            }

            if (projectilesToGroup == 0)
            {
                firstPass.Add(new List<Output>(currentGroup));
                potentialWrapModifiers.Clear();
                currentGroup = new List<Output>();
                projectilesToGroup = 1;
            }
        }

        if (hasWrapped)
        {
            // Add modifiers until we reach the modifier that caused a wrap
            for (int i = 0; i < projectilesToGroup; i++)
            {
                if (potentialWrapModifiers.Contains(i))
                {
                    break;
                }
                else
                {
                    firstPass[firstPass.Count - 1].Add(new Output(modifiers[i]));
                }
            }
        }

        return firstPass;
    }

    private List<List<Output>> CalculateSecondPass(List<List<Output>> firstPass)
    {
        List<List<Output>> secondPass = new List<List<Output>>();

        for (int i = 0; i < firstPass.Count; i++)
        {
            secondPass.Add(new List<Output>());
            int postProjectilesToGroup = 0;
            int triggerIndex = 0;
            bool foundTrigger = false;
            for (int j = 0; j < firstPass[i].Count; j++)
            {
                // First occurence of a trigger
                if (firstPass[i][j].projectile is ITrigger)
                {
                    if (foundTrigger == false)
                    {
                        foundTrigger = true;
                        triggerIndex = j;
                        secondPass[i].Add(firstPass[i][j]);
                        postProjectilesToGroup++;
                        continue;
                    }
                    else
                    {
                        secondPass[i][triggerIndex].postModifiers.Add(firstPass[i][j]);
                    }
                }
                else if (firstPass[i][j].projectile is ICastX)
                {
                    if (foundTrigger)
                    {
                        var castX = firstPass[i][j].projectile as ICastX;
                        // TODO: Not sure if this is needed
                        //secondPass[i][triggerIndex].postModifiers.Add(firstPass[i][j]);
                        postProjectilesToGroup += castX.ModifiersPerCast;
                        postProjectilesToGroup--;
                    }
                    else
                    {
                        secondPass[i].Add(firstPass[i][j]);
                    }
                }
                else
                {
                    if (foundTrigger)
                    {
                        secondPass[i][triggerIndex].postModifiers.Add(firstPass[i][j]);
                        postProjectilesToGroup--;
                    }
                    else
                    {
                        secondPass[i].Add(firstPass[i][j]);
                    }
                }
                if (foundTrigger && postProjectilesToGroup == 0)
                {
                    break;
                }
            }
        }

        return secondPass;
    }

    private List<List<Output>> RemoveNonProjectiles(List<List<Output>> firstPass)
    {
        List<List<Output>> nonEmpty = firstPass.Where(x => x.Count != 0).ToList();
        List<List<Output>> finalPass = new List<List<Output>>();

        for (int i = 0; i < nonEmpty.Count(); i++)
        {
            finalPass.Add(new List<Output>());
            for (int j = 0; j < nonEmpty[i].Count; j++)
            {
                if (nonEmpty[i][j].projectile is IProjectile)
                {
                    finalPass[i].Add(nonEmpty[i][j]);
                }
            }
        }

        return finalPass.Where(x => x.Count != 0).ToList();
    }

    private void InstantiateOutput(List<Output> output)
    {
        foreach (var modifier in output)
        {
            var projectile = modifier.projectile as IProjectile;
            Ray ray = mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hit;

            Vector3 targetPoint;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
            {
                targetPoint = hit.point;
            }
            else
            {
                targetPoint = ray.GetPoint(75);
            }

            Vector3 directionWithoutSpread = targetPoint - projectileSpawner.position;

            float x = Random.Range(-totalXSpread * 0.5f, totalXSpread * 0.5f);
            float y = Random.Range(-totalYSpread * 0.5f, totalYSpread * 0.5f);
            
            Vector3 directionWithSpread = Quaternion.AngleAxis(x, projectileSpawner.up) * Quaternion.AngleAxis(y, projectileSpawner.forward) * directionWithoutSpread;

            var instantiatedProjectile = Instantiate(projectile.ProjectilePrefab, projectileSpawner.position, Quaternion.identity);
            var triggerList = instantiatedProjectile.GetComponent<TriggerList>();
            if (triggerList != null)
            {
                triggerList.triggerList = modifier.postModifiers; 
                triggerList.cameraDirection = ray.direction;
                triggerList.hitNormal = hit.normal;
            }
            instantiatedProjectile.transform.forward = directionWithSpread.normalized;
            instantiatedProjectile.GetComponent<Rigidbody>().AddForce(instantiatedProjectile.transform.forward * 4, ForceMode.Impulse);
            //TODO: Create destroy property in projectile interface
            Object.Destroy(instantiatedProjectile, 1.0f);
        }
        //gunShotAudio.Play(0);
    }

    private float TotalXSpread()
    {
        float totalXSpread = 0.0f;
        if (_currentItem.item is IWeapon)
        {
            var weapon = _currentItem.item as IWeapon;
            totalXSpread = weapon.XSpread;
            foreach (var modifier in modifiers)
            {
                totalXSpread += modifier.XSpread;
            }
        }
        return totalXSpread < 0.0f ? 0.01f : totalXSpread;
    }
    
    private float TotalYSpread()
    {
        float totalYSpread = 0.0f;
        if (_currentItem.item is IWeapon)
        {
            var weapon = _currentItem.item as IWeapon;
            totalYSpread = weapon.YSpread;
            foreach (var modifier in modifiers)
            {
                totalYSpread += modifier.YSpread;
            }
        }
        return totalYSpread < 0.0f ? 0.01f : totalYSpread;
    }

    private float TotalCastDelay()
    {
        float totalCastDelay = 0.0f;
        if (_currentItem.item is IWeapon)
        {
            var weapon = _currentItem.item as IWeapon;
            gunCastDelay = weapon.CastDelay;
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
        if (_currentItem.item is IWeapon)
        {
            var weapon = _currentItem.item as IWeapon;
            gunRechargeTime = weapon.RechargeTime;
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

    //TODO: Cleanup function
    public void RemoveModifierFromWeapon(int modifierIndex, int modifierSlotIndex)
    {
        if (modifierIndex == -1)
        {
            return;
        }
        Modifier modifier = modifiers[modifierIndex];
        modifiers.RemoveAt(modifierIndex);
        modifierSlotIndices.RemoveAt(modifierIndex);
        _currentItem.properties[Constants.P_W_MODIFIERS_LIST] = new List<Modifier>(modifiers);
        _currentItem.properties[Constants.P_W_MODIFIER_SLOT_INDICES] = new List<int>(modifierSlotIndices);
        equipmentManager.RemoveItem(curEquipmentIndex);
        equipmentManager.equipmentInventory.items.Insert(curEquipmentIndex, _currentItem);
        var inventorySlot = new InventorySlot(modifier.GetInstanceID(), modifier, 1);
        GameEvents.current.AddItemToPlayerInventory(inventorySlot);
        UpdateEquipmentContainer();
    }

    public InventorySlot GetCurrentWeapon(bool test)
    {
        if (equipmentManager.equipmentInventory.items.Count == 0)
        {
            return null;
        }
        return equipmentManager.GetItem(curEquipmentIndex);;
    }

    public void UpdateCurrentWeapon(InventorySlot updatedWeapon)
    {
        equipmentManager.equipmentInventory.items[curEquipmentIndex] = new InventorySlot(updatedWeapon);
        UpdateEquipmentContainer();
    }
}
