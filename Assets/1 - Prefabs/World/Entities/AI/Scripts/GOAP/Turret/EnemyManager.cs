using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : LifeEntity, IHittable, IDropLoot
{
    private Collider capsuleCollider;
    private float topHitPointPos;
    public float hitPointOffset;
    private List<GameObject> lootTable;
    public GameObject lootToDrop;
    public HealthBar healthBar;
    private bool hasDropped = false;
    public float chanceToDropLoot;

    void Start()
    {
        maxHealth = 100.0f;
        health = 100.0f;
        capsuleCollider = GetComponent<Collider>();
        topHitPointPos = capsuleCollider.bounds.max.y;
        lootTable = DatabaseManager.instance.GetGameObjects();
    }

    void Update()
    {
        topHitPointPos = capsuleCollider.bounds.max.y;
    }

    public void ModifyHealth(float hitPoints)
    {
        if (!isInvincible)
        {
            health -= hitPoints;
            healthBar.ModifyHealthBar(health, maxHealth);
        }
        hitPointText.text = hitPoints.ToString();
        Instantiate(hitPointText, new Vector3(Random.Range(transform.position.x - 0.3f, transform.position.x + 0.3f), Random.Range(topHitPointPos - 0.3f + hitPointOffset, topHitPointPos + 0.3f + hitPointOffset), transform.position.z), new Quaternion());
        if (health <= 0)
        {
            if (!hasDropped) {
                if (lootToDrop != null) {
                    DropLoot(lootToDrop);
                } else {
                    DropLoot(lootTable[Random.Range(0, lootTable.Count)]);
                }
                hasDropped = true;
            }
            Destroy(gameObject);
        }
    }

    public void DropLoot(GameObject loot) {
        if (chanceToDropLoot >= Random.Range(0.0f, 100.0f)) {
            GameObject instance = Instantiate(loot, transform.position, new Quaternion());
            instance.GetComponent<Rigidbody>().AddForce(transform.up * 5, ForceMode.Impulse);
        }
    }
}