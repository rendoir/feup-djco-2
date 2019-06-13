using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempleStatueDoor : ActionObject
{
	public Vector3 offset;
	public float movementSpeed = 5.0f;
	
	private Vector3 initPos;
	private IEnumerator movement;
	private GameObject gem;
	private Renderer gemRend;
	private Shader initGemShader;
	private Shader glowGemShader;

    // Start is called before the first frame update
    void Start()
    {
    	initPos = transform.position;
    	gem = transform.GetChild(0).gameObject;
    	gemRend = gem.GetComponent<Renderer>();
    	initGemShader = gemRend.material.shader;
    	glowGemShader = Shader.Find("MK/Glow/Selective/Standard");
    }
    
    public override void Action(){
    	Vector3 targetPos = initPos+offset;

    	//Make gem glow
    	gemRend.material.shader = glowGemShader;
		if(movement != null)
			StopCoroutine(movement);
		movement = MovementUtils.SmoothMovement((bool end) => {
			movement = null;
		},gameObject,targetPos,movementSpeed);
		StartCoroutine(movement);
    }

    public override void ExitAction(){
    	if(movement != null)
			StopCoroutine(movement);
		movement = MovementUtils.SmoothMovement((bool end) => {
			//Reset gem shader
			gemRend.material.shader = initGemShader;
			movement = null;
		},gameObject,initPos,movementSpeed);
		StartCoroutine(movement);
    }

}
