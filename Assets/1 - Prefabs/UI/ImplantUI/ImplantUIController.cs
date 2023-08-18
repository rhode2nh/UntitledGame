using UnityEngine;

public class ImplantUIController : MonoBehaviour
{
    //private MouseLook mouseLook;
    public static ImplantUIController instance;
    public GameObject implantUI;
    
    void Awake() {
        if (instance != null)
        {
            Debug.LogError("Found more than one Implant UI Controller in the scene.");
        }
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        //mouseLook = GetComponent<MouseLook>();
    }

    // Update is called once per frame
    void Update()
    {
    }
    
    public void OpenImplantUI()
    {
        implantUI.SetActive(true);
        //Time.timeScale = 0;
        //mouseLook.enabled = false;
        Cursor.lockState = CursorLockMode.None;
    }

    public void CloseImplantUI()
    {
        implantUI.SetActive(false);
        //Time.timeScale = 1;
        //mouseLook.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
