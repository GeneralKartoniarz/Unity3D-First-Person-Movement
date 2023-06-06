using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Key bindings")]
    public static KeyCode jumpKey = KeyCode.Space;
    public static KeyCode crouchKey = KeyCode.LeftShift;
    [Header("Moving")]
    [SerializeField] float walkingSpeed;
    [SerializeField] float runningSpeed;
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

    private float xInput;
    private float yInput;
    private bool isRunning;
    private bool isGrounded;
    private float playerHeight;

    private Rigidbody playerRb;
    private Vector3 moveDirection;
    private Transform player;


    
    void Start()
    {
        //assigning variables
        canJump = true;
        playerHeight = 2f;
        player = GetComponent<Transform>().transform;
        playerRb = GetComponent<Rigidbody>();
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
        //checking if is grounded
        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.1f, ground);
        //adding drag (removing sliding)
        if (isGrounded) playerRb.drag = groundDrag;
        else playerRb.drag = 0;
        //removing acceleration (speed control)
        Vector3 actualVelocity = new Vector3(playerRb.velocity.x, 0f, playerRb.velocity.z);
        if(actualVelocity.magnitude > walkingSpeed)
        {
            Vector3 limitedVelocity = actualVelocity.normalized * walkingSpeed;
            playerRb.velocity = new Vector3(limitedVelocity.x, playerRb.velocity.y, limitedVelocity.z);
        }
        MovePlayer();
    }
    //getting direction and adding force
    private void MovePlayer()
    {
        moveDirection = player.forward * yInput + player.right * xInput;
        if(isGrounded)
            playerRb.AddForce(moveDirection.normalized * walkingSpeed, ForceMode.Force);
        else
            playerRb.AddForce(moveDirection.normalized * walkingSpeed * airMultiplier, ForceMode.Force);

    }
    private void Jump()
    {
        playerRb.velocity = new Vector3(playerRb.velocity.x, 0f, playerRb.velocity.z);
        playerRb.AddForce(transform.up * jumpingForce, ForceMode.Impulse);
    }
    private void CanJumpReset()
    {
        canJump = true;
    }
}
