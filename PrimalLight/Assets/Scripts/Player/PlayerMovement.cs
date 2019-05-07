using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MovingObject
{
    [Header("Movement")]
    public float movementSpeed = 5.0f;
    public float pushingSpeed = 2.5f;
    public float jumpForce = 5f;
    public float airMovementMultiplier = 0.5f;

    [Header("Physics")]
    public Vector3 footOffset = new Vector3(0.1f, 0f, 0f);
    public float groundDistance = 0.1f;
    public LayerMask groundLayer;

    public bool isOnGround;
    public bool pushing = false;

    private bool pushButton;

    private Animator anim;
    private GameObject pushableObject = null;

    protected override void Start()
    {
        anim = GetComponent<Animator>();

        base.Start();
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
        pushButton = Input.GetButtonDown("Fire1");

        // the movement direction is the way the came is facing
        Vector3 Direction = Camera.main.transform.forward * input.z +
                            Vector3.Cross(Camera.main.transform.forward, Vector3.up) *  -input.x;

        //rotates the player to face in the camera direction if he is moving
        if(Math.Abs(input.x) > 0.0f || Math.Abs(input.z) > 0.0f)
        {
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, Camera.main.transform.localEulerAngles.y, transform.localEulerAngles.z);
        }

        if(pushing)
    		return;

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

	protected IEnumerator ClampToSpot (System.Action<bool> done, Vector3 targetPos, Quaternion targetRot, float inverseTransitionTime){
		int count = 2;
        StartCoroutine(SmoothMovement((bool end) => { count--;  }, targetPos,inverseTransitionTime));
		StartCoroutine(SmoothRotation((bool end) => { count--;  }, targetRot, 100));
        while(count > 0) {
            yield return null;
        }
		done(true);
	}

    // Animation Events

    void StopPushingEnd(){
    	pushing = false;
    }

    void StartPushingEnd(){
    	PushableObjectPad pad = pushableObject.GetComponent<PushableObjectPad>();
    	Vector3 endPos = rb.position+pad.endPosOffset;
    	float inverseMoveTime = pushingSpeed/pad.endPosOffset.magnitude;
    	pushableObject.transform.parent.GetComponent<PushableObject>().Push(pad.endPosOffset,inverseMoveTime);
    	StartCoroutine(SmoothMovement((bool done) => {
    			anim.SetTrigger("stopPushing");
    	},endPos,inverseMoveTime));
    }

    // Triggers

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "PushableObjectPad")
        {
            //Push
	        if(!pushing && isOnGround && pushButton){
	        	rb.velocity = Vector3.zero;
                pushing = true;
	        	pushableObject = other.gameObject;
                PushableObjectPad pad = pushableObject.GetComponent<PushableObjectPad>();
	        	Vector3 targetPos = pad.transform.position + pad.relStartPos;
	        	Quaternion targetRot = Quaternion.Euler(transform.eulerAngles.x, pad.yRot, transform.eulerAngles.z);
	        	StartCoroutine(ClampToSpot((bool end) => {
	        		anim.SetTrigger("startPushing");
	        	},targetPos,targetRot,1));
	        }
        }
    }

    void Animate()
    {
        Vector3 velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        float movementSpeed = velocity.magnitude;
        float jumpingSpeed = Mathf.Abs(Mathf.Clamp(1.0f/rb.velocity.y, -1f, 1f));

        anim.SetFloat("horizontal", Input.GetAxis("Horizontal"));
        anim.SetFloat("vertical", Input.GetAxis("Vertical"));

        anim.SetFloat("movementSpeed", movementSpeed);
        anim.SetBool("isJumping", !isOnGround);
        anim.SetFloat("jumpingSpeed", jumpingSpeed);
        anim.SetFloat("pushingSpeed", pushingSpeed);
    }

    void StopPlayer()
    {
        Vector3 stopVelocity = Vector3.zero;
        stopVelocity.y = rb.velocity.y;
        rb.velocity = stopVelocity;
    }
}
