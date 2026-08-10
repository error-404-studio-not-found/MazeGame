using UnityEngine;
using System.Collections;

public class DayNightCycle : MonoBehaviour
{
    public GameObject SunLight;
    [Tooltip("Cycle length in minutes")]
    public float cycleMinutes = 6f;
    public bool useUnscaledTime = true;

    private float cycleSeconds;

    public bool IsDaytime()
    {
        if (SunLight == null) return true;
        float sunAngle = transform.rotation.eulerAngles.x;
        return sunAngle >= 0f && sunAngle < 180f;
    }

    private void Start()
    {
        cycleSeconds = Mathf.Max(0.0001f, cycleMinutes * 60f); // avoid division by zero
        StartCoroutine(OnTimerComplete());
    }

    private IEnumerator OnTimerComplete()
    {
        float elapsed = 0f;
        var sunStartEuler = transform.rotation.eulerAngles;

        while (true)
        {


            elapsed += useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
            float progress = (elapsed % cycleSeconds) / cycleSeconds;
            float angle = 360f * progress;
            transform.rotation = Quaternion.Euler(sunStartEuler + new Vector3(angle, 0f, 0f));
            yield return null;
        }
    }
}