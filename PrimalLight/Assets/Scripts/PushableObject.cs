using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushableObject : MovingObject
{

    public void Push(Vector3 endPosOffset, float inverseMoveTime){
    	Vector3 endPos = rb.position+endPosOffset;
    	StartCoroutine(SmoothMovement( (bool done) => {}, endPos, inverseMoveTime));
    }
    
}
