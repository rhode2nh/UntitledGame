using UnityEngine;

public class ThreatArea : MonoBehaviour
{
    public ThreatBehaviour threatBehaviour;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == Constants.PLAYER)
        {
            threatBehaviour.isEnemyInThreatArea = true;
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
            threatBehaviour.isEnemyInThreatArea = false;
        }
    }
}
