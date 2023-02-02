using System.Collections;
using TMPro;
using UnityEngine;

public class HitPointText : MonoBehaviour
{
    public TMP_Text hitPoints;
    private bool coroutineStarted = false;

    void Update()
    {
        transform.LookAt(Camera.main.gameObject.transform);
        if (!coroutineStarted)
        {
            StartCoroutine(FadeTextToFullAlpha(0.5f, hitPoints));
            StartCoroutine(LerpPosition(1.0f));
            coroutineStarted = true;
        }
    }

    public IEnumerator FadeTextToFullAlpha(float t, TMP_Text i)
    {
        i.color = new Color(i.color.r, i.color.g, i.color.b, 1.0f);
        while (i.color.a >= 0.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a - (Time.deltaTime / t));
            yield return null;
        }
        if (i.color.a <= 0.0f)
        {
            Destroy(gameObject);
        }
    }

    public IEnumerator LerpPosition(float duration)
    {
        float time = 0;
        Vector3 startPosition = transform.position;

        while (time < duration)
        {
            transform.position = Vector3.Lerp(startPosition, new Vector3(startPosition.x, startPosition.y + 2, startPosition.z), time / duration);
            time += Time.deltaTime;
            yield return null;
        }
    }
}
