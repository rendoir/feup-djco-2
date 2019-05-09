using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    public GameObject target;

    void Start()
    {
    	
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player" || other.tag == "PushableObject") {
            target.GetComponent<Door>().Open();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player" || other.tag == "PushableObject") {
            target.GetComponent<Door>().Close();
        }
    }
}
