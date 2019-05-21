using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushableObjectPad : MonoBehaviour
{

	public Vector3 relStartPos;
	public Vector3 endPosOffset;
	public float yRot = 0;
    public string direction;
	private PushableObject objectScript;

    // Start is called before the first frame update
    void Start()
    {
    	objectScript = transform.parent.gameObject.GetComponent<PushableObject>();
    }

    public bool CanPush(){
    	return objectScript.CanPush(endPosOffset);
    } 
}
