using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrap : MonoBehaviour
{

	public float startDelay = 0;
	public float interval = 5f;
	public float upTime = 1f;
	public float speed = 5f;
	public Vector3 offset;
	private GameObject spikes;
	private bool active = false;
	private bool down = true;
	private bool ready = true;
	private Vector3 upPos;
	private Vector3 downPos;

    // Audio
    private AudioSource spikeRisingAudio;

    // Start is called before the first frame update
    void Start()
    {
	    spikes = transform.GetChild(0).gameObject;
	    upPos = spikes.transform.position+offset;
	    downPos = spikes.transform.position;

        spikeRisingAudio = GetComponent<AudioSource>();
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

    	if(ready && down){
    		ready = false;
    		down = false;
    		
    		//Activate
    		StartCoroutine(MovementUtils.SmoothMovement( (bool done) => {
	            StartCoroutine(Deactivate()); 
        	}, spikes, upPos, speed));
            spikeRisingAudio.Play();
		
    		StartCoroutine(WaitCoolDown());
		}
    }

    IEnumerator Deactivate(){
    	yield return new WaitForSeconds(upTime);
    	StartCoroutine(MovementUtils.SmoothMovement( (bool done) => { down = true; }, spikes, downPos, speed));
    }

    IEnumerator WaitCoolDown(){
    	yield return new WaitForSeconds(interval);
		ready = true;		
    }
}
