using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempleDualStatueDoor : ActionObject
{
	public Vector3 offset;
	public float movementSpeed = 5.0f;
	private Vector3 initPos;
	private GameObject gem;
	[HideInInspector] public bool locked = true;

    // Start is called before the first frame update
    void Start()
    {
    	initPos = transform.position;
    	gem = transform.GetChild(0).gameObject;
    }
    
    public override void Action(){
    	locked = false;
    	// Light up gem
    	transform.parent.GetComponent<TempleDualStatue>().UnlockedDoor();
    }

    public void Open(){
    	Vector3 targetPos = initPos+offset;
    	StartCoroutine(MovementUtils.SmoothMovement((bool end) => {},gameObject,targetPos,movementSpeed));
    }

    public override void ExitAction(){}
}
