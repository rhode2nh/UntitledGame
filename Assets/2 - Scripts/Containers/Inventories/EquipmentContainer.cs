using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public struct Output
{
    public Slot slot;
    public List<Output> postModifiers;
    public Slot potentialTrajectory;

    public Output(Slot slot)
    {
        this.slot = slot;
        postModifiers = new List<Output>();
        potentialTrajectory = new Slot();
    }
}

public class EquipmentContainer : MonoBehaviour, IDataPersistence
{
    public int curEquipmentIndex;
    public GameObject instantiatedGun;
    public EquipmentManager equipmentManager;
    public GameObject equipmentContainer;
    public Transform projectileSpawner;
    public Camera mainCamera;
    public LayerMask layerMask;
    public bool inGas;
    public GasProps curGasProps;

    public List<Slot> modifierSlots;
    public List<int> modifierSlotIndices;
    public List<Output> lastOutput;
    public bool isAttacking;
    public bool isRecharging;
    public bool coroutineStarted;
    public bool shooting;
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
    private Slot _currentItem;
    private List<int> _usedCastXIds;
    private List<int> _usedModifierIds;
    private List<Output> _curOutput;

    private Animator gunAnimator;

    void Awake()
    {
        GameEvents.current.onUpdateEquipmentContainer += UpdateEquipmentContainer;
        GameEvents.current.onRemoveModifierFromWeapon += RemoveModifierFromWeapon;
        GameEvents.current.onGetCurrentWeapon += GetCurrentWeapon;
        GameEvents.current.onUpdateCurrentWeapon += UpdateCurrentWeapon;
        GameEvents.current.onRemoveWeaponFromEquipmentInventory += RemoveWeaponFromEquipmentInventory;
        GameEvents.current.onGetCurEquipmentIndex += GetCurEquipmentIndex;
        modifierSlots = new List<Slot>();
        // gunAnimator = equipmentContainer.GetComponent<Animator>();
        modifierSlotIndices = new List<int>();
        _curOutput = new List<Output>();
        lastOutput = new List<Output>();
        _usedCastXIds = new List<int>();
        _usedModifierIds = new List<int>();
        curEquipmentIndex = 0;
        maxSlots = 0;
        isAttacking = false;
        isRecharging = false;
        shooting = false;
        inGas = false;
        curModifierIndex = 0;
        curGroupIndex = 0;
        coroutineStarted = false;
        _currentItem = null;
    }

    void Start() {
        equipmentManager = EquipmentManager.instance;
    }

    void Update()
    {
        if (isAttacking && coroutineStarted == false && _currentItem != null)
        {
            coroutineStarted = true;
            StartCoroutine(Attack());
        }

        if (shooting)
        {
            gunAnimator.SetTrigger("TrRecoil");
            shooting = false;
        }
    }

    public void SwitchEquipment(int index)
    {
        GameEvents.current.SwitchActiveEquipmentUISlot(curEquipmentIndex, index);
        curEquipmentIndex = index;
        if (isRecharging)
        {
            curModifierIndex = 0;
            curGroupIndex = 0;
            _usedCastXIds.Clear();
            _usedModifierIds.Clear();
        }
        GameEvents.current.StopLoadingBars();
        StopAllCoroutines();
        coroutineStarted = false;
        UpdateEquipmentContainer();
    }

    void UpdateEquipmentContainer()
    {
        _currentItem = equipmentManager.equipmentInventory.items[curEquipmentIndex];
        if (_currentItem.item != GameEvents.current.GetEmptyItem())
        {
            if (instantiatedGun != null)
            {
                Destroy(instantiatedGun);
            }
            instantiatedGun = Instantiate(_currentItem.item.testPrefab, equipmentContainer.transform);
            gunAnimator = instantiatedGun.GetComponentInChildren<Animator>();
            instantiatedGun.transform.parent = equipmentContainer.transform;
            instantiatedGun.transform.localPosition = instantiatedGun.GetComponent<Gun>().gunPos;
            projectileSpawner.localPosition = instantiatedGun.GetComponent<Gun>().bulletSpawnPos.localPosition + instantiatedGun.GetComponent<Gun>().gunPos;
            modifierSlots = new List<Slot>((List<Slot>)_currentItem.properties[Constants.P_W_MODIFIERS_LIST]);
            modifierSlotIndices = new List<int>((List<int>)_currentItem.properties[Constants.P_W_MODIFIER_SLOT_INDICES_LIST]);
            maxSlots = (int)_currentItem.properties[Constants.P_W_MAX_SLOTS_INT];
            GameEvents.current.UpdateModifierGUI(modifierSlots, modifierSlotIndices, maxSlots);
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
            Destroy(instantiatedGun);
            equipmentContainer.GetComponent<MeshFilter>().mesh = null;
            for (int i = 0; i < modifierSlots.Count; i++)
            {
                modifierSlots[i] = GameEvents.current.GetEmptySlot();
            }
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
            GameEvents.current.UpdateModifierGUI(modifierSlots, modifierSlotIndices, maxSlots);
        }
        GameEvents.current.UpdateWeaponGUI(equipmentManager.GetAllEquipment());
    }

    //TODO: Refactor this code so the output is only recalculated when the modifiers are changed
    IEnumerator Attack()
    {
        List<List<Output>> firstPass = CalculateFirstPass();
        // PrintOutput(firstPass);
        List<List<Output>> secondPass = CalculateSecondPass(firstPass);
        // PrintOutput(secondPass);
        List<List<Output>> thirdPass = ApplyTrajectories(secondPass);
        PrintOutput(thirdPass);
        secondPass = RemoveNonProjectiles(thirdPass);

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
        List<Slot> filteredModifierSlots = modifierSlots.Where(x => x.item != GameEvents.current.GetEmptyItem()).ToList();

        for (int i = 0; i < filteredModifierSlots.Count; i++)
        {
            var curSlot = filteredModifierSlots[i];
            if (curSlot.item == GameEvents.current.GetEmptyItem())
            {
                continue;
            }
            var curModifier = filteredModifierSlots[i].item as Modifier;
            if (curModifier is IProjectile)
            {
                if (curModifier is ITrigger)
                {
                    currentGroup.Add(new Output(curSlot));
                    potentialWrapModifiers.Add(i);
                }
                else
                {
                    currentGroup.Add(new Output(curSlot));
                    projectilesToGroup--;
                }
            }

            else if (curModifier is ICastX)
            {
                var castX = curModifier as ICastX;
                projectilesToGroup += castX.ModifiersPerCast;
                currentGroup.Add(new Output(curSlot));
                potentialWrapModifiers.Add(i);
                projectilesToGroup--;
            }

            else if (curModifier is ITrajectory) {
                currentGroup.Add(new Output(curSlot));
            }
            
            // TODO: Figure out what to do with the rest of modifiers
            else
            {
                // firstPass.Add(new List<Output>());
                // projectilesToGroup--;
            }


            // We've reached the end of the list, check if we need to wrap
            if (i == filteredModifierSlots.Count - 1 && projectilesToGroup > 0)
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
                if (modifierSlots[i].item is ICastX)
                {
                    var castX = modifierSlots[i].item as ICastX;
                    projectilesToGroup += castX.ModifiersPerCast;
                }
                else if (modifierSlots[i].item is ITrigger)
                {
                    projectilesToGroup += 1;
                }
                if (potentialWrapModifiers.Contains(i))
                {
                    break;
                }
                else
                {
                    firstPass[firstPass.Count - 1].Add(new Output(modifierSlots[i]));
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
                var curModifier = firstPass[i][j].slot.item;
                // First occurence of a trigger
                if (curModifier is ITrigger)
                {
                    if (!foundTrigger)
                    {
                        foundTrigger = true;
                        secondPass[i].Add(firstPass[i][j]);
                        triggerIndex = secondPass[i].Count - 1;
                        postProjectilesToGroup++;
                        continue;
                    }
                    else
                    {
                        secondPass[i][triggerIndex].postModifiers.Add(firstPass[i][j]);
                    }
                }
                else if (curModifier is ICastX)
                {
                    if (foundTrigger)
                    {
                        var castX = curModifier as ICastX;
                        // TODO: Not sure if this is needed
                        secondPass[i][triggerIndex].postModifiers.Add(firstPass[i][j]);
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
                    foundTrigger = false;
                }
            }
        }

        return secondPass;
    }

    private List<List<Output>> ApplyTrajectories(List<List<Output>> secondPass) {
        List<List<Output>> thirdPass = new List<List<Output>>();
        int potentialTrajectoriesToApply = 0;
        

        for (int i = 0; i < secondPass.Count; i++) {
            thirdPass.Add(new List<Output>());
            Slot trajectoryModifier = new Slot();
            for (int j = 0; j < secondPass[i].Count; j++) {
                var curModifier = secondPass[i][j].slot.item;
                if (curModifier is ICastX) {
                    potentialTrajectoriesToApply += ((ICastX)curModifier).ModifiersPerCast;
                } else if (curModifier is ITrajectory) {
                    trajectoryModifier = new Slot(secondPass[i][j].slot);
                    potentialTrajectoriesToApply += 1;
                } else if (curModifier is IProjectile) {
                    var projectileWithTrajectory = secondPass[i][j];
                    projectileWithTrajectory.potentialTrajectory = new Slot(trajectoryModifier);
                    thirdPass[i].Add(projectileWithTrajectory);
                    potentialTrajectoriesToApply -= 1;
                }
            }
        }

        return thirdPass;
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
                if (nonEmpty[i][j].slot.item is IProjectile || nonEmpty[i][j].slot.item is ITrajectory)
                {
                    finalPass[i].Add(nonEmpty[i][j]);
                }
            }
        }

        return finalPass.Where(x => x.Count != 0).ToList();
    }

    private void InstantiateOutput(List<Output> output)
    {
        float recoilKnockback = 0.0f; 
        foreach (var modifier in output)
        {
            var projectile = modifier.slot.item as IProjectile;
            recoilKnockback += projectile.Knockback;
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

            float x = UnityEngine.Random.Range(-totalXSpread * 0.5f, totalXSpread * 0.5f);
            float y = UnityEngine.Random.Range(-totalYSpread * 0.5f, totalYSpread * 0.5f);
            
            Vector3 directionWithSpread = Quaternion.AngleAxis(x, projectileSpawner.up) * Quaternion.AngleAxis(y, projectileSpawner.forward) * directionWithoutSpread;

            var instantiatedProjectile = Instantiate(projectile.ProjectilePrefab, projectileSpawner.position, Quaternion.identity);
            if (modifier.potentialTrajectory.item != null) {
                instantiatedProjectile.GetComponent<RaycastProjectile>().currentTrajectory = (ITrajectory)modifier.potentialTrajectory.item;
                instantiatedProjectile.GetComponent<RaycastProjectile>().RandomizeProperties();
            }
            var triggerList = instantiatedProjectile.GetComponent<TriggerList>();
            if (triggerList != null) {
                triggerList.triggerList = modifier.postModifiers; 
                triggerList.xSpread = totalXSpread;
                triggerList.ySpread = totalYSpread;
            }
            instantiatedProjectile.transform.forward = directionWithSpread.normalized;
            var shootable = instantiatedProjectile.GetComponent<IShootable>();
            if (shootable != null) {
                shootable.Shoot();
            }
            if (inGas) {
                var raycastProjectile = instantiatedProjectile.GetComponent<RaycastProjectile>();
                raycastProjectile.inGas = true;
                raycastProjectile.gasProps = curGasProps;
            }
            //instantiatedProjectile.GetComponent<Rigidbody>().AddForce(instantiatedProjectile.transform.forward * 4, ForceMode.Impulse);
        }
        GameEvents.current.RecoilKnockback(recoilKnockback, new Vector3());
        var gunSmoke = instantiatedGun.GetComponentInChildren<ParticleSystem>();
        if (gunSmoke != null)
        {
            gunSmoke.Play();
        }
        shooting = true;
    }

    private float TotalXSpread()
    {
        float totalXSpread = 0.0f;
        if (_currentItem.item is IWeapon)
        {
            var weapon = _currentItem.item as IWeapon;
            totalXSpread = weapon.XSpread;
            foreach (var slot in modifierSlots)
            {
                if (slot.item == GameEvents.current.GetEmptyItem())
                {
                    continue;
                }
                var modifier = (Modifier)slot.item;
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
            foreach (var slot in modifierSlots)
            {
                if (slot.item == GameEvents.current.GetEmptyItem())
                {
                    continue;
                }
                var modifier = (Modifier)slot.item;
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
            foreach (var slot in modifierSlots)
            {
                if (slot.item == GameEvents.current.GetEmptyItem())
                {
                    continue;
                }
                var modifier = (Modifier)slot.item;
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
            foreach (var slot in modifierSlots)
            {
                if (slot.item == GameEvents.current.GetEmptyItem())
                {
                    continue;
                }
                var modifier = (Modifier)slot.item;
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
    public Slot RemoveModifierFromWeapon(int modifierIndex, int modifierSlotIndex)
    {
        if (modifierIndex == -1)
        {
            return GameEvents.current.GetEmptySlot();
        }
        Slot slot = new Slot(modifierSlots[modifierIndex]);
        modifierSlots[modifierIndex] = GameEvents.current.GetEmptySlot();
        _currentItem.properties[Constants.P_W_MODIFIERS_LIST] = new List<Slot>(modifierSlots);
        _currentItem.properties[Constants.P_W_MODIFIER_SLOT_INDICES_LIST] = new List<int>(modifierSlotIndices);
        UpdateEquipmentContainer();
        return slot;
        // var inventorySlot = new Slot(slot);
        // GameEvents.current.AddItemToPlayerInventory(inventorySlot);
    }

    public Slot RemoveWeaponFromEquipmentInventory(string id)
    {
        Slot equipment = equipmentManager.Unequip(id);
        UpdateEquipmentContainer();
        return equipment;
    }

    public Slot GetCurrentWeapon(bool test)
    {
        if (equipmentManager.equipmentInventory.items.Count == 0)
        {
            return null;
        }
        return equipmentManager.GetItem(curEquipmentIndex);;
    }

    public void UpdateCurrentWeapon(Slot updatedWeapon)
    {
        equipmentManager.equipmentInventory.items[curEquipmentIndex] = new Slot(updatedWeapon);
        UpdateEquipmentContainer();
    }

    private void PrintOutput(List<List<Output>> outputList)
    {
        var debugString = "";
        for (int i = 0; i < outputList.Count; i++)
        {
            debugString += "Group " + i + ":\n";
            for (int j = 0; j < outputList[i].Count; j++)
            {
                debugString += "   - " + outputList[i][j].slot.item.name + "\n";
                if (outputList[i][j].slot is ITrigger)
                {
                    var postProjectiles = outputList[i][j].postModifiers;
                    for (int k = 0; k < postProjectiles.Count; k++)
                    {
                        debugString += "      * " + postProjectiles[k].slot.item.name + "\n";
                    }
                }
            }
        }
        Debug.Log(debugString);
    }

    public void SaveData(ref GameData data)
    {
        data.curEquipmentIndex = curEquipmentIndex;
    }

    public void LoadData(GameData data)
    {
        curEquipmentIndex = data.curEquipmentIndex;
    }

    public int GetCurEquipmentIndex()
    {
        return curEquipmentIndex;
    }
}
