using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraConfiner : MonoBehaviour
{

	public CinemachineConfiner cmConfiner;
	public GameObject confiner;

	public void SetConfiner(){
		if(confiner == null)
			cmConfiner.m_BoundingVolume = null;
		else
			cmConfiner.m_BoundingVolume = confiner.GetComponent<Collider>();
	}

	void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player") {
            SetConfiner();
        }
    }
}
