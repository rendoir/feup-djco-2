using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushableObject : MovingObject
{
    public float speedMult;
    public bool bounded;
    public Vector3 relMinBound;
    public Vector3 relMaxBound;
	public GameObject target;
	private Vector3 initPos;

	protected override void Start(){
		base.Start();
		initPos = rb.position;
	}

    public void Push(Vector3 endPosOffset, float inverseMoveTime){
    	Vector3 endPos = rb.position+endPosOffset;
    	StartCoroutine(SmoothMovement( (bool done) => {}, endPos, inverseMoveTime));
    }
    
    private bool CheckInBounds(Vector3 relPos){
    	bool minBound = false;
    	bool maxBound = false;

    	if(relPos.x <= relMaxBound.x && relPos.y <= relMaxBound.y && relPos.z <= relMaxBound.z)
    		maxBound = true;

    	if(relPos.x >= relMinBound.x && relPos.y >= relMinBound.y && relPos.z >= relMinBound.z)
    		minBound = true;

    	return minBound && maxBound;
    }

    public bool CanPush(Vector3 endPosOffset){
    	Vector3 endPos = rb.position+endPosOffset;
    	return bounded ? CheckInBounds(endPos-initPos) : true;
    }
}
