using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;

public class MouseSens : MonoBehaviour
{
    public TMP_InputField text;

    void Awake() {
        text.text = GameEvents.current.GetMouseSense().ToString();
    }

    public void SetMouseSens() {
        float mouseSense;
        if (float.TryParse(text.text, out mouseSense)) {
            GameEvents.current.SetMouseSense(mouseSense);
        };
    }
}
