using UnityEngine;

public class WeaponSpawner : MonoBehaviour
{
    public Transform spawnPos;
    public GameObject weapon;
    private GameObject weaponPrefab;
    // Start is called before the first frame update
    void Start()
    {
        weaponPrefab = Instantiate(weapon, spawnPos.position, spawnPos.rotation);
        weaponPrefab.transform.parent = null;
        weaponPrefab.GetComponent<Rigidbody>().useGravity = false;
        Physics.IgnoreLayerCollision(7, 8);
    }

    // Update is called once per frame
    void Update()
    {
        if (weaponPrefab == null)
        {
            Destroy(this);
        }
        else
        {
            weaponPrefab.transform.Rotate(transform.up, 20 * Time.deltaTime);
        }
    }
}
