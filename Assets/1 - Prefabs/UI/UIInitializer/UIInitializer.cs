using UnityEngine;

public class UIInitializer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
    }
}
