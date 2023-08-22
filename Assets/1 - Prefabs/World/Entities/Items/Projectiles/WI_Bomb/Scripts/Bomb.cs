using System.Collections;
using System.Collections.Generic;
using CrashKonijn.Goap.Classes.References;
using UnityEditor.Callbacks;
using UnityEditor.Search;
using UnityEngine;

public class Bomb : MonoBehaviour, IShootable
{
    private Rigidbody rb;

    public Projectile projectileSO;
    public GameObject explosion;
    public LayerMask layerMask;
    public float damage;
    public float speed;
    
    public void Awake() {
        rb = GetComponent<Rigidbody>();
        Destroy(gameObject, projectileSO.timeAlive);
    }
    public void Shoot() {
        rb.AddForce(transform.forward * speed, ForceMode.Impulse);
        rb.AddTorque(transform.right * speed);
    }

    void OnDestroy() {
        GameObject explosionPS = Instantiate(explosion, transform.position, transform.rotation);
        explosionPS.GetComponent<ParticleSystem>().Play();
        Collider[] colliders = Physics.OverlapSphere(transform.position, 3.0f, layerMask, QueryTriggerInteraction.Ignore);
        if (colliders.Length > 0) {
            foreach (var collider in colliders) {
                if (collider.gameObject.GetComponent<IHittable>() != null) {
                    collider.gameObject.GetComponent<IHittable>().ModifyHealth(damage);
                }
            }
        }
    }
}
