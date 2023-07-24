using TMPro;
using UnityEngine;
using System.Collections;

public class WeaponStatsUI : MonoBehaviour
{
    public TMP_Text castDelay;
    public TMP_Text rechargeTime;
    public TMP_Text xSpread;
    public TMP_Text ySpread;
    public TMP_Text health;
    public RectTransform castDelayBar;
    public RectTransform rechargeDelayBar;
    private float castDelayTime;
    private float rechargeDelayTime;

    // Start is called before the first frame update
    void Awake()
    {
        GameEvents.current.onUpdateWeaponStatsGUI += UpdateWeaponStatsGUI;
        GameEvents.current.onIsCastDelayBarLoading += CastDelayBarIsLoading;
        GameEvents.current.onIsRechargeDelayBarLoading += RechargeDelayBarIsLoading;
        GameEvents.current.onStopLoadingBars += StopLoadingBars;
        GameEvents.current.onUpdateHealth += UpdateHealth;
        castDelayTime = 0.0f;
        rechargeDelayTime = 0.0f;
    }

    // TODO: Make a loading bar component
    IEnumerator CastDelayBarLoading()
    {
        float elapsedTime = 0.0f;
        while (elapsedTime < castDelayTime)
        {
            elapsedTime += Time.deltaTime;
            castDelayBar.localScale = new Vector3(elapsedTime / castDelayTime, 1.0f, 1.0f);
            yield return null;
        }
        castDelayBar.localScale = new Vector3(0.0f, 1.0f, 1.0f);
    }

    IEnumerator RechargeDelayBarLoading()
    {
        float elapsedTime = 0.0f;
        while (elapsedTime < rechargeDelayTime)
        {
            elapsedTime += Time.deltaTime;
            rechargeDelayBar.localScale = new Vector3(elapsedTime / rechargeDelayTime, 1.0f, 1.0f);
            yield return null;
        }
        rechargeDelayBar.localScale = new Vector3(0.0f, 1.0f, 1.0f);
    }

    public void UpdateHealth(float health) {
        this.health.SetText("Health: " + health.ToString());
    }

    public void UpdateWeaponStatsGUI(string[] stats)
    {
        castDelay.SetText("gerrymandering delay: " + stats[0]);
        castDelayTime = float.Parse(stats[0]);
        rechargeDelayTime = float.Parse(stats[1]);
        rechargeTime.SetText("reptillian time: " + stats[1]);
        xSpread.SetText("X Spread (Deg): " + stats[2]);
        ySpread.SetText("Y Spread (Deg): " + stats[3]);
    }

    public void CastDelayBarIsLoading()
    {
        StartCoroutine(CastDelayBarLoading());
    }

    public void RechargeDelayBarIsLoading()
    {
        StartCoroutine(RechargeDelayBarLoading());
    }

    public void StopLoadingBars()
    {
        StopAllCoroutines();
        castDelayBar.localScale = new Vector3(0.0f, 1.0f, 1.0f);
        rechargeDelayBar.localScale = new Vector3(0.0f, 1.0f, 1.0f);
    }
}
