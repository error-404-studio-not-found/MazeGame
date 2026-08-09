using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class DeployShooter : MonoBehaviour
{

    [SerializeField]
    private Collider DeployStart;

    [SerializeField]
    private float deployTime;

    private bool isdeploying = false;

    private bool deployed = false;

    private bool triggered = false;

    private float maxChange;

    private float minChange;

    private bool leftDuringDeploy = false;
    private bool enteredDuringRetract = false;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        maxChange = transform.localPosition.x - 5f; minChange = transform.localPosition.x;


    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {

        deployed = true;




        if (!triggered && transform.localPosition.x == minChange)
            StartCoroutine(deployDelay());

    }

   

    private void OnTriggerExit(Collider other)
    {
        deployed = false;


        if (!triggered && transform.localPosition.x == maxChange)
        {
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

        Debug.Log("moving shooter");

        float moveDirecZ = 5f;

        if (deployed)
        {
            moveDirecZ = -5f;
        }
        else if (!deployed)
        {
            moveDirecZ = 5f;
        }

        float t = 0f;



        var startPosLeft = transform.localPosition;

        Debug.Log("start pos =" + startPosLeft);

        

        while (t < deployTime)
        {
            isdeploying = true;

            t += Time.deltaTime;

            transform.localPosition = Vector3.Lerp(startPosLeft, startPosLeft + new Vector3(moveDirecZ, 0f, 0f), t / deployTime);


            yield return null;

        }


        triggered = false;
        isdeploying = false;
    }
}
