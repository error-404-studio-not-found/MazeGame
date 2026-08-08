using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class CrushPlayer : MonoBehaviour
{
    private bool isCrushing = false;

    [SerializeField]
    private openGate openGate;

    [SerializeField]
    private float timeToStart = 10f; // Time remaining for the crushing action

    private float startingTime;

    private Collider death;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        timeToStart = startingTime;

        death = GetComponent<Collider>();

    }

    // Update is called once per frame
    void Update()
    {
        
        isCrushing = openGate.openGates;
        
        if (isCrushing)
        {

            StartCoroutine(TimerTick());
            isCrushing = false;

        }

    }

    private IEnumerator TimerTick()
    {

        timeToStart = startingTime;

        while (timeToStart > 0)
        {

            // Wait exactly 1 real-time second before continuing the loop
            yield return new WaitForSeconds(1f);

            timeToStart--;
        }


        Debug.Log("Time has run out!");
        yield return StartCoroutine(OnTimerComplete());

    }


    private IEnumerator OnTimerComplete()
    {

        yield return new WaitForSeconds(timeToStart);

    }
}
