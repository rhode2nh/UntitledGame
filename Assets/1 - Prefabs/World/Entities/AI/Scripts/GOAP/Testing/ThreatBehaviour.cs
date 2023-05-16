using UnityEngine;

public class ThreatBehaviour : MonoBehaviour
{
    public float threat;
    public float threatRate;
    public float threshold;
    public Vector3 lastEnemyPos;
    public bool isEnemyInThreatArea;
    public bool isLookingAtEnemy;

    private ThreatArea threatArea;
    private LOSArea losArea;

    private void Awake()
    {
        this.threat = 0.0f;
        lastEnemyPos = new Vector3();
        isEnemyInThreatArea = false;
        isLookingAtEnemy = false;
    }

    private void FixedUpdate()
    {
        if (isLookingAtEnemy)
        {
            threat = threshold + 1;
        }
        else if (isEnemyInThreatArea)
        {
            threat += Time.fixedDeltaTime * threatRate;
        }
        else if (!isLookingAtEnemy && threat > 0.0f)
        {
            threat -= Time.fixedDeltaTime * threatRate;
        }
        else
        {
            threat = 0.0f;
        }
    }

    public Vector3 GetLastEnemyPos()
    {
        return this.lastEnemyPos;
    }
}
