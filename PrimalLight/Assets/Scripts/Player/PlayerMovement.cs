using System;
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
    public LayerMask groundLayer;

    public bool isOnGround;

    private Rigidbody rb;
    private Animator anim;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        if( GameManager.IsInputCaptured() ) {
            StopPlayer();
            Animate();
            return;
        }

        PhysicsCheck();
        Move();
        Animate();
    }

    void PhysicsCheck()
    {
        //Default values
        isOnGround = false;

        //Cast rays for the left and right foot
        bool leftCheck  = Physics.Raycast(transform.position + footOffset, -Vector3.up, groundDistance, groundLayer.value);
        bool rightCheck = Physics.Raycast(transform.position - footOffset, -Vector3.up, groundDistance, groundLayer.value);
        //Debug.DrawRay(transform.position + footOffset, -Vector3.up * groundDistance, leftCheck ? Color.red : Color.green);
        //Debug.DrawRay(transform.position - footOffset, -Vector3.up * groundDistance, leftCheck ? Color.red : Color.green);

        isOnGround = leftCheck || rightCheck;
    }

    void Move()
    {
        //Input
        Vector3 input = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
        bool jumpPressed = Input.GetButtonDown("Jump");

        // the movement direction is the way the came is facing
        Vector3 Direction = Camera.main.transform.forward * input.z +
                            Vector3.Cross(Camera.main.transform.forward, Vector3.up) *  -input.x;

        //rotates the player to face in the camera direction if he is moving
        if(Math.Abs(input.x) > 0.0f || Math.Abs(input.z) > 0.0f)
        {
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, Camera.main.transform.localEulerAngles.y, transform.localEulerAngles.z);
        }

        //Walk
        Vector3 velocity = Direction * movementSpeed;
        if(!isOnGround) 
            velocity *= airMovementMultiplier; //Limit walk control in the air
        velocity = Vector3.ClampMagnitude(velocity, movementSpeed); //Clamp velocity
        velocity.y = rb.velocity.y;

        rb.velocity = velocity;

        //Jump
        if(jumpPressed && isOnGround)
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            
    }

    void Animate()
    {
        Vector3 velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        float movementSpeed = velocity.magnitude;
        float jumpingSpeed = Mathf.Abs(Mathf.Clamp(1.0f/rb.velocity.y, -1f, 1f));

        anim.SetFloat("movementSpeed", movementSpeed);
        anim.SetBool("isJumping", !isOnGround);
        anim.SetFloat("jumpingSpeed", jumpingSpeed);
    }

    void StopPlayer()
    {
        Vector3 stopVelocity = Vector3.zero;
        stopVelocity.y = rb.velocity.y;
        rb.velocity = stopVelocity;
    }
}
