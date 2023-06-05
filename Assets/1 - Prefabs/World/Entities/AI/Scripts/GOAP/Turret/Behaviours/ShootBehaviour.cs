using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootBehaviour : MonoBehaviour
{
    public Transform shootPos;
    public GameObject projectile;
    public float fireRate;
    private float timeSinceLastShot;

    // Start is called before the first frame update
    void Start()
    {
        timeSinceLastShot = Time.time;
    }

    public void Shoot()
    {
        if (Time.time - timeSinceLastShot > fireRate)
        {
            timeSinceLastShot = Time.time;
            var instantiatedProjectile = Instantiate(projectile, shootPos.position, shootPos.rotation);
            instantiatedProjectile.GetComponent<RaycastProjectile>().shouldRedirect = false;
        }
    }
}
