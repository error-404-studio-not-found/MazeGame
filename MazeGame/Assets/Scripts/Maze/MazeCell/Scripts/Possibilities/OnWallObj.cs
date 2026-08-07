using UnityEngine;

public class OnWallObj : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public GameObject vines;

    void Start()
    {
        
        if (Random.value < 0.2f)
        {
            vines.SetActive(true);
        }
        else
        {
            vines.SetActive(false);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
