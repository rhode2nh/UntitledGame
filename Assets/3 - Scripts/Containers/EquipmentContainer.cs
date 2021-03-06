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
    public Camera mainCamera;
    public LayerMask layerMask;

    public List<Modifier> modifiers;
    public List<Output> lastOutput;
    public bool isAttacking;
    public bool isRecharging;
    public bool coroutineStarted;
    public int curModifierIndex;
    public float gunCastDelay;
    public float gunRechargeTime;
    public float gunXSpread;
    public float gunYSpread;
    public float totalXSpread;
    public float totalYSpread;
    public float totalCastDelay;
    public float totalRechargeTime;

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
        
        modifiers = new List<Modifier>();
        _curOutput = new List<Output>();
        lastOutput = new List<Output>();
        _usedCastXIds = new List<int>();
        _usedModifierIds = new List<int>();
        curEquipmentIndex = 0;
        isAttacking = false;
        isRecharging = false;
        curModifierIndex = 0;
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
            totalCastDelay = TotalCastDelay();
            totalRechargeTime = TotalRechargeTime();
            isRecharging = false;
        }
        else
        {
            projectileSpawner.localPosition = new Vector3(0, 0, 0);
            _currentItem = null;
            equipmentMesh = null;
            equipmentContainer.GetComponent<MeshFilter>().mesh = null;
            modifiers.Clear();
            gunCastDelay = 0.0f;
            gunRechargeTime = 0.0f;
            totalCastDelay = 0.0f;
            totalRechargeTime = 0.0f;
            isRecharging = false;
            curModifierIndex = 0;
        }
    }

    IEnumerator Attack()
    {
        var weapon = _currentItem.item as IWeapon;
        _projectilesToGroup = 1;

        if (modifiers.Count == 0)
        {
            yield return new WaitForSeconds(totalRechargeTime);
            coroutineStarted = false;
            yield break;
        }

        bool hasWrapped = false;
        while (_projectilesToGroup != 0)
        {
            var modifier = modifiers[curModifierIndex];
            if (modifier is IProjectile)
            {
                _curOutput.Add(new Output(modifier));
            }
            
            else if (modifier is ICastX)
            {
                if (_usedCastXIds.Contains(curModifierIndex))
                {
                    _usedCastXIds.Clear();
                    _projectilesToGroup = 1;
                    break;
                }
                var castX = modifier as ICastX;
                _projectilesToGroup += castX.ModifiersPerCast;
                _usedCastXIds.Add(curModifierIndex);
            }

            else
            {
                if (!_usedModifierIds.Contains(curModifierIndex))
                {
                    _usedModifierIds.Add(curModifierIndex);
                }
                curModifierIndex++;
                if (curModifierIndex >= modifiers.Count())
                {
                    curModifierIndex = curModifierIndex % modifiers.Count();
                    hasWrapped = true;
                }
                continue;
            }

            if (!_usedModifierIds.Contains(curModifierIndex))
            {
                _usedModifierIds.Add(curModifierIndex);
            }

            curModifierIndex++;
            _projectilesToGroup--;

            if (curModifierIndex >= modifiers.Count())
            {
                curModifierIndex = curModifierIndex % modifiers.Count();
                hasWrapped = true;
            }
        }

        InstantiateOutput(_curOutput);
        lastOutput = new List<Output>(_curOutput);

        if (_usedModifierIds.Count() >= modifiers.Count())
        {
            hasWrapped = false;
            curModifierIndex = 0;
            _usedModifierIds.Clear();
            _usedCastXIds.Clear();
            _curOutput.Clear();
            isRecharging = true;
            yield return new WaitForSeconds(totalRechargeTime);
            _projectilesToGroup = 1;
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
            _curOutput.Clear();
            _usedCastXIds.Clear();
            yield return new WaitForSeconds(totalCastDelay);
            coroutineStarted = false;
        }
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

            float x = Random.Range(-gunXSpread * 0.5f, gunXSpread * 0.5f);
            float y = Random.Range(-gunYSpread * 0.5f, gunYSpread * 0.5f);
            
            //Vector3 directionWithSpread = directionWithoutSpread + new Vector3(Mathf.Cos(xRad), (float)Mathf.Sin(yRad), 0);
            //Vector3 directionWithSpread = directionWithoutSpread.normalized + transform.TransformVector(new Vector3(x, y, 0));
            Vector3 directionWithSpread = Quaternion.AngleAxis(x, projectileSpawner.up) * Quaternion.AngleAxis(y, projectileSpawner.forward) * directionWithoutSpread;

            var instantiatedProjectile = Instantiate(projectile.ProjectilePrefab, projectileSpawner.position, Quaternion.identity);
            instantiatedProjectile.transform.forward = directionWithSpread.normalized;
            instantiatedProjectile.GetComponent<Rigidbody>().AddForce(instantiatedProjectile.transform.forward * 4, ForceMode.Impulse);
        }
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
}
