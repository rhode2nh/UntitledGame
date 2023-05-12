using UnityEngine;

public class ThreatBehaviour : MonoBehaviour
{
    public float threat;
    public float threatRate;
    public float threshold;
    public Vector3 lastEnemyPos;

    private ThreatArea threatArea;
    private LOSArea losArea;

    private void Awake()
    {
        this.threat = 0.0f;
        threatArea = GetComponentInChildren<ThreatArea>();
        losArea = GetComponentInChildren<LOSArea>();
        lastEnemyPos = new Vector3();
    }

    private void FixedUpdate()
    {
        if (this.IsLookingAtEnemy())
        {
            threat = threshold + 1;
        }
        else if (this.IsEnemyInThreatArea())
        {
            threat += Time.fixedDeltaTime * threatRate;
        }
        else if (!this.IsLookingAtEnemy() && threat > 0.0f)
        {
            threat -= Time.fixedDeltaTime * threatRate;
        }
        else
        {
            threat = 0.0f;
        }
    }

    public bool IsEnemyInThreatArea()
    {
        return threatArea.isEnemyInThreatArea;
    }

    public bool IsLookingAtEnemy()
    {
        return losArea.isLookingAtEnemy;
    }

    public Vector3 GetLastEnemyPos()
    {
        return this.lastEnemyPos;
    }
}
