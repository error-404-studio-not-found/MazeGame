using UnityEngine;

public class OnWallObj : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public GameObject vines;

    void Start()
    {

        if (Random.value < 0.2f && vines != null)
        {
            GameObject clone = Instantiate(vines, new Vector3(3, -5, 0), Quaternion.identity);
            clone.transform.SetParent(transform, false);
            clone.transform.rotation = transform.rotation;

        }
        

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
