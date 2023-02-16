using TMPro;
using UnityEngine;

public class StatsPanel : MonoBehaviour
{
    public TMP_Text agility;
    public TMP_Text strength;

    void Start()
    {
        GameEvents.current.onUpdateStatsPanel += UpdateStatsPanel;
        UpdateStatsPanel(GameEvents.current.GetBuffedStats().agility, GameEvents.current.GetBuffedStats().strength);
    }

    public void UpdateStatsPanel(int agility, int strength)
    {
        this.agility.SetText("Agilty: " + agility.ToString());
        this.strength.SetText("Strength: " + strength.ToString());
    }
}
