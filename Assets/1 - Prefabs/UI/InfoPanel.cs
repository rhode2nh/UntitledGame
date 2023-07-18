using System.Collections;
using TMPro;
using UnityEngine;

public class InfoPanel : MonoBehaviour
{
    public Canvas parentCanvas;
    public TMP_Text text;
    public RectTransform rectTransform;
    Vector3[] v = new Vector3[4];
    Vector3[] r = new Vector3[4];

    // Start is called before the first frame update
    void Awake()
    {
        GameEvents.current.onDeactivateInfoPanel += DeactivateInfoPanel;
        GameEvents.current.onActivateInfoPanel += ActivateInfoPanel;
        GameEvents.current.onSetInfoText += SetInfoText;
        rectTransform = GetComponent<RectTransform>();
        DeactivateInfoPanel();
        parentCanvas.GetComponent<RectTransform>().GetWorldCorners(v);

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
        transform.position = parentCanvas.transform.TransformPoint(new Vector2(movePos.x, movePos.y));
    }

    void DeactivateInfoPanel()
    {
        GetComponent<RectTransform>().pivot = Vector2.up;
        transform.localScale = Vector2.zero;
    }

    void ActivateInfoPanel()
    {
        transform.localScale = Vector2.one;
    }

    void SetInfoText(string newText)
    {
        text.SetText(newText);
        StartCoroutine(CalculatePivot());
    }

    IEnumerator CalculatePivot() {
        // Since rect size calculation is done at render time, we need to wait for rendering to get the updated width/height
        DeactivateInfoPanel();
        yield return new WaitForEndOfFrame();
        ActivateInfoPanel();
        rectTransform.GetWorldCorners(r);
        if (r[3].y < 0.0f && r[3].x > v[3].x) {
            rectTransform.pivot = Vector2.right;
        } else if (r[3].y < 0.0f) {
            rectTransform.pivot = Vector2.zero;
        } else if (r[3].x > v[3].x) {
            rectTransform.pivot = Vector2.right;
        } else {
            rectTransform.pivot = Vector2.up;
        }
    }
}
