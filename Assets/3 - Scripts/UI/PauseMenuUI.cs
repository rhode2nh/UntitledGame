using UnityEngine;

public class PauseMenuUI : MonoBehaviour
{
    [SerializeField] private GameObject _uiCanvas;

    public void Toggle()
    {
        if (_uiCanvas.activeSelf)
        {
            _uiCanvas.SetActive(false);
        }
        else
        {
            _uiCanvas.SetActive(true);
        }
    }

    public void OnQuitClick()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #elif UNITY_WEBPLAYER
        Application.OpenURL(webplayerQuitURL);
        #else
        Application.Quit();
        #endif
    }
}
