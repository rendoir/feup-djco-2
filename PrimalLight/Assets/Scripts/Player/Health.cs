using UnityEngine;

public class Health : MonoBehaviour
{
    [Header("Health")]
    private float health;
    public float initialHealth = 20f;

    private bool isDead;

    void Start()
    {
        health = initialHealth;
        isDead = false;
    }

    void FixedUpdate()
    {

    }

    public void OnHit(float healthLoss)
    {
        health -= Time.deltaTime * healthLoss;

        //Enemy died
        if (health < 0) {
            if (!isDead)
                OnDeath();

            health = 0;
        }
    }

    void OnDeath()
    {
        isDead = true;
    }
}
