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
            float velocity = other.gameObject.GetComponentInParent<CharacterController>().velocity.magnitude;
            if (velocity > threatBehaviour.velocityThreshold) {
                threatBehaviour.hasEnemyMoved = true;
            } else {
                threatBehaviour.hasEnemyMoved = false;
            }
            threatBehaviour.lastEnemyPos = other.gameObject.transform.position;
            // threatBehaviour.lastEnemyPos = other.gameObject.transform.position;
            // if (!(Mathf.Abs(threatBehaviour.lastLastEnemyPos.x - threatBehaviour.lastEnemyPos.x) > 0.1f)) {
            //     threatBehaviour.hasEnemyMoved = true;
            // } else {
            //     threatBehaviour.hasEnemyMoved = false;
            // }
            // threatBehaviour.lastLastEnemyPos = threatBehaviour.lastEnemyPos;
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
