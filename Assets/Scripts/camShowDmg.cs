using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

public class camShowDmg : MonoBehaviour
{
    private VignetteAndChromaticAberration vig;

    private Coroutine timer;
    private Coroutine timerStart;
    private Coroutine timerEnd;

    public float fadingOutTime = 2f;
    public float startValueVig = 0.4f;
    private float vigAtFunctionStart;
    private float blurAtFunctionStart;

    private bool deathLock = false;

    public void Awake()
    {
        vig = gameObject.GetComponent<VignetteAndChromaticAberration>();
    }

    public void showDmg()
    {
        if (!deathLock)
        {
            StopAllCoroutines();
            timer = StartCoroutine(fadingOut());
        }
    }

    public void showStart()
    {
        if (!deathLock)
        {
            StopAllCoroutines();
            timerStart = StartCoroutine(fadingStart());
        }
    }

    public void showDead()
    {
        if (!deathLock)
        {
            StopAllCoroutines();
            deathLock = true;
            vigAtFunctionStart = vig.intensity;
            timerEnd = StartCoroutine(fadingDeath());
        }
    }

    private IEnumerator fadingOut()
    {
        for (int i = 100; i >= 0; i--)
        {
            vig.intensity = startValueVig * (((float)i) / 100);
            yield return new WaitForSeconds(fadingOutTime / 100);
        }
        StopCoroutine(timer);
    }

    private IEnumerator fadingStart()
    {
        for (int i = 25; i >= 0; i--)
        {
            vig.intensity = (((float)i * 4) / 100);
            yield return new WaitForSeconds(0.1f / 100);
        }
        StopCoroutine(timerStart);
    }

    private IEnumerator fadingDeath()
    {
        for (int i = 1; i <= 100; i++)
        {
            vig.intensity = vigAtFunctionStart + (1 - vigAtFunctionStart) * (((float)i) / 100);
            vig.blur = (((float)i) / 100);
            yield return new WaitForSeconds(1.6f / 100);
        }
        StopCoroutine(timerEnd);
    }


}
