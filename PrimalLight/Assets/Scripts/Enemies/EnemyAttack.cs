using UnityEngine;

public abstract class EnemyAttack : MonoBehaviour
{
    public Enemy enemy;

    void Start() {
        enemy = GetComponent<Enemy>();
    }

    void FixedUpdate()
    {
        if(!enemy.isDead && enemy.inConflict) {
            UpdateAttack();
        }
    }

    protected abstract void UpdateAttack();
}
