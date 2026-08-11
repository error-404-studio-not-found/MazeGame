using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShootArrows : MonoBehaviour
{
    [Header("Refrences")]
    public GameObject arrow;

    public DeployShooter deployScript;

    [Header("Firing Customization")]
    public Transform[] arrowSpawns;

    public float shootDelay = 3f;

    public float arrowLength;


    public float betweenArrowDelay = 5f;

    private bool triggered = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (!triggered && !deployScript.isdeploying && deployScript.deployed)
        {

            triggered = true;
            StartCoroutine(ShootDelay(shootDelay));

        }

    }

    private IEnumerator ShootDelay(float waitTime)
    {
        float timeRemaining = waitTime;

        while (timeRemaining > 0)
        {
            yield return new WaitForSeconds(1f);
            timeRemaining--;
        }


        spawnArrows();

        yield break;
    }

    void spawnArrows()
    {
        for (int i = 0; i < arrowSpawns.Length; i++)
        {
            StartCoroutine(SpawnDelay(betweenArrowDelay * i, i));
        }

        triggered = false;

    }

    private IEnumerator SpawnDelay(float waitTime, int index)
    {
        if (deployScript.deployed)
        {



            float timeRemaining = waitTime;

            while (timeRemaining > 0)
            {
                yield return new WaitForSeconds(1f);
                timeRemaining--;
            }

            yield return StartCoroutine(spawnArrow(index));


        }
        else
        {
            yield return null;
            Debug.Log("Cant " + deployScript.deployed);
        }
    }

    private IEnumerator spawnArrow(int i)
    {


        Debug.Log("shooting");

        GameObject arrowClone;

        arrowClone = Instantiate(arrow, new Vector3(arrowSpawns[i].position.x, arrowSpawns[i].position.y, arrowSpawns[i].position.z + arrowLength), transform.parent.rotation);

        arrowClone.transform.SetParent(transform, true);

        arrowClone.transform.rotation = Quaternion.Euler(arrowClone.transform.rotation.x, arrowSpawns[i].transform.rotation.y + -90, arrowClone.transform.rotation.z);

        Rigidbody rb = arrowClone.GetComponent<Rigidbody>();

        rb.AddForce(-arrowSpawns[i].transform.right * 50, ForceMode.Impulse);

        arrowClone.transform.SetParent(transform, true);

        yield break;

    }

}
