using System.Collections.Generic;
using UnityEngine;

public class TriggerList : MonoBehaviour
{
    public List<Output> triggerList;
    public bool spawnedFromTrigger = false;
    private Vector3 camDir;
    public float xSpread;
    public float ySpread;

    // Start is called before the first frame update
    void Awake()
    {
        triggerList = new List<Output>();
    }

    void OnCollisionEnter(Collision collision)
    {
        camDir = Camera.main.transform.TransformDirection(Vector3.forward);
        if (spawnedFromTrigger)
        {
            spawnedFromTrigger = false;
        }
        else
        {
            for (int i = 0; i < triggerList.Count; i++)
            {
                float x = Random.Range(-xSpread * 0.5f, xSpread * 0.5f);
                float y = Random.Range(-ySpread * 0.5f, ySpread * 0.5f);
                var projectile = triggerList[i].projectile as IProjectile;
                var instantiatedProjectile = Instantiate(projectile.ProjectilePrefab);
                var redirect = instantiatedProjectile.GetComponent<RedirectProjectile>();
                if (redirect != null)
                {
                    redirect.shouldRedirect = false;
                }
                instantiatedProjectile.transform.position = transform.position;
                var triggers = instantiatedProjectile.GetComponent<TriggerList>();
                if (triggers != null)
                {
                    triggers.spawnedFromTrigger = true;
                    triggers.triggerList = new List<Output>(triggerList);
                    triggers.triggerList.RemoveAt(0);
                    instantiatedProjectile.GetComponent<Rigidbody>().AddForce(instantiatedProjectile.transform.forward * 4, ForceMode.Impulse);
                    Debug.Log("here");
                    break;
                }
                instantiatedProjectile.transform.rotation = Quaternion.LookRotation(camDir);
                instantiatedProjectile.transform.rotation = Quaternion.AngleAxis(x, Vector3.up) * instantiatedProjectile.transform.rotation;
                instantiatedProjectile.GetComponent<Rigidbody>().AddForce(instantiatedProjectile.transform.forward * 4, ForceMode.Impulse);
            }
            Destroy(gameObject);
        }
    }
}
