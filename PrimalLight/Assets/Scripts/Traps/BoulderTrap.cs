using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoulderTrap : ActionObject
{
	public float speed = 5f;
	public float rotSpeed = 100f;
	public Vector3 movement;
    public float bumpHeight = 1f;
    public float bumpAscendingSpeedMult = 5f;
    public float bumpDescendingSpeedMult = 1f;
	
    private bool active = false;

    private Coroutine bump = null;
    private bool ascending = true;
    private float height = 0;
    private GameObject boulder;
    private GameObject dust;

    // Start is called before the first frame update
    void Start()
    {
        boulder = transform.GetChild(0).gameObject;
        dust = transform.GetChild(1).gameObject;
    }

    public override void Action(){
  		SetActive(true);
    	Vector3 endPos = transform.position+movement;
        Coroutine rotation = StartCoroutine(MovementUtils.SmoothRotationConst(boulder, Vector3.back, rotSpeed));

    	StartCoroutine(MovementUtils.SmoothMovement( (bool done) => { 
            StopCoroutine(rotation); 
            SetActive(false);
        }, gameObject, endPos, speed));
  	}

    public override void ExitAction(){}

    IEnumerator BumpRoutine() {
        while(true){
            Vector3 offset;
            if(ascending)
                offset = Vector3.up*Time.deltaTime*bumpAscendingSpeedMult;
            else
                offset = Vector3.down*Time.deltaTime*bumpDescendingSpeedMult;

            height += offset.y;
            transform.position += offset;
            if(height >= bumpHeight)
                ascending = false;
            else if(height <= 0){
            	dust.SetActive(true);
                yield break;
            }

            yield return null;
        }
    }

    public void Bump(){
    	if(bump == null){
    		dust.SetActive(false);
            bump = StartCoroutine(BumpRoutine());
    	}
    }	

    private void SetActive(bool state){
        active = state;
        boulder.GetComponent<Boulder>().SetColliderTrigger(state);
        dust.SetActive(state);
    }
}
