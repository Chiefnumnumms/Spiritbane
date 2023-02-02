using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowMotion : MonoBehaviour
{
    private float originalFixedDeltaTime;
    private Coroutine slowMotionCoroutine;

    private IEnumerator SlowMotionSequence(float duration, float slowMotionFactor)
    {
        originalFixedDeltaTime = Time.fixedDeltaTime;
        Time.timeScale = slowMotionFactor;
        Time.fixedDeltaTime = Time.fixedDeltaTime * slowMotionFactor;

        yield return new WaitForSecondsRealtime(duration);

        Time.timeScale = 1;
        Time.fixedDeltaTime = originalFixedDeltaTime;
    }

    public void StartSlowMotionSequence(float duration, float slowMotionFactor)
    {
        if (slowMotionCoroutine != null)
        {
            StopCoroutine(slowMotionCoroutine);
        }
        slowMotionCoroutine = StartCoroutine(SlowMotionSequence(duration, slowMotionFactor));
    }

    public void StopSlowMotionSequence()
    {
        if (slowMotionCoroutine != null)
        {
            StopCoroutine(slowMotionCoroutine);
            Time.timeScale = 1;
            Time.fixedDeltaTime = originalFixedDeltaTime;
            slowMotionCoroutine = null;
        }
    }
}
