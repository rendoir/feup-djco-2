using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    Enemy enemy;
    Transform target;
    NavMeshAgent agent;
    Animator anim;
    public float lookRadius = 10f;

    void Start() {
        enemy = GetComponent<Enemy>();
        anim = GetComponent<Animator>();
        target = GameManager.GetPlayer().transform;
        agent = GetComponent<NavMeshAgent>();
    }

    void FixedUpdate()
    {
        if(!enemy.isDead)
            Move();
        else Stop();
    }

    void Move() {
        float distance = Vector3.Distance(target.position, transform.position);

        if(distance <= lookRadius) {
            enemy.inConflict = true;
            agent.isStopped = false;
            agent.SetDestination(target.position);
            anim.SetBool("isWalking", true);

            if(distance <= agent.stoppingDistance) {
                Stop();
                FaceTarget();
            }
        } else {
            Stop();
        }
    }

    void Stop() {
        agent.isStopped = true;
        enemy.inConflict = false;
        anim.SetBool("isWalking", false);
    }

    void FaceTarget() {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10f);
    }

    void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }
}
