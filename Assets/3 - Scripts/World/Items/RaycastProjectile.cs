using System;
using UnityEngine;

public class RaycastProjectile : MonoBehaviour
{
    public float speed;
    public Vector3 curPos;
    public Vector3 nextPos;
    public int maxNumBouces;
    public LayerMask layerMask;
    public Projectile projectileSO;
    public bool shouldRedirect;

    private int _numBounces;
    private TriggerList _triggerList;

    // Start is called before the first frame update
    void Start()
    {
        curPos = transform.position;
        nextPos = curPos;
        shouldRedirect = true;
        _triggerList = GetComponent<TriggerList>();
        Destroy(gameObject, projectileSO.timeAlive);
    }

    void Update()
    {
        nextPos += gameObject.transform.forward * Time.deltaTime * speed;

        RaycastHit hitInfo;
        if (Physics.Linecast(curPos, nextPos, out hitInfo, layerMask))
        {
            //TODO: Figure out what to do when a collision occurs
            _numBounces++;

            if (_triggerList != null)
            {
               _triggerList.CalculateTriggerChildren(); 
            }

            var hittable = hitInfo.collider.gameObject.GetComponentInParent<IHittable>();
            if (hittable != null)
            {
                hittable.ModifyHealth(projectileSO.HitPoints);
            }

            if (_numBounces == maxNumBouces)
            {
                Destroy(gameObject);
            }

            if (shouldRedirect)
            {
                RedirectProjectile();
            }
            transform.forward = Vector3.Reflect(transform.forward, hitInfo.normal);
        }
        else
        {
            transform.position = nextPos;   
            curPos = transform.position;
        }
    }

    void RedirectProjectile()
    {
        shouldRedirect = false;
        var camDir = Camera.main.transform.TransformDirection(Vector3.forward);
        transform.rotation = Quaternion.LookRotation(camDir);
    }
}
