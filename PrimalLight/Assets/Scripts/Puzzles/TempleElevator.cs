using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempleElevator : ActionObject
{
    
    public float setHeightOffset = 10f;
    public float topHeightOffset = 20f;
    public float speed = 10f;

    private Vector3 setPosition;
    private Vector3 topPosition;
    private GameObject platform;
	private GameObject trigger;
	private Coroutine movement = null;

    // Start is called before the first frame update
    void Start()
    {
    	setPosition = transform.position + new Vector3(0,setHeightOffset,0);;
    	topPosition = transform.position + new Vector3(0,topHeightOffset,0);
    	platform = transform.GetChild(0).gameObject;
        trigger = platform.transform.GetChild(2).GetChild(0).gameObject;
    }

    public void Set(){
    	StartCoroutine(MovementUtils.SmoothMovement( (bool done) => { 
            trigger.SetActive(true);
        }, gameObject, setPosition, speed));
    }

    public override void Action(){
    	if(movement != null)
    		StopCoroutine(movement);

    	movement = StartCoroutine(MovementUtils.SmoothMovement( (bool done) => { 
            movement = null;
        }, platform, topPosition, speed));
    }

    public override void ExitAction(){
    	if(movement != null)
    		StopCoroutine(movement);

    	movement =  StartCoroutine(MovementUtils.SmoothMovement( (bool done) => { 
            movement = null;
        }, platform, setPosition, speed));
    }
}
