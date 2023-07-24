using TMPro;
using UnityEngine;

public class StatsPanel : MonoBehaviour
{
    public TMP_Text agility;
    public TMP_Text strength;
    public TMP_Text jumpHeight;

    void Start()
    {
        GameEvents.current.onUpdateStatsPanel += UpdateStatsPanel;
        UpdateStatsPanel(GameEvents.current.GetBuffedStats());
    }

    public void UpdateStatsPanel(TestStats stats)
    {
        this.agility.SetText("Agilty: " + stats.agility.ToString());
        this.strength.SetText("Strength: " + stats.strength.ToString());
        this.jumpHeight.SetText("Jump Height: " + stats.jumpHeight.ToString());
    }
}
