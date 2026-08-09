using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class CrushPlayer : MonoBehaviour
{
    [SerializeField]
    private openGate openGate;

    [SerializeField]
    private Health health;

    private void Start()
    {
        GameObject player = GameObject.FindWithTag("player");

        health = player.GetComponent<Health>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("player")) 
        { 
        
            Debug.Log("player killed");

        
            health.playerHealth = 0;
        }

    }
}
