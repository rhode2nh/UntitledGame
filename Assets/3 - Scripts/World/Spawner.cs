using UnityEngine;

public class Spawner : MonoBehaviour, IExecutable
{
    public Transform spawnPos;
    public GameObject objectToSpawn;
    public int id;

    // Start is called before the first frame update
    void Start()
    {
        GameEvents.current.onSpawnObject += SpawnObject;
        id = this.GetInstanceID();
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
