using TMPro;
using UnityEngine;
using System.Collections;

public class WeaponStatsUI : MonoBehaviour
{
    public TMP_Text castDelay;
    public TMP_Text rechargeTime;
    public TMP_Text xSpread;
    public TMP_Text ySpread;
    public RectTransform castDelayBar;
    private bool isCastDelayBarLoading;
    private bool isRechargeBarLoading;
    private bool coroutineStarted;
    private float test;

    // Start is called before the first frame update
    void Start()
    {
        GameEvents.current.onUpdateWeaponStatsGUI += UpdateWeaponStatsGUI;
        GameEvents.current.onIsCastDelayBarLoading += CastDelayBarIsLoading;
        isCastDelayBarLoading = false;
        isRechargeBarLoading = false;
        coroutineStarted = false;
        test = 0.0f;
    }

    void Update() 
    {
        if (isCastDelayBarLoading && !coroutineStarted)
        {
            StartCoroutine(CastDelayBarLoading());
        }
    }

    IEnumerator CastDelayBarLoading()
    {
        Debug.Log("HERE!");
        for (float xScale = 0.0f; xScale <= 1.0f; xScale += 1.0f/test)
        {
            castDelayBar.localScale = new Vector3(xScale, 1.0f, 1.0f);
        }
        coroutineStarted = false;
        isCastDelayBarLoading = false;
        castDelayBar.localScale = new Vector3(0.0f, 1.0f, 1.0f);
        yield return null;
    }

    public void UpdateWeaponStatsGUI(string[] stats)
    {
        castDelay.SetText("Cast Delay: " + stats[0]);
        test = float.Parse(stats[0]);
        rechargeTime.SetText("Recharge Time: " + stats[1]);
        xSpread.SetText("X Spread (Deg): " + stats[2]);
        ySpread.SetText("Y Spread (Deg): " + stats[3]);
    }

    public void CastDelayBarIsLoading()
    {
        isCastDelayBarLoading = true;
    }
}
