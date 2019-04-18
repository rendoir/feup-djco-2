using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject : MonoBehaviour
{
	protected Rigidbody rb;

	protected virtual void Start ()
    {
        rb = GetComponent<Rigidbody>();;
    }

	protected IEnumerator SmoothMovement (System.Action<bool> done, Vector3 end, float inverseMoveTime)
	{		
		float sqrRemainingDistance = (transform.position - end).sqrMagnitude;
		
		while(sqrRemainingDistance > 0.01f)
		{
			Vector3 newPosition = Vector3.MoveTowards(rb.position, end, inverseMoveTime * Time.deltaTime);
			
            rb.position = newPosition;
			
			sqrRemainingDistance = (rb.position - end).sqrMagnitude;
			
			yield return null;
		}

		done(true);
	}


	protected IEnumerator SmoothRotation (System.Action<bool> done, Quaternion end, float inverseRotationTime){
		while(transform.rotation != end)
		{
			transform.rotation = Quaternion.RotateTowards(transform.rotation, end, inverseRotationTime * Time.deltaTime);
			yield return null;
		}

		done(true);
	}
}
