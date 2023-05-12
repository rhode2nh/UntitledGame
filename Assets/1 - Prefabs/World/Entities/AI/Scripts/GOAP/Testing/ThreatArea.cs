using UnityEngine;

public class ThreatArea : MonoBehaviour
{
    public bool isEnemyInThreatArea;
    public ThreatBehaviour threatBehaviour;

    // Start is called before the first frame update
    void Start()
    {
        isEnemyInThreatArea = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == Constants.PLAYER)
        {
            isEnemyInThreatArea = true;
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
            isEnemyInThreatArea = false;
        }
    }
}
