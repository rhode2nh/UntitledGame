using UnityEngine;
using TMPro;

public class Spawner : MonoBehaviour, IExecutable
{
    public Transform spawnPos;
    public GameObject objectToSpawn;
    public TextMeshPro itemName;
    public int id;

    // Start is called before the first frame update
    void Start()
    {
        GameEvents.current.onSpawnObject += SpawnObject;
        id = this.GetInstanceID();
        itemName.SetText(objectToSpawn.GetComponent<WorldItem>().item.name);
    }

    public void SpawnObject(int id)
    {
        if (id == this.id)
        {
            Instantiate(objectToSpawn, spawnPos.position, spawnPos.rotation);
        }
    }

    public void Execute()
    {
        SpawnObject(id);
    }
}
