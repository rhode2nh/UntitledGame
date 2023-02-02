using UnityEngine;

public class ProjectileProperties : MonoBehaviour
{
    public Vector3 camDir;
    public bool shouldRedirect;
    public Projectile projectileSO;
    public int numFramesSinceLastCollision = 0;
    public int maxFramesToCollideAgain = 3;
    public bool canHit;

    void Awake()
    {
        shouldRedirect = true;
        canHit = true;
    }

    void FixedUpdate()
    {
        if (numFramesSinceLastCollision < maxFramesToCollideAgain)
        {
            numFramesSinceLastCollision++;
        }
        if (gameObject.GetComponent<Rigidbody>().velocity != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(gameObject.GetComponent<Rigidbody>().velocity);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // TODO: this is a temporary fix. Since the trigger projectile is destroyed in OnCollisionExit,
        // it's rendered a few frames until the next call to FixedUpdate. This hides that artifact.
        if (shouldRedirect)
        {
            RedirectProjectile();
        }

        AlterCollidedObject(collision);
    }

    void AlterCollidedObject(Collision collision)
    {
        var hittable = collision.gameObject.GetComponentInParent<IHittable>();
        if (hittable != null)
        {
            if (numFramesSinceLastCollision >= maxFramesToCollideAgain)
            {
                hittable.ModifyHealth(projectileSO.HitPoints);
            }
        }
        numFramesSinceLastCollision = 0;

    }

    void RedirectProjectile()
    {
        shouldRedirect = false;
        camDir = Camera.main.transform.TransformDirection(Vector3.forward);
        transform.rotation = Quaternion.LookRotation(camDir);
        gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        gameObject.GetComponent<Rigidbody>().AddForce(transform.forward * 4, ForceMode.Impulse);
    }
}
