using UnityEngine;

public class OnWallObj : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public GameObject vines;

    public GameObject arrowShooter;

    void Start()
    {

        if (Random.value < 0.2f && vines != null)
        {
            GameObject clone = null;

            if (Random.value < 0.5f)
               clone = Instantiate(vines, new Vector3(3, -5, 0), Quaternion.identity);
            else
               clone = Instantiate(vines, new Vector3(-3, -5, 0), Quaternion.identity);


            clone.transform.SetParent(transform, false);
            clone.transform.rotation = transform.rotation;

        }

        if (Random.value < 0.2f && arrowShooter != null)
        {
            GameObject clone = null;

            if (Random.value < 0.5f)
            {
                clone = Instantiate(arrowShooter, new Vector3(10, -32, 0), Quaternion.identity);

                
                clone.transform.SetParent(transform, false);   
                clone.transform.localRotation = Quaternion.identity;

                clone.transform.localPosition = new Vector3( 10, -32, 0f );
            }
            else
            {
                clone = Instantiate(arrowShooter, new Vector3(-10, -32, 0), Quaternion.identity);

                clone.transform.SetParent(transform, false);
                clone.transform.localRotation = Quaternion.Euler(0, 180, 0);

                clone.transform.localPosition = new Vector3(-10, -32, 0f);

            }


            if (clone.transform.rotation.y == -90)
            {
                Debug.Log("gelp");
                

            }




        }


    }

    // Update is called once per frame
    void Update()
    {
        


    }
}
