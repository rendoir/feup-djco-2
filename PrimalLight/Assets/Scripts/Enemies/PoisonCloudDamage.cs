using UnityEngine;

public class PoisonCloudDamage : MonoBehaviour
{
    public LayerMask playerMask;
    public float healthLoss = 1f;
    public ParticleSystem cloud;
    Health playerHealth = null;

    void FixedUpdate() {
        if(!cloud.IsAlive(true))
            Destroy(gameObject);
    }

    void OnTriggerStay(Collider other) {
        if(Utils.MaskContainsLayer(playerMask, other.gameObject.layer)) {
            
            if(playerHealth) {
                playerHealth.DamageOvertime(healthLoss);
            } else {
                playerHealth = other.gameObject.GetComponent<Health>();
                playerHealth.DamageOvertime(healthLoss);
            }

        }
    }

}
