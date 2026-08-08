using System.Collections;

using UnityEngine;

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

    public float respawnTime;

    private float startingTime;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()

    {

        defaultHealth = playerHealth;

        startingTime = respawnTime;

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
