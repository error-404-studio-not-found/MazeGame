using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    public float sensX;
    public float sensY;

    public Transform orientation;


    float xRotation;
    float yRotation;

    public void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void Update()
    {
        MyInput();
    }

    /*
     * void responsible for getting the mouse input and rotating the camera and orientation based on that input
     * 
     * 
     */
    private void MyInput()
    {
        // get the mouse input and multiply it by the sensitivity and delta time to make it frame rate independent
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

        // calculate the rotation of the camera based on the mouse input
        yRotation += mouseX;
        xRotation -= mouseY;

        // clamp the x rotation to prevent the camera from flipping over
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // set the rotation of the camera and orientation based on the mouse input
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
    }
}
