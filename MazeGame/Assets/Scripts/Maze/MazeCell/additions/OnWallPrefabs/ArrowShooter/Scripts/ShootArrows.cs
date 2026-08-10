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

    public float fireStrength = 5.0f;

    public float betweenArrowDelay = 0.2f;

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
            Debug.Log("shooting");

            GameObject arrowClone;

            arrowClone = Instantiate(arrow, new Vector3(arrowSpawns[i].position.x, arrowSpawns[i].position.y, arrowSpawns[i].position.z + arrowLength), Quaternion.identity);

            Rigidbody arrowRb = arrowClone.GetComponent<Rigidbody>();

            arrowRb.AddForce(new Vector3(0f, 0f, -1f) * fireStrength, ForceMode.Impulse);


        }

        triggered = false;

    }
}
