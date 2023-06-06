using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float walkingSpeed;
    [SerializeField] float runningSpeed;
    [SerializeField] float jumpingForce;
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
        //checking if is grounded
        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.1f, ground);
        //adding drag (removing sliding)
        if (isGrounded) playerRb.drag = groundDrag;
        else playerRb.drag = 0;

        MovePlayer();
    }
    
    //getting direction and adding force
    private void MovePlayer()
    {
        moveDirection = player.forward * yInput + player.right * xInput;
        playerRb.AddForce(moveDirection.normalized * walkingSpeed, ForceMode.Force);
    }
}
