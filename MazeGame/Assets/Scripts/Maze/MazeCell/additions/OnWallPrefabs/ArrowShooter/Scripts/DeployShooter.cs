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



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {

        deployed = true;


        StartCoroutine(deployDelay());
    }

   

    private void OnTriggerExit(Collider other)
    {
        deployed = false;


        StartCoroutine(deployDelay());



    }
    IEnumerator deployDelay()
    {

        Debug.Log("waiting");

        yield return new WaitUntil(() => !isdeploying);

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



        var startPosLeft = transform.position;

        while (t < deployTime)
        {
            isdeploying = true;

            t += Time.deltaTime;

            transform.position = Vector3.Lerp(startPosLeft, startPosLeft + new Vector3(0f, 0f, moveDirecZ), t / deployTime);


            yield return null;

        }

        isdeploying = false;
    }
}
