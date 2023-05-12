using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAIShoot : MonoBehaviour
{
    public Transform shootPos;
    public GameObject projectile;

    public void Shoot()
    {
        Instantiate(projectile, shootPos.position, shootPos.rotation);
    }
}
