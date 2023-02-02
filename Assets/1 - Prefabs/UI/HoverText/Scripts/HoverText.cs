using TMPro;
using UnityEngine;

public class HoverText : MonoBehaviour
{
    public TMP_Text hoverText;
    // Start is called before the first frame update
    void Start()
    {
        hoverText = GetComponent<TMP_Text>();    
        GameEvents.current.onUpdateHoverText += UpdateHoverText;
    }

    public void UpdateHoverText(string text)
    {
        hoverText.text = text;
    }
}
