using UnityEngine;

public class WButton : MonoBehaviour
{
    private IExecutable _executable;
    // Start is called before the first frame update
    void Start()
    {
        _executable = this.transform.parent.GetComponentInChildren<IExecutable>();
    }

    public void Execute()
    {
        if (_executable != null)
        {
            _executable.Execute();
        }
        else
        {
            Debug.Log("The button is not connected to anything!");
        }
    }
}
