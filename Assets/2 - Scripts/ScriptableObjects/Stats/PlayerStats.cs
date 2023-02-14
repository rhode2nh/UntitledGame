using UnityEngine;

[CreateAssetMenu(fileName = "New Player Stats", menuName = "Stats/Player Stats")]
public class PlayerStats : Stats
{
    [Header("Player Specific Stats")]
    [SerializeField] public float health;
    [SerializeField] private float distanceTraveled = 0.0f;
    [SerializeField] public TestStats buffedStats;

    public float DistanceTraveled
    {
        get { return distanceTraveled; }
        set { distanceTraveled = value; }
    }

    public void InitializeStats()
    {
        testStats = new TestStats(1, 1);
        buffedStats = new TestStats(testStats);
    }
}
