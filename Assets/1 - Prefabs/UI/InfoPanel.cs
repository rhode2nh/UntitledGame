using TMPro;
using UnityEngine;

public class InfoPanel : MonoBehaviour
{
    public Canvas parentCanvas;
    public TMP_Text text;

    // Start is called before the first frame update
    void Start()
    {
        GameEvents.current.onDeactivateInfoPanel += DeactivateInfoPanel;
        GameEvents.current.onActivateInfoPanel += ActivateInfoPanel;
        GameEvents.current.onSetInfoText += SetInfoText;
        DeactivateInfoPanel();
    }

    void OnEnable()
    {
        CalculatePos();
    }

    // Update is called once per frame
    void Update()
    {
        CalculatePos();
    }

    private void CalculatePos()
    {
        Vector2 movePos;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
                parentCanvas.transform as RectTransform,
                Input.mousePosition, parentCanvas.worldCamera,
                out movePos);
        transform.position = parentCanvas.transform.TransformPoint(new Vector2(movePos.x + 5, movePos.y - 5));
    }

    void DeactivateInfoPanel()
    {
        transform.localScale = new Vector2(0, 0);
    }

    void ActivateInfoPanel()
    {
        transform.localScale = new Vector2(1, 1);
    }

    void SetInfoText(string newText)
    {
        text.SetText(newText);
    }
}
