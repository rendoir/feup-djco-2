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
    public float flameLength = 1.8f;
    public float flameSpeed = 5f;
	public float flameTime = 1f;
	private bool ready = true;
	private bool active = false;

	private static float defaultRateOverTime = 100f;
	private static float defaultStartLifeTime = 1.8f;

	// Audio

	private AudioSource flameShotAudio;


    // Start is called before the first frame update
    void Start()
    {
    	foreach(Transform flameThrower in transform){
    		ParticleSystem ps = flameThrower.GetChild(0).GetComponent<ParticleSystem>();
    		var main = ps.main;
    		var em = ps.emission;

    		// Proportionally set emisson rate over time
        	em.rateOverTime = FlameTrap.defaultRateOverTime*flameTime/FlameTrap.defaultStartLifeTime;
        	main.duration = flameTime;
        	main.startSpeed = flameSpeed;
            main.startLifetime = flameLength;
    	}

    	flameShotAudio = GetComponent<AudioSource>();
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
        	flameShotAudio.Play();

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
