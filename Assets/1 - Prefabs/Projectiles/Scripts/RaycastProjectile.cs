using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class RaycastProjectile : MonoBehaviour
{
    private Vector3 startPosition;
    private Vector3 _verticalDirection;
    private Vector3 _forwardDirection, _horizontalDirection;
    private float _forwardProgress, _verticalProgress, _horizontalProgress;
    public float sineFrequency, cosFrequency;
    public float sineAmplitude, cosAmplitude;
    public ITrajectory currentTrajectory;

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
    private float flipHorizontalDir;
    private float horizontalScaleDelta;
    private float verticalScaleDelta;
    private float forwardScaleDelta;
    private float timeElapsed;
    private bool propertiesInitialized;
    private float _verticalStartSpeed;
    private float _horizontalStartSpeed;
    private float _forwardStartSpeed;


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

    void Start() {
        startPosition = transform.position;
        _verticalDirection = Vector3.Cross(-Camera.main.transform.right, transform.forward);
        _horizontalDirection = Vector3.Cross(Camera.main.transform.up, transform.forward);
        _forwardDirection = transform.forward;
        _verticalStartSpeed = 0.0f;
        flipHorizontalDir = 1.0f;
        horizontalScaleDelta = 0.0f;
        verticalScaleDelta = 0.0f;
        forwardScaleDelta = 0.0f;
        timeElapsed = 0f;
        propertiesInitialized = true;
    }

    public void RandomizeProperties() {
        _verticalDirection *= currentTrajectory.InitializeVerticalStartDir();
        _horizontalDirection *= currentTrajectory.InitializeHorizontalStartDir();
        _forwardDirection *= currentTrajectory.InitializeForwardStartDir();
        _verticalStartSpeed = currentTrajectory.InitializeVerticalStartSpeed();
        _horizontalStartSpeed = currentTrajectory.InitializeHorizontalStartSpeed();
        _forwardStartSpeed = currentTrajectory.InitializeForwardStartSpeed();
    }

    void Update()
    {
        if (propertiesInitialized && currentTrajectory != null) {
            RandomizeProperties();
            propertiesInitialized = false;
        }
        _forwardProgress += Time.deltaTime;
        Vector3 position = startPosition;
     
        _verticalProgress += sineFrequency * Time.deltaTime;
        _horizontalProgress += cosFrequency * Time.deltaTime;
        if (currentTrajectory != null) {
            horizontalScaleDelta = currentTrajectory.CalculateHorizontalScaleDelta(timeElapsed);
            verticalScaleDelta = currentTrajectory.CalculateVerticalScaleDelta(timeElapsed);
            forwardScaleDelta = currentTrajectory.CalculateForwardScaleDelta(timeElapsed);
            position += currentTrajectory.CalculateTrajectory(_forwardDirection, _verticalDirection, _horizontalDirection,
                _forwardProgress, horizontalScaleDelta, verticalScaleDelta, forwardScaleDelta,
                _verticalStartSpeed, _horizontalStartSpeed, _forwardStartSpeed);
            timeElapsed += Time.deltaTime;
        } else {
            position += _forwardProgress * _forwardDirection * speed;
        }
     
        nextPos = position;
        transform.LookAt(position);
        // nextPos += gameObject.transform.forward * Time.deltaTime * _curSpeed;
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
                startPosition = transform.position;
                _forwardProgress = 0.0f;
                _verticalProgress = 0.0f;
                _horizontalProgress = 0.0f;
                flipHorizontalDir = -flipHorizontalDir;
                _forwardDirection = transform.forward;
                transform.position += gameObject.transform.forward * Time.deltaTime * _curSpeed;
                nextPos += transform.forward * Time.deltaTime * _curSpeed;
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
