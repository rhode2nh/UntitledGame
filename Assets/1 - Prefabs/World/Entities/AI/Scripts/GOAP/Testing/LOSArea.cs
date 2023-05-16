using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LOSArea : MonoBehaviour
{
    public ThreatBehaviour threatBehaviour;

    // Start is called before the first frame update
    void Start()
    {
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == Constants.PLAYER)
        {
            threatBehaviour.isLookingAtEnemy = true;
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == Constants.PLAYER)
        {
            threatBehaviour.lastEnemyPos = other.gameObject.transform.GetChild(0).transform.position;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == Constants.PLAYER)
        {
            threatBehaviour.isLookingAtEnemy = false;
        }
    }
}
