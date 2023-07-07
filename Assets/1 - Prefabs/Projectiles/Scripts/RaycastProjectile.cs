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
    public bool inGas;
    public GasProps gasProps;

    private int _numBounces;
    private float _curSpeed;
    private TriggerList _triggerList;
    private Vector3 _camDir;

    // Start is called before the first frame update
    void Awake()
    {
        curPos = transform.position;
        nextPos = curPos;
        shouldRedirect = true;
        inGas = false;
        _triggerList = GetComponent<TriggerList>();
        Destroy(gameObject, projectileSO.timeAlive);
        _curSpeed = speed;
        _camDir = Camera.main.transform.TransformDirection(Vector3.forward);
    }

    void Update()
    {
        nextPos += gameObject.transform.forward * Time.deltaTime * _curSpeed;
        RaycastHit hitInfo;
        if (Physics.Linecast(curPos, nextPos, out hitInfo, layerMask))
        {
            if (!hitInfo.collider.isTrigger)
            {
                //TODO: Figure out what to do when a collision occurs
                _numBounces++;
                if (shouldRedirect)
                {
                    RedirectProjectile();
                }

                if (_triggerList != null)
                {
                   _triggerList.CalculateTriggerChildren(); 
                }

                var hittable = hitInfo.collider.gameObject.GetComponentInParent<IHittable>();
                if (hittable != null)
                {
                    hittable.ModifyHealth(projectileSO.HitPoints);
                    var flyingEnemy = hitInfo.collider.gameObject.GetComponentInParent<PIDController>();
                    if (flyingEnemy != null)
                    {
                        flyingEnemy.ApplyForces(transform.right, transform.forward);
                        flyingEnemy.hit = true;
                    }
                    Destroy(gameObject);
                }

                if (_numBounces == maxNumBouces)
                {
                    Destroy(gameObject);
                }

                transform.forward = Vector3.Reflect(transform.forward, hitInfo.normal);
                transform.position += gameObject.transform.forward * Time.deltaTime * _curSpeed;
                nextPos += gameObject.transform.forward * Time.deltaTime * _curSpeed;
                curPos = transform.position;
            }
            else
            {
                var gasProperties = hitInfo.collider.gameObject.GetComponent<GasProperties>();
                if (gasProperties != null)
                {
                    inGas = true;
                    gasProps = gasProperties.gasProps;
                }
                transform.position = nextPos;   
                curPos = transform.position;
            }
        }
        else
        {
            transform.position = nextPos;   
            curPos = transform.position;
        }
        if (inGas)
        {
            ApplyGasProperties();
        }
    }

    void RedirectProjectile()
    {
        shouldRedirect = false;
        transform.rotation = Quaternion.LookRotation(_camDir);
    }

    void ApplyGasProperties()
    {
        _curSpeed += gasProps.accelerationFactor * Time.deltaTime;
        if (_curSpeed <= gasProps.minSpeed)
        {
            _curSpeed = gasProps.minSpeed;
            inGas = false;
        }
        else if (_curSpeed > gasProps.maxSpeed + speed)
        {
            _curSpeed = gasProps.maxSpeed + speed;
            inGas = false;
        }
    }
}
