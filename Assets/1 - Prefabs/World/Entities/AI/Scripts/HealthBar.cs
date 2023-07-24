using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private GameObject healthBarCanvas;
    public GameObject healthBar;
    private Image remaining;
    private GameObject instantiatedHealthBar;
    public Vector3 offsetPos;

    // Start is called before the first frame update
    void Start()
    {
        healthBarCanvas = GameObject.FindGameObjectWithTag("HealthBarCanvas");
        instantiatedHealthBar = Instantiate(healthBar);
        remaining = instantiatedHealthBar.transform.GetChild(0).GetComponent<Image>();
        instantiatedHealthBar.transform.SetParent(healthBarCanvas.transform);
        instantiatedHealthBar.transform.position = transform.parent.position + offsetPos;
    }

    // Update is called once per frame
    void Update()
    {
        instantiatedHealthBar.transform.position = transform.parent.position + offsetPos;
        instantiatedHealthBar.transform.LookAt(Camera.main.gameObject.transform);
    }

    public void ModifyHealthBar(float curHealth, float maxHealth) {
        remaining.transform.localScale = new Vector3(curHealth / maxHealth, 1f, 1f);
    }

    void OnDestroy() {
        Destroy(instantiatedHealthBar);
    }
}
