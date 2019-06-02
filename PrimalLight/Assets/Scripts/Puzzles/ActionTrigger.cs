using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionTrigger : MonoBehaviour
{
    public GameObject target;
    public bool triggerOnce = false;

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player" || other.tag == "PushableObject") {
            target.GetComponent<ActionObject>().Action();
            if(triggerOnce)
                gameObject.SetActive(false);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player" || other.tag == "PushableObject") {
            target.GetComponent<ActionObject>().ExitAction();
        }
    }
}
