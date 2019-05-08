using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementUtils 
{
 	public static IEnumerator SmoothMovement (System.Action<bool> done, GameObject obj, Vector3 end, float inverseMoveTime)
	{
		float sqrRemainingDistance = (obj.transform.position - end).sqrMagnitude;
		
		while(sqrRemainingDistance > 0.01f)
		{
			Vector3 newPosition = Vector3.MoveTowards(obj.transform.position, end, inverseMoveTime * Time.deltaTime);
			
            obj.transform.position = newPosition;
			
			sqrRemainingDistance = (obj.transform.position - end).sqrMagnitude;
			
			yield return null;
		}

		done(true);
	}

	public static IEnumerator SmoothRotation (System.Action<bool> done, GameObject obj, Quaternion end, float inverseRotationTime){
		while(obj.transform.rotation != end)
		{
			obj.transform.rotation = Quaternion.RotateTowards(obj.transform.rotation, end, inverseRotationTime * Time.deltaTime);
			yield return null;
		}

		done(true);
	}
	   
}
