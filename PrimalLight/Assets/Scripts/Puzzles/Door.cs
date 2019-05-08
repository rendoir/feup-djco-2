using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DoorPart {
	private Door door;
	public GameObject obj;
	public Vector3 initPos;
	public IEnumerator movement;

	public DoorPart(Door door, GameObject obj) {
		this.door = door;
		this.obj = obj;
		initPos = obj.GetComponent<Rigidbody>().position;
	}

	public void Open(Vector3 openPosOffset){
		Vector3 targetPos = initPos+openPosOffset;
		if(movement != null)
			door.StopCoroutine(movement);
		movement = MovementUtils.SmoothMovement((bool end) => {
	        			movement = null;
	       			},obj,targetPos,door.closingSpeed);
		door.StartCoroutine(movement);
	}

	public void Close(){
		if(movement != null)
			door.StopCoroutine(movement);
		movement = MovementUtils.SmoothMovement((bool end) => {
	        			movement = null;
	       			},obj,initPos,door.closingSpeed);
		door.StartCoroutine(movement);
	}
}

public class Door : MonoBehaviour
{
	private DoorPart leftDoor;
	private DoorPart rightDoor;
	public float closingSpeed = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
    	leftDoor = new DoorPart(this, transform.GetChild(0).gameObject);
    	rightDoor = new DoorPart(this, transform.GetChild(1).gameObject);
    }
    
    public void Open(){
    	leftDoor.Open(5*Vector3.left);
    	rightDoor.Open(5*Vector3.right);
    }

    public void Close(){
    	leftDoor.Close();
    	rightDoor.Close();
    }
}
