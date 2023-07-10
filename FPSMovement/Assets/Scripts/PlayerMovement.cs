using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Key bindings")]
    public static KeyCode jumpKey = KeyCode.Space;
    public static KeyCode crouchKey = KeyCode.LeftShift;
    public static KeyCode runningKey = KeyCode.LeftControl;

    [Header("Moving")]
    [SerializeField] float moveSpeed;
    [SerializeField] float walkingSpeed;
    [SerializeField] float runningSpeed;
    [SerializeField] float crouchSpeed;
    [Header("Crouching")]
    [SerializeField] float crouchHeight;
    [SerializeField] bool canStandUp;
    [Header("Jumping")]
    [SerializeField] float jumpingForce;
    [SerializeField] float jumpingCooldown;
    [SerializeField] float airMultiplier;
    [SerializeField] bool canJump;
    [Header("Physics")]
    [SerializeField] float gravityForce;
    [SerializeField] bool canMove;
    [SerializeField] float groundDrag;
    [SerializeField] LayerMask ground;
    [Header("Camera")]
    [SerializeField] Camera mainCamera;
    [SerializeField] float cameraHeightPos;

    private float xInput;
    private float yInput;
    private bool isGrounded;
    private float playerHeight;

    private CapsuleCollider playerCollider;
    private Rigidbody playerRb;
    private Vector3 moveDirection;
    private Transform player;
    private PlayerState state;

    public enum PlayerState
    {
        walking,
        crouching,
        running,
        air
    }

    
    void Start()
    {
        //assigning variables
        canJump = true;
        player = GetComponent<Transform>().transform;
        playerRb = GetComponent<Rigidbody>();
        playerCollider = GetComponent<CapsuleCollider>();
        playerHeight = playerCollider.height;
        playerRb.freezeRotation = true;
    }
    void Update()
    {
        //getting inputs
        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");
        //jumping
        if(Input.GetKey(jumpKey) && isGrounded && canJump)
        {
            canJump = false;
            Jump();
            Invoke("CanJumpReset", jumpingCooldown);
        }
        canStandUp = Physics.Raycast(transform.position, Vector3.up, playerHeight * 0.5f);
        if (Input.GetKey(crouchKey) && !Input.GetKey(runningKey))
        {
            playerCollider.height *= crouchHeight;
            playerCollider.center = new Vector3(0, -1 * (player.position.y * .5f), 0);
            mainCamera.transform.position = new Vector3(player.transform.position.x, (player.transform.position.y + cameraHeightPos) * crouchHeight, player.transform.position.z);
            canStandUp = true;
        }
        else 
        {
            if (!canStandUp)
            {
                playerCollider.height = playerHeight;
                playerCollider.center = new Vector3(0, 0, 0);
                mainCamera.transform.position = new Vector3(player.transform.position.x, player.transform.position.y + cameraHeightPos, player.transform.position.z);
            }
        } 
        //checking if is grounded
        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.1f, ground); 
        //adding drag (removing sliding)
        if (isGrounded) playerRb.drag = groundDrag;
        else playerRb.drag = 0;
        //removing acceleration (speed control)
        Vector3 actualVelocity = new Vector3(playerRb.velocity.x, 0f, playerRb.velocity.z);
        if(actualVelocity.magnitude > moveSpeed)
        {
            Vector3 limitedVelocity = actualVelocity.normalized * moveSpeed;
            playerRb.velocity = new Vector3(limitedVelocity.x, playerRb.velocity.y, limitedVelocity.z);
        }
        MovePlayer();
        StateHandler();
    }
    //getting direction and adding force
    private void MovePlayer()
    {
        moveDirection = player.forward * yInput + player.right * xInput;
        playerRb.AddForce(isGrounded ? moveDirection.normalized * walkingSpeed : moveDirection.normalized * walkingSpeed * airMultiplier, ForceMode.Force);

    }
    //jumping
    private void Jump()
    {
        playerRb.velocity = new Vector3(playerRb.velocity.x, 0f, playerRb.velocity.z);
        playerRb.AddForce(transform.up * jumpingForce, ForceMode.Impulse);
    }
    //jumping reset
    private void CanJumpReset()
    {
        canJump = true;
    }
    //checking state
    private void StateHandler()
    {

        if(isGrounded && Input.GetKey(crouchKey) && !Input.GetKey(runningKey))
        {
            state = PlayerState.crouching;
            moveSpeed = crouchSpeed;
        }
        else if (isGrounded && Input.GetKey(runningKey) && !Input.GetKey(crouchKey))
        {
            state = PlayerState.running;
            moveSpeed = runningSpeed;
        }
        else if (isGrounded)
        {
            state = PlayerState.walking;
            moveSpeed = walkingSpeed;
        }
        else
        {
            state = PlayerState.air;
        }
    }
}
