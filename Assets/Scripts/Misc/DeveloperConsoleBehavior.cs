using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DeveloperConsoleBehavior : MonoBehaviour
{
    [SerializeField] private string prefix = string.Empty;
    [SerializeField] private ConsoleCommand[] commands = new ConsoleCommand[0];

    [Header("UI")]
    [SerializeField] private GameObject uiCanvas = null;
    [SerializeField] private TMP_InputField inputField = null;
    [SerializeField] private TMP_Text historyText = null;

    public MouseLook mouseLook;

    private float pausedTimeScale;

    private static DeveloperConsoleBehavior instance;

    private DeveloperConsole developerConsole;

    private List<string> commandHistory = new List<string>();
    private int historyIndex = 0;

    private DeveloperConsole DeveloperConsole
    {
        get
        {
            if (developerConsole != null) { return developerConsole; }
            return developerConsole = new DeveloperConsole(prefix, commands);
        }
    }

    private void Awake()
    {
        instance = this;
    }

    public void Toggle()
    {
        if (uiCanvas.activeSelf)
        {
            Time.timeScale = pausedTimeScale;
            uiCanvas.SetActive(false);
            mouseLook.enabled = true;
        }
        else
        {
            pausedTimeScale = Time.timeScale;
            Time.timeScale = 0;
            uiCanvas.SetActive(true);
            mouseLook.enabled = false;
            inputField.ActivateInputField();
        }
    }

    public void ProcessCommand(string inputValue)
    {
        if (inputValue.Contains("`"))
        {
            inputField.text = string.Empty;
            return;
        }
        DeveloperConsole.ProcessCommand(inputValue);

        inputField.text = string.Empty;
        historyText.text += inputValue + "\n";
        commandHistory.Add(inputValue);
        historyIndex = commandHistory.Count;
    }

    public void PreviousCommand()
    {
        if (historyIndex <= 0 || commandHistory.Count == 0)
        {
            //historyIndex = 0;
            return;
        }

        inputField.text = commandHistory[--historyIndex];
    }

    public void NextCommand()
    {
        if (historyIndex >= commandHistory.Count || commandHistory.Count == 0)
        {
            inputField.text = string.Empty;
            return;
        }

        inputField.text = commandHistory[historyIndex++];
    }
}
