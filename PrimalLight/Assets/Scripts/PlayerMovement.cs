using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float movementSpeed = 5.0f;
    public float jumpForce = 5f;
    public float airMovementMultiplier = 0.5f;

    [Header("Physics")]
    public Vector3 footOffset = new Vector3(0.1f, 0f, 0f);
    public float groundDistance = 0.1f;

    public bool isOnGround;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        PhysicsCheck();
        Move();
    }

    void PhysicsCheck()
    {
        //Default values
		isOnGround = false;

		//Cast rays for the left and right foot
		bool leftCheck  = Physics.Raycast(transform.position + footOffset, -Vector3.up, groundDistance);
        bool rightCheck = Physics.Raycast(transform.position - footOffset, -Vector3.up, groundDistance);
        //Debug.DrawRay(transform.position + footOffset, -Vector3.up * groundDistance, leftCheck ? Color.red : Color.green);
        //Debug.DrawRay(transform.position - footOffset, -Vector3.up * groundDistance, leftCheck ? Color.red : Color.green);

        isOnGround = leftCheck || rightCheck;
    }

    void Move()
    {
        //Input
        Vector3 input = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
        bool jumpPressed = Input.GetButtonDown("Jump");


        //Walk
        Vector3 velocity = input * movementSpeed;
        if(!isOnGround) 
            velocity *= airMovementMultiplier; //Limit walk control in the air
        velocity = Vector3.ClampMagnitude(velocity, movementSpeed); //Clamp velocity
        velocity.y = rb.velocity.y;
        rb.velocity = velocity;


        //Jump
        if(jumpPressed && isOnGround)
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }
}
