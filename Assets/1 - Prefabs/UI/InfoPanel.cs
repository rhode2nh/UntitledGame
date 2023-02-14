using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoPanel : MonoBehaviour
{
    public Canvas parentCanvas;
    // Start is called before the first frame update
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

}
