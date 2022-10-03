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
    public RectTransform rechargeDelayBar;
    private bool isCastDelayBarLoading;
    private bool isRechargeDelayBarLoading;
    private bool coroutineStarted;
    private float castDelayTime;
    private float rechargeDelayTime;

    // Start is called before the first frame update
    void Start()
    {
        GameEvents.current.onUpdateWeaponStatsGUI += UpdateWeaponStatsGUI;
        GameEvents.current.onIsCastDelayBarLoading += CastDelayBarIsLoading;
        GameEvents.current.onIsRechargeDelayBarLoading += RechargeDelayBarIsLoading;
        isCastDelayBarLoading = false;
        isRechargeDelayBarLoading = false;
        coroutineStarted = false;
        castDelayTime = 0.0f;
        rechargeDelayTime = 0.0f;
    }

    void Update() 
    {
        if (isCastDelayBarLoading && !coroutineStarted)
        {
            StartCoroutine(CastDelayBarLoading());
        }
        if (isRechargeDelayBarLoading && !coroutineStarted)
        {
            StartCoroutine(RechargeDelayBarLoading());
        }
    }

    IEnumerator CastDelayBarLoading()
    {
        coroutineStarted = true;
        float elapsedTime = 0.0f;
        while (elapsedTime < castDelayTime)
        {
            elapsedTime += Time.deltaTime;
            castDelayBar.localScale = new Vector3(elapsedTime / castDelayTime, 1.0f, 1.0f);
            yield return null;
        }
        coroutineStarted = false;
        isCastDelayBarLoading = false;
        castDelayBar.localScale = new Vector3(0.0f, 1.0f, 1.0f);
    }

    IEnumerator RechargeDelayBarLoading()
    {
        coroutineStarted = true;
        float elapsedTime = 0.0f;
        while (elapsedTime < rechargeDelayTime)
        {
            elapsedTime += Time.deltaTime;
            rechargeDelayBar.localScale = new Vector3(elapsedTime / rechargeDelayTime, 1.0f, 1.0f);
            yield return null;
        }
        coroutineStarted = false;
        isRechargeDelayBarLoading = false;
        rechargeDelayBar.localScale = new Vector3(0.0f, 1.0f, 1.0f);
    }

    public void UpdateWeaponStatsGUI(string[] stats)
    {
        castDelay.SetText("Cast Delay: " + stats[0]);
        castDelayTime = float.Parse(stats[0]);
        rechargeDelayTime = float.Parse(stats[1]);
        rechargeTime.SetText("Recharge Time: " + stats[1]);
        xSpread.SetText("X Spread (Deg): " + stats[2]);
        ySpread.SetText("Y Spread (Deg): " + stats[3]);
    }

    public void CastDelayBarIsLoading()
    {
        isCastDelayBarLoading = true;
    }

    public void RechargeDelayBarIsLoading()
    {
        isRechargeDelayBarLoading = true;
    }
}
