using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Quit Command", menuName = "Utilities/DeveloperConsole/Commands/Quit Command")]
public class QuitCommand : ConsoleCommand
{
    public override bool Process(string[] args)
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        return true;
        #elif UNITY_WEBPLAYER
        Application.OpenURL(webplayerQuitURL);
        return true;
        #else
        Application.Quit();
        return true;
        #endif
    }
}
