using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatueDoor : ActionObject
{
	public Vector3 offset;
	public IEnumerator movement;
	public float movementSpeed = 5.0f;
	private Vector3 initPos;
	private GameObject gem;

    // Start is called before the first frame update
    void Start()
    {
    	initPos = transform.position;
    	gem = transform.GetChild(0).gameObject;
    }
    
    public override void Action(){
    	Vector3 targetPos = initPos+offset;
		if(movement != null)
			StopCoroutine(movement);
		movement = MovementUtils.SmoothMovement((bool end) => {
	        			movement = null;
	       			},gameObject,targetPos,movementSpeed);
		StartCoroutine(movement);
    }

    public override void ExitAction(){
    	if(movement != null)
			StopCoroutine(movement);
		movement = MovementUtils.SmoothMovement((bool end) => {
	        			movement = null;
	       			},gameObject,initPos,movementSpeed);
		StartCoroutine(movement);
    }
}
