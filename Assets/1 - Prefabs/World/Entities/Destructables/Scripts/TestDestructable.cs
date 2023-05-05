using UnityEngine;

public class TestDestructable : LifeEntity, IDestructable, IHittable
{
    // Start is called before the first frame update
    void Start()
    {
        health = 100.0f;
    }

    public void ModifyHealth(float hitPoints)
    {
        if (!isInvincible)
        {
            health -= hitPoints;
        }
        hitPointText.text = hitPoints.ToString();
        Instantiate(hitPointText, new Vector3(Random.Range(transform.position.x - 0.3f, transform.position.x + 0.3f), Random.Range(transform.position.y - 0.3f, transform.position.y + 0.3f), transform.position.z), new Quaternion());
        if (health <= 0)
        {
            Destruct();
        }
    }

    public void Destruct()
    {
        Destroy(gameObject);
    }
}
