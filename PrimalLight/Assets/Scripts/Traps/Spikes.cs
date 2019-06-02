using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
	public float damage = 5f;

    void OnTriggerEnter(Collider other) {
        if(other.tag == "Player") {
            Health playerHealth = other.gameObject.GetComponent<Health>();
            playerHealth.Damage(damage);
        }
    }

    void OnTriggerStay(Collider other) {
        if(other.tag == "Player") {
            Health playerHealth = other.gameObject.GetComponent<Health>();
            playerHealth.Damage(damage);
        }
    }
}
