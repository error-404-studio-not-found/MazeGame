using System.Collections;
using UnityEngine;

public class openGate : MonoBehaviour
{
    public GameObject leftGate;
    public GameObject rightGate;
    private float openSpeed = 30f; // Speed at which the gates open

    [SerializeField] 
    private int startingTime = 60;

    private int timeRemaining = 260;

    public bool openGates = false;

    [SerializeField]
    private BoxCollider killCol;

    [SerializeField]
    private Health health;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
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

            // Wait exactly 1 real-time second before continuing the loop
            yield return new WaitForSeconds(1f);

            timeRemaining--;
        }


        Debug.Log("Time has run out!");
        yield return StartCoroutine(OnTimerComplete());

    }

    private IEnumerator OnTimerComplete()
    {

        GameObject player = GameObject.FindWithTag("player");

        health = player.GetComponent<Health>();

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

        while (t < openSpeed)
        {
            t += Time.deltaTime;

            leftGate.transform.position = Vector3.Lerp(startPosLeft,startPosLeft + new Vector3(directonToMoveLeft, 0f, 0f), t / openSpeed);
            rightGate.transform.position = Vector3.Lerp(startPosRight,startPosRight + new Vector3(directonToMoveRight, 0f, 0f), t / openSpeed);


            yield return null;

        }

        if (!openGates)
        {

            killCol.enabled = true;
            OnTriggerEnter(killCol);

        }

        StartCoroutine(TimerTick());

    }


    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("player"))
            health.playerHealth = 0;


    }
}
