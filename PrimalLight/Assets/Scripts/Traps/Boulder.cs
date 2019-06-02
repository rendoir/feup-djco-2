using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boulder : MonoBehaviour
{
    public float damage = 50f;
    private MeshCollider meshCollider;

    void Start(){
        meshCollider = GetComponent<MeshCollider>();
    }

    void OnTriggerEnter(Collider other) {
        if(other.tag == "Player") {
            Health playerHealth = other.gameObject.GetComponent<Health>();
            playerHealth.Damage(damage);
            transform.parent.GetComponent<BoulderTrap>().Bump();
        }
    }

    public void SetColliderTrigger(bool trigger){
        meshCollider.isTrigger = trigger;
    }
}
