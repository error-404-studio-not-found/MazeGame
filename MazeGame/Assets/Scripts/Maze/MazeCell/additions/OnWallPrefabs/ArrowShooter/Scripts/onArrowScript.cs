using System.Collections;
using UnityEngine;

public class onArrowScript : MonoBehaviour
{

    Rigidbody rb;

    public float fireStrength = 10.0f;

    public float despawnTimeSeconds = 7;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        rb = GetComponent<Rigidbody>();

        float direction = 1;

        Debug.Log(transform.parent.parent.parent.localRotation.eulerAngles);

        if (transform.parent.parent.parent.localRotation.eulerAngles.y == 180)
        {


            rb.AddForce(-transform.forward * fireStrength, ForceMode.Impulse);

            Debug.Log("check");

        }
        else
        { 

            rb.AddForce(transform.forward * fireStrength, ForceMode.Impulse); 

        }


        StartCoroutine(destroyDelay(despawnTimeSeconds));



    }

    private IEnumerator destroyDelay(float waitTime)
    {
        float timeRemaining = waitTime;

        while (timeRemaining > 0)
        {
            yield return new WaitForSeconds(1f);
            timeRemaining--;
        }


        Destroy(gameObject);

        yield break;
    }

    // Update is called once per frame
    void Update()
    {

       

    }
}
