using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class DeployShooter : MonoBehaviour
{

    [SerializeField]
    private Collider DeployStart;

    [SerializeField]
    private float deployTime;

    public bool isdeploying = false;

    public bool deployed = false;

    private bool triggered = false;

    private float maxChange;

    private float minChange;

    private Vector3 startingPosition;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startingPosition = transform.localPosition;
        maxChange = transform.localPosition.x - 5f;
        minChange = transform.localPosition.x;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        deployed = true;

        if (!triggered && transform.localPosition.x == minChange && (other.CompareTag("player") || other.CompareTag("enemy")))
        {
            deployed = true;
            StartCoroutine(deployDelay());
        }
    }



    private void OnTriggerExit(Collider other)
    {


        if (!triggered && transform.localPosition.x == maxChange && ( other.CompareTag("player") || other.CompareTag("enemy")))
        {
            deployed = false;
            StartCoroutine(deployDelay());
        }
    }

    IEnumerator deployDelay()
    {
        triggered = true;

        Debug.Log("waiting");

        yield return new WaitUntil(() => !isdeploying || !triggered);

        StartCoroutine(OnTimerComplete());
    }


    private IEnumerator OnTimerComplete()
    {
        isdeploying = true;

        Debug.Log("moving shooter");

        while (true)
        {
            // Determine target position based on deployed state
            Vector3 targetPos = deployed ?
                startingPosition + new Vector3(-5f, 0f, 0f) :
                startingPosition;

            // If already at target, we're done
            if (Vector3.Distance(transform.localPosition, targetPos) < 0.01f)
            {
                transform.localPosition = targetPos;
                break;
            }

            // Lerp from current position to target
            float t = 0f;
            Vector3 startPos = transform.localPosition;

            while (t < deployTime)
            {


                // Check if the target has changed (player entered/exited during movement)
                Vector3 currentTargetPos = deployed ?
                    startingPosition + new Vector3(-5f, 0f, 0f) :
                    startingPosition;

                if (currentTargetPos != targetPos)
                {
                    // Target changed, break out and recalculate from current position
                    break;
                }

                t += Time.deltaTime;
                transform.localPosition = Vector3.Lerp(startPos, targetPos, t / deployTime);

                yield return null;
            }
        }



        isdeploying = false;
        triggered = false;
    }
}