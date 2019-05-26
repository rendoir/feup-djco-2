using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MovingObject, DeathObserver
{
	[Header("Movement")]
	public float movementSpeed = 3f;
	public float pushingSpeed = 2.5f;
	public float jumpForce = 5f;
	public float sprintMultiplier = 2f;
	public float airMovementMultiplier = 0.5f;

	[Header("Physics")]
	public float groundDistance = 0.1f;
	public LayerMask groundLayer;
	public CapsuleCollider playerColllider;

	public bool isOnGround;
	public bool pushing = false;
	public float sprint;
	private Vector3 input;

	private bool interactButton;

	private Animator anim;
	private GameObject pushableObject = null;

	private bool isDead = false;
	public float jumpCooldown = 0.1f;
	private float jumpCooldownCounter;

	protected override void Start()
	{
		GameManager.RegisterDeathObserver(this);
		anim = GetComponent<Animator>();
		jumpCooldownCounter = 0f;

		base.Start();
	}

	void FixedUpdate()
	{
		if(isDead) {
			StopPlayer();
			return;
		}

		if( GameManager.IsInputCaptured() ) {
			PhysicsCheck();
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
		isOnGround = Physics.CheckCapsule(playerColllider.bounds.center,
			new Vector3(playerColllider.bounds.center.x, playerColllider.bounds.min.y - groundDistance, playerColllider.bounds.center.z),
			playerColllider.radius,
			groundLayer.value,
			QueryTriggerInteraction.Ignore);
	}

	void Move()
	{
		//Input
		input = new Vector3(GameInput.horizontal, 0.0f, GameInput.vertical);
		bool jumpPressed = GameInput.jumpPressed;
		interactButton = GameInput.interactPressed;
		sprint = GameInput.sprint;

		if(pushing)
			return;

		// the movement direction is the way the came is facing
		Vector3 Direction = Camera.main.transform.forward * input.z +
							Vector3.Cross(Camera.main.transform.forward, Vector3.up) *  -input.x;

		//rotates the player to face in the camera direction if he is moving
		if(Math.Abs(input.x) > 0.0f || Math.Abs(input.z) > 0.0f)
		{
			transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, Camera.main.transform.localEulerAngles.y, transform.localEulerAngles.z);
		}

		//Walk
		float finalSpeed = movementSpeed;
		finalSpeed += finalSpeed*sprint*(sprintMultiplier-1); //Increase speed while sprinting
		if(!isOnGround) 
			finalSpeed *= airMovementMultiplier; //Limit walk control in the air
		
		Vector3 velocity = Direction.normalized * finalSpeed;
		velocity = Vector3.ClampMagnitude(velocity, finalSpeed); //Clamp velocity
		velocity.y = rb.velocity.y;

		rb.velocity = velocity;

		//Jump
		jumpCooldownCounter += Time.fixedDeltaTime;
		if(jumpPressed && isOnGround && jumpCooldownCounter >= jumpCooldown) {
			jumpCooldownCounter = 0f;
			rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
		}
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
			if(!pushing && isOnGround && interactButton){
				PushableObjectPad pad = other.gameObject.GetComponent<PushableObjectPad>();
				if(!pad.CanPush())
					return;
				rb.velocity = Vector3.zero;
				pushing = true;
				pushableObject = other.gameObject;
				Vector3 targetPos = pad.transform.position + pad.relStartPos;
				Quaternion targetRot = Quaternion.Euler(transform.eulerAngles.x, pad.yRot, transform.eulerAngles.z);
				StartCoroutine(ClampToSpot((bool end) => {
					anim.SetTrigger("startPushing");
				},targetPos,targetRot,1));
			}
		}
		else if (other.tag == "Pillar"){
			if(isOnGround && interactButton){
				PushableObjectPad pad = other.gameObject.GetComponent<PushableObjectPad>();
				Pillar pillar = other.gameObject.transform.parent.GetComponent<Pillar>();
				pillar.Move(pad.direction);
			}
		}

	}

	void Animate()
	{
		Vector3 velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
		float magnitude = velocity.magnitude;
		float jumpingSpeed = Mathf.Abs(Mathf.Clamp(1.0f/rb.velocity.y, -1f, 1f));

		Vector3 blend = input / sprintMultiplier + input * sprint / sprintMultiplier;

		anim.SetFloat("horizontal", !GameManager.IsInputCaptured() ? blend.x : 0f);
		anim.SetFloat("vertical", !GameManager.IsInputCaptured() ? blend.z : 0f);

		//anim.SetFloat("movementSpeed", magnitude > Mathf.Epsilon ? magnitude : 1f);
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

	public void OnPlayerDeath() {
		isDead = true;
		StopPlayer();
	}

	public void OnPlayerAlive() {
		isDead = false;
	}
}
