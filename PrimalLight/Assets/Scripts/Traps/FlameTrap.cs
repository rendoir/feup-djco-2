using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameTrap : MonoBehaviour
{

	enum FlameThrower // your custom enumeration
	{
		Top, 
		Middle, 
		Bottom
	};

	public float startDelay = 0;
	public int flame = 0;
	public float interval = 1f; 
    public float flameLength = 0.5f;
	public float flameTime = 1f;
	private bool ready = true;
	private bool active = false;


    // Start is called before the first frame update
    void Start()
    {
    	foreach(Transform flameThrower in transform){
    		ParticleSystem ps = flameThrower.GetChild(0).GetComponent<ParticleSystem>();
    		var main = ps.main;
        	main.duration = flameTime;
            main.startLifetime = flameLength;
    	}
    	StartCoroutine(Init());    
    }

    IEnumerator Init(){
    	yield return new WaitForSeconds(startDelay);
    	active = true;
    }

    // Update is called once per frame
    void Update()
    {
    	if(!active)
    		return;

        if(ready){
        	ready = false;

        	ParticleSystem ps = transform.GetChild(flame).GetChild(0).GetComponent<ParticleSystem>();
        	ps.Play();

        	flame++;
        	if(flame > 2)
        		flame = 0;
        	StartCoroutine(WaitInterval());
        }
    }

    IEnumerator WaitInterval(){
    	yield return new WaitForSeconds(interval);
    	ready = true;
    }
}
