using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

public class openGate : MonoBehaviour
{
    public GameObject leftGate;
    public GameObject rightGate;

    // Duration (seconds) for the gate opening/closing movement
    [SerializeField]
    private float openSpeed = 30f;

    // Curve that maps normalized time [0..1] -> interpolation value [0..1]
    // Set this in the Inspector. Default is ease-in-out.
    [SerializeField]
    private AnimationCurve openCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

    [SerializeField] 
    private int startingTime = 60;

    private int timeRemaining = 260;

    public bool openGates = false;

    public bool killPlayerB = false;

    [SerializeField]
    private BoxCollider killCol;

    private void Start()
    {
        openSpeed = 30;

        startingTime = timeRemaining;

        StartCoroutine(OnTimerComplete());

        GameObject player = GameObject.FindWithTag("player");
    }

    private IEnumerator TimerTick()
    {
        timeRemaining = startingTime;

        while (timeRemaining > 0)
        {
            yield return new WaitForSeconds(1f);
            timeRemaining--;
        }

        Debug.Log("Time has run out!");
        yield return StartCoroutine(OnTimerComplete());
    }

    private IEnumerator OnTimerComplete()
    {

        openGates = !openGates;

        if (openGates)
        {
            killCol.enabled = false;
        }

        float directonToMoveLeft = 5f;
        float directonToMoveRight = -5f;

        if (openGates)
        {
            directonToMoveLeft = -5f;
            directonToMoveRight = 5f;
        }
        else if (!openGates)
        {
            directonToMoveLeft = 5f;
            directonToMoveRight = -5f;
        }

        float t = 0f;

        var startPosLeft = leftGate.transform.position;
        var startPosRight = rightGate.transform.position;

        // Use the curve to control interpolation over the duration
        while (t < openSpeed)
        {
            t += Time.deltaTime;
            float normalized = Mathf.Clamp01(t / openSpeed);          // 0..1
            float curveT = openCurve.Evaluate(normalized);           // remapped by curve

            leftGate.transform.position = Vector3.Lerp(startPosLeft, startPosLeft + new Vector3(directonToMoveLeft, 0f, 0f), curveT);
            rightGate.transform.position = Vector3.Lerp(startPosRight, startPosRight + new Vector3(directonToMoveRight, 0f, 0f), curveT);

            yield return null;
        }

        if (!openGates)
            killCol.enabled = true;

        StartCoroutine(TimerTick());
    }
}