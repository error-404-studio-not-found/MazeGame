using System.Collections;

using UnityEngine;
using static UnityEngine.UI.Image;

public class Health : MonoBehaviour
{

    public float playerHealth = 100f;

    private float defaultHealth = 0;

    [SerializeField]
    private Transform respawnPoint;

    [SerializeField]
    private Rigidbody rb;

    [SerializeField]
    private BasicMovement movement;

    [SerializeField]
    private LayerMask groundLayer;

    public float respawnTime;

    private float startingTime;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()

    {

        defaultHealth = playerHealth;

        startingTime = respawnTime;


        // set spwan point properly
        /* 
        RaycastHit hit;

        if (Physics.Raycast(respawnPoint.transform.position, Vector3.down, out hit, Mathf.Infinity, groundLayer))
        {
            // 4. Get the exact Vector3 impact position in world space
            Vector3 hitPosition = hit.point;

            respawnPoint.position = new Vector3(respawnPoint.transform.position.x, hit.transform.position.y + 2, respawnPoint.transform.position.z);

            // Example usage: log the position or move an object to it
            Debug.Log("Raycast hit at position: " + hitPosition);
        }

        respawn();
        */




    }

    private IEnumerator TimerTick()
    {

        respawnTime = startingTime;

        while (respawnTime > 0)
        {

            // Wait exactly 1 real-time second before continuing the loop
            yield return new WaitForSeconds(1f);

            respawnTime--;
        }


        Debug.Log("Respawing");
        respawn();

    }

    // Update is called once per frame
    void Update()
    {
        if (playerHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        // Handle player death (e.g., respawn, game over, etc.)

        movement.enabled = false;
        rb.freezeRotation = false;


        StartCoroutine(TimerTick());
    }


    public void respawn()
    {
        playerHealth = defaultHealth;

        transform.position = respawnPoint.position;


        movement.enabled = true;

        transform.rotation = Quaternion.identity;
        rb.freezeRotation = true;
    }
}
