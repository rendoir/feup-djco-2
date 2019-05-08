using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject : MonoBehaviour
{
	protected Rigidbody rb;

	protected virtual void Start ()
    {
        rb = GetComponent<Rigidbody>();
    }

	protected IEnumerator SmoothMovement (System.Action<bool> done, Vector3 end, float inverseMoveTime)
	{		
		return MovementUtils.SmoothMovement(done,gameObject,end,inverseMoveTime);
	}

	protected IEnumerator SmoothRotation (System.Action<bool> done, Quaternion end, float inverseRotationTime){
		return MovementUtils.SmoothRotation(done,gameObject,end,inverseRotationTime);
	}
}
