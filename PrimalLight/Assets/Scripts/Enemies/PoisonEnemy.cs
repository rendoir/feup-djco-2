using UnityEngine;

public class PoisonEnemy : EnemyAttack
{
    public float cooldown = 2f;
    private float cooldownCounter = 0f;
    public float forwardBallOffset;
    public float upBallOffset;
    public GameObject ballPrefab;

    protected override void UpdateAttack() {
        cooldownCounter += Time.deltaTime;
        if(cooldownCounter > cooldown) {
            cooldownCounter = 0f;
            GameObject obj = Instantiate(ballPrefab, transform.position + transform.forward * forwardBallOffset + transform.up * upBallOffset, transform.rotation);
            obj.GetComponent<Rigidbody>().AddForce(transform.forward * 5f + transform.up * 5f, ForceMode.Impulse);
        }
    }
}
