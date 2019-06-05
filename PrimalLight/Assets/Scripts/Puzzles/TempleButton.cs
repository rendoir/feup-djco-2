using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempleButton : MonoBehaviour
{
    public GameObject target;
    public bool triggerOnce = false;
    public float offset;
    public Color activeColor;

    private Color initColor;
    private Vector3 initPos;
    private Vector3 activePos;

    private Renderer rend;
    private BoxCollider boxCollider;

    void Start(){
    	rend = GetComponent<Renderer>();
    	boxCollider = GetComponent<BoxCollider>();

    	initPos = transform.position;
    	activePos = initPos + new Vector3(0,offset,0);
    	initColor = rend.material.color;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player" || other.tag == "PushableObject") {
        	transform.position = activePos;
        	//rend.material.shader = Shader.Find("_Color");
        	//rend.material.SetColor("_Color", activeColor);
            target.GetComponent<ActionObject>().Action();
            if(triggerOnce)
                boxCollider.enabled = false;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player" || other.tag == "PushableObject") {
            transform.position = initPos;
            //rend.material.shader = Shader.Find("_Color");
        	//rend.material.SetColor("_Color", initColor);
            target.GetComponent<ActionObject>().ExitAction();
        }
    }
}