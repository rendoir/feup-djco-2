using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable] public class PlatformMovement {
	public Vector3 offset;
	public float speed = 5f;
}

public class MovingPlatform : ActionObject
{
	public float startDelay = 0;
	public PlatformMovement [] movements;
    public bool loop = false;
    public bool trigger = true;

    private int currMovement = 0;
    private bool move = false;
    private Vector3 initPos;
    private Rigidbody rb;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        initPos = transform.position;

        if(!trigger)
        	StartCoroutine(Init());
    }

    IEnumerator Init(){
    	yield return new WaitForSeconds(startDelay);
    	Action();
    }

    void Update(){
    	if(!move)
    		return;

    	if(currMovement >= movements.Length){
        	if(!loop){
        		move = false;
        		return;
        	}
        	else{
        		transform.position = initPos;
        		currMovement = 0;
        	}
        }

        Move();
        currMovement++;
    }

    public override void Action(){
    	move = true;
  	}

  	public void Move(){
  		move = false;
  		PlatformMovement movement = movements[currMovement];
    	Vector3 endPos = transform.position+movement.offset;
        
    	StartCoroutine(MovementUtils.SmoothMovement( (bool done) => {
            move = true;
        }, gameObject, endPos, movement.speed));
  	}

    public override void ExitAction(){}
}
