using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThreatBehaviour : MonoBehaviour
{
    public float threat;
    public float threatRate;
    public float threshold;
    public bool enemyIsInThreatArea;
    public bool isLookingAtEnemy;
    public Vector3 lastEnemyPos;

    private void Awake()
    {
        this.threat = 0.0f;
        enemyIsInThreatArea = false;
        lastEnemyPos = new Vector3();
    }

    private void FixedUpdate()
    {
        if (isLookingAtEnemy)
        {
            threat += Time.fixedDeltaTime * threatRate;
        }
        // else if (threat > 0.0f)
        // {
        //     threat -= Time.fixedDeltaTime * threatRate;
        // }
        // else
        // {
        //     isLookingAtEnemy = false;
        //     threat = 0.0f;
        // }
        else if (!isLookingAtEnemy && threat > 0.0f)
        {
            threat -= Time.fixedDeltaTime * threatRate;
        }
        else
        {
            threat = 0.0f;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == Constants.PLAYER)
        {
            enemyIsInThreatArea = true;
            isLookingAtEnemy = true;
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == Constants.PLAYER)
        {
            threat += Time.fixedDeltaTime * threatRate;
            isLookingAtEnemy = true;
            lastEnemyPos = other.gameObject.transform.position;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == Constants.PLAYER)
        {
            enemyIsInThreatArea = false;
            isLookingAtEnemy = false;
        }
    }
}
