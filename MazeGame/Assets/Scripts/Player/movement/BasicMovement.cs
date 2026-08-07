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
    public KeyCode climbKey = KeyCode.W;

    [Header("Ground Check")]
    public LayerMask groundLayer;
    public float playerHeight;
    public bool grounded;

    [Header("Climbing")]
    public LayerMask isClimbable;
    public bool climbing;
    public float climbSpeed;


    [Header("ClimbingDetection")]
    public float checkDistance;
    public float sphereRadius;
    public float maxWallAngle;
    private float wallAngle;

    private RaycastHit frontWallHit;
    private bool wallFront;



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


        wallCheck();



        MyInputs();
        controlSpeed();

        // climging movement
        if (climbing) climbingMovement();


        // handle drag
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

        if (Input.GetKey(jumpKey) && readyToJump && grounded)
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

        if (Input.GetKey(climbKey) && wallFront && wallAngle < maxWallAngle)
        {

            startClimb();
        }
        else
        {
            if (climbing)
            {
                stopClimb();
            }
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

    // --- Climbing ---
    private void wallCheck()
    {
        wallFront = Physics.SphereCast(transform.position, sphereRadius, oreintation.forward, out frontWallHit, checkDistance, isClimbable);
        wallAngle = Vector3.Angle(oreintation.forward, -frontWallHit.normal);
    }

    private void startClimb()
    {
        climbing = true;
    }

    private void climbingMovement()
    {
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, climbSpeed, rb.linearVelocity.z);
    }

    private void stopClimb()
    {
        climbing = false;
    }

}