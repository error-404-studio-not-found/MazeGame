using UnityEngine;

public class DamagePlayer : MonoBehaviour
{

    public int damage;
    private Health health;
    Collider col;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        col = GetComponent<Collider>();

    }

    private void OnTriggerEnter(Collider other)
    {

        Debug.Log("hit");
        Debug.Log(other.tag);

        if (other.CompareTag("player"))
        {
            Debug.Log("hit player");

            GameObject collided = other.gameObject;
            health = collided.transform.parent.GetComponent<Health>();

            health.playerHealth -= damage;

            Debug.Log("player health = " + health.playerHealth);

        }

        col.enabled = false;

    }

    // Update is called once per frame
    void Update()
    {
        


    }
}
