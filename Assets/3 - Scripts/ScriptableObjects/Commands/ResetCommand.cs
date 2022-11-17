using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "New Reset Command", menuName = "Utilities/DeveloperConsole/Commands/Reset Command")]
public class ResetCommand : ConsoleCommand
{
    public override bool Process(string[] args)
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        return true;
    }
}
