using UnityEngine;

public class BasicMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    private float defaultMoveSpeed;

    public float groundDrag;

    [Header("Jumping")]
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump = true;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;
    public KeyCode crouchKey = KeyCode.LeftControl;

    [Header("Ground Check")]
    public LayerMask groundLayer;
    public float playerHeight;
    public bool grounded;

    [Header("Climbing")]
    public LayerMask isClimbable;
    public bool vinesClimbing;
    public float checkDistance;
    public float climbSpeed;


    [Header("Other")]
    public Transform oreintation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        defaultMoveSpeed = moveSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        // ground check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.3f, groundLayer);






        MyInputs();
        controlSpeed();

        if (grounded)
        {
            rb.linearDamping = groundDrag;
        }
        else
        {
            rb.linearDamping = 0;
        }
    }


    private void FixedUpdate()
    {
        movePlayer();
    }

    private void MyInputs()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if ((Input.GetKey(jumpKey) && readyToJump && grounded))
        {
           
             readyToJump = false;
             Jump();
             Invoke(nameof(ResetJump), jumpCooldown);

        }

        if (Input.GetKey(sprintKey) && grounded)
        {
            moveSpeed = defaultMoveSpeed * 2f;
        }
        else
        {
            moveSpeed = defaultMoveSpeed;
        }

        if (Input.GetKey(crouchKey) && grounded)
        {
            moveSpeed = defaultMoveSpeed / 2f;

        }
    }

    private void movePlayer()
    {
        // calculate the move direction based on the input and orientation

        moveDirection = oreintation.forward * verticalInput + oreintation.right * horizontalInput;

        //on ground
        if (grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);

        //in air
        else if (!grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
    }

    private void controlSpeed()
    {
        Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.linearVelocity = new Vector3(limitedVel.x, rb.linearVelocity.y, limitedVel.z);
        }
    }

    private void Jump()
    {
        // reset y velocity
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        readyToJump = true;
    }
}