using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeveloperConsoleBehavior : MonoBehaviour
{
    [SerializeField] private string prefix = string.Empty;
    [SerializeField] private ConsoleCommand[] commands = new ConsoleCommand[0];

    [Header("UI")]
    [SerializeField] private GameObject uiCanvas = null;
    [SerializeField] private TMP_InputField inputField = null;
    [SerializeField] private TMP_Text historyText = null;
    [SerializeField] private TMP_Text suggestionsText = null;
    [SerializeField] private GameObject suggestionsBox = null;

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

    void Start()
    {
        inputField.onValueChanged.AddListener(delegate { GiveSuggestions(); });
        suggestionsBox.SetActive(false);
    }

    public void Toggle()
    {
        if (uiCanvas.activeSelf)
        {
            uiCanvas.SetActive(false);
        }
        else
        {
            uiCanvas.SetActive(true);
            inputField.ActivateInputField();
        }
    }

    public void ProcessCommand(string inputValue)
    {
        if (inputValue.Contains("`") || inputValue.Equals(string.Empty))
        {
            inputField.text = string.Empty;
            return;
        }
        DeveloperConsole.ProcessCommand(inputValue);

        inputField.text = string.Empty;
        historyText.text += inputValue + "\n";
        commandHistory.Add(inputValue);
        historyIndex = commandHistory.Count;
        inputField.ActivateInputField();
    }

    public void PreviousCommand()
    {
        if (historyIndex <= 0 || commandHistory.Count == 0)
        {
            //historyIndex = 0;
            return;
        }

        inputField.text = commandHistory[--historyIndex];
        inputField.ActivateInputField();
        inputField.caretPosition = inputField.text.Length;
    }

    public void NextCommand()
    {
        if (historyIndex >= commandHistory.Count || commandHistory.Count == 0)
        {
            inputField.text = string.Empty;
            return;
        }

        inputField.text = commandHistory[historyIndex++];
        inputField.ActivateInputField();
        inputField.caretPosition = inputField.text.Length;
    }

    public void GiveSuggestions()
    {
        if (inputField.text.Length == 0 || !inputField.text.StartsWith('/'))
        {
            suggestionsText.text = "";
            suggestionsBox.SetActive(false);
            return;
        }

        List<string> commandNames = new List<string>();

        for (int i = 0; i < commands.Length; i++)
        {
            commandNames.Add(commands[i].CommandWord[0]);
        }

        string suggestions = "";

        int matches = 0;

        foreach (string command in commandNames)
        {
            if (command.ToLower().StartsWith(inputField.text.Substring(1).ToLower()))
            {
                suggestions += command + "\n";
                matches++;
            }
        }
        suggestionsText.text = suggestions;
        
        if (matches == 0)
            suggestionsBox.SetActive(false);
        else
            suggestionsBox.SetActive(true);
    }
}
