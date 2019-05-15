using UnityEngine;

public class PoisonEnemy : EnemyAttack
{
    public float cooldown = 2f;
    private float cooldownCounter = 0f;
    public float forwardBallOffset;
    public float upBallOffset;
    public float force = 0.5f;
    public float randomDirectionAngle = 5f;
    public GameObject ballPrefab;

    protected override void UpdateAttack() {
        cooldownCounter += Time.deltaTime;
        if(cooldownCounter > cooldown) {
            cooldownCounter = 0f;
            GameObject obj = Instantiate(ballPrefab, transform.position + transform.forward * forwardBallOffset + transform.up * upBallOffset, transform.rotation);
            obj.GetComponent<Rigidbody>().AddForce(Quaternion.AngleAxis(Random.Range(-randomDirectionAngle, randomDirectionAngle), Vector3.up) * transform.forward * force, ForceMode.Impulse);
        }
    }
}
