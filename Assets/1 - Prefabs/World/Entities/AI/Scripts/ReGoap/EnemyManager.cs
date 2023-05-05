using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : LifeEntity, IHittable
{
    private CapsuleCollider capsuleCollider;
    private float topHitPointPos;
    public float hitPointOffset;
    void Start()
    {
        health = 100.0f;
        capsuleCollider = GetComponent<CapsuleCollider>();
        topHitPointPos = capsuleCollider.bounds.max.y;
    }

    public void ModifyHealth(float hitPoints)
    {
        if (!isInvincible)
        {
            health -= hitPoints;
        }
        hitPointText.text = hitPoints.ToString();
        Instantiate(hitPointText, new Vector3(Random.Range(transform.position.x - 0.3f, transform.position.x + 0.3f), Random.Range(topHitPointPos - 0.3f + hitPointOffset, topHitPointPos + 0.3f + hitPointOffset), transform.position.z), new Quaternion());
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
