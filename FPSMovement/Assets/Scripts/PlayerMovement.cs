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

    private float xInput;
    private float yInput;
    private bool isRunning;

    private Rigidbody playerRb;
    private Vector3 moveDirection;
    private Transform player;
    
    void Start()
    {
        //assigning variables
        player = GetComponent<Transform>().transform;
        playerRb = GetComponent<Rigidbody>();
        playerRb.freezeRotation = true;
    }
    void Update()
    {
        //getting inputs
        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");
        MovePlayer();
    }
    
    //getting direction and adding force
    private void MovePlayer()
    {
        moveDirection = player.forward * yInput + player.right * xInput;
        playerRb.AddForce(moveDirection.normalized * walkingSpeed, ForceMode.Force);
    }
}
