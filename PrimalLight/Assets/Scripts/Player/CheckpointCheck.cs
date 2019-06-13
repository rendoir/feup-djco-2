using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointCheck : MonoBehaviour
{
    void OnTriggerEnter(Collider other){
    	if(other.tag == "Checkpoint"){
    		other.gameObject.SetActive(false);
    		GameManager.current.SetCurrentCheckpoint(other.gameObject);
		}
	}
}
