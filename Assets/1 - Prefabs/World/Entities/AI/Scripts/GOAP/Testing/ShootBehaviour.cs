using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootBehaviour : MonoBehaviour
{
    private TestAIShoot testAIShoot;
    public float fireRate;
    private float timeSinceLastShot;

    // Start is called before the first frame update
    void Start()
    {
        testAIShoot = GetComponent<TestAIShoot>();
        timeSinceLastShot = Time.time;
    }

    public void Shoot()
    {
        if (Time.time - timeSinceLastShot > fireRate)
        {
            timeSinceLastShot = Time.time;
            testAIShoot.Shoot();
        }
    }
}
