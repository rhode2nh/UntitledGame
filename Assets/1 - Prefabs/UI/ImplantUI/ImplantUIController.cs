using UnityEngine;

public class ImplantUIController : MonoBehaviour
{
    //private MouseLook mouseLook;
    public GameObject implantUI;
    
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
