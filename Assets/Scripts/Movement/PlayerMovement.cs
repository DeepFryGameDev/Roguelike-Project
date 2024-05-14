using UnityEngine;

// Purpose: Provides the base moving the player around the world
// Directions: Attach to player GameObject
// Other notes: Written from YouTube tutorial: https://www.youtube.com/watch?v=f473C43s8nE

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [Tooltip("Base speed at which the player moves")]
    [SerializeField] float moveSpeed;
    [Tooltip("Base speed at which the player moves while sprinting")]
    [SerializeField] float sprintSpeed;

    [Tooltip("Additional drag to apply to the player while grounded")]
    [SerializeField] float groundDrag;

    [Tooltip("Any additional force to add while the player is in the air")]
    [SerializeField] float airMultiplier;

    [Header("Keybinds")]
    [Tooltip("Key to hold to sprint")]
    [SerializeField] KeyCode sprintKey = KeyCode.LeftShift;
    //[Tooltip("Key to press to jump (not being used right now)")]
    //[SerializeField] KeyCode jumpKey = KeyCode.Space;

    [Header("Ground Check")]
    [Tooltip("Raycast is shot downward from the player's height to ensure they are grounded")]
    [SerializeField] float playerHeight;
    [Tooltip("Layer of ground")]
    [SerializeField] LayerMask whatIsGround;
    [Tooltip("Turns true when player is not in the air")]
    bool grounded;

    [Tooltip("Set to the Player -> Orientation Transform")]
    [SerializeField] Transform orientation;

    float horizontalInput; // Set to any movement from the player along the X axis
    float verticalInput; // Set to any movement from the player along the Y axis

    Vector3 moveDirection; // Direction the player is moving towards

    BasePlayer player; // Used to gather the player's stamina levels
    PlayerManager pm; // Used to manipulate stamina when player sprints

    Rigidbody rb; // Used to add force to the GameObject so the player is able to move
    Animator anim; // Used to change the player's animations while walking and sprinting

    static PlayerMovement instance; // For singleton to ensure script persists across scenes

    // -- Disabled for now - jumping is not active
    //public float jumpForce;
    //public float jumpCooldown;
    // bool readyToJump;

    void Awake()
    {
        Singleton();

        SetVars();
    }

    void Singleton()
    {
        if (instance == null) //check if instance exists
        {
            instance = this; //if not set the instance to this
        }
        else if (instance != this) //if it exists but is not this instance
        {
            Destroy(gameObject); //destroy it
        }
        DontDestroyOnLoad(gameObject); //set this to be persistable across scenes
    }

    void SetVars()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        anim = GetComponentInChildren<Animator>();

        // readyToJump = true;

        player = GetComponent<BasePlayer>();
        pm = GetComponent<PlayerManager>();
    }

    void FixedUpdate()
    {
        MovePlayer();
    }

    void Update()
    {
        // ground check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * .1f, whatIsGround);

        GetInput();
        SpeedControl();

        // handle drag
        if (grounded)
        {
            rb.drag = groundDrag;
        } else
        {
            rb.drag = 0;
        }
    }

    /// <summary>
    /// Gather the player's input for movement and process animations/sprinting mechanisms
    /// </summary>
    void GetInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (horizontalInput != 0 || verticalInput != 0)
        {
            if (Input.GetKey(sprintKey) && canSprint())
            {
                anim.SetBool("isWalking", false);
                anim.SetBool("isRunning", true);

                pm.ReduceStaminaFromSprint();
                pm.SetStandingStill(false);
            } else
            {
                anim.SetBool("isWalking", true);
                anim.SetBool("isRunning", false);

                pm.SetStandingStill(false);
                pm.RecoverStamina();
            }
        } else
        {
            anim.SetBool("isWalking", false);
            anim.SetBool("isRunning", false);

            pm.SetStandingStill(true);
            pm.RecoverStamina();
        }

        // when to jump
        /*if (Input.GetKey(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;

            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);
        }*/
    }

    /// <summary>
    /// Uses the attached RigidBody to move the player around the world
    /// </summary>
    void MovePlayer()
    {
        // calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // on ground
        if (grounded)
        {
            if (Input.GetKey(sprintKey) && canSprint())
            {
                rb.AddForce(moveDirection.normalized * sprintSpeed * 10f, ForceMode.Force);
            } else
            {
                rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
            }
            
        }

        // in air
        else if (!grounded)
        {
            if (Input.GetKey(sprintKey) && canSprint())
            {
                rb.AddForce(moveDirection.normalized * sprintSpeed * 10f * airMultiplier, ForceMode.Force);
            } else
            {
                rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
            }
        }
    }

    /// <summary>
    /// Returns true if player's stamina is greater than 0 and has not been fully depleted
    /// </summary>
    bool canSprint()
    {
        if (player.GetCurrentStamina() > 0 && !pm.GetStamDepleted())
        {
            return true;
        }
        else
        {
            //Debug.Log("Unable to sprint because player's stam: " + player.currentStamina + " and stamDepleted: " + pm.stamDepleted);
            return false;
        }
    }

    /// <summary>
    /// Ensures the rigidbody's velocity does not exceed moveSpeed
    /// </summary>
    void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        // limit velocity if needed
        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }

        /*if (Input.GetAxisRaw("Horizontal").Equals(0) && Input.GetAxisRaw("Vertical").Equals(0) && !rb.isKinematic && grounded)
        {
            rb.isKinematic = true;
        } else
        {
            rb.isKinematic = false;
        }*/
    }

    /*void Jump()
    {
        // reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    void ResetJump()
    {
        readyToJump = true;
    }*/
}
