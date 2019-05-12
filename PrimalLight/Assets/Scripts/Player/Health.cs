using UnityEngine;

public class Health : MonoBehaviour, DeathObserver
{
    [Header("Health")]
    private float health;
    public float initialHealth = 20f;

    private bool isDead;
    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
        health = initialHealth;
        isDead = false;
        GameManager.RegisterDeathObserver(this);
    }

    public void OnHit(float healthLoss)
    {
        health -= Time.deltaTime * healthLoss;

        //Enemy died
        if (health < 0) {
            if (!isDead)
                GameManager.PlayerDied();

            health = 0;
        }
    }

    public void OnPlayerDeath()
    {
        isDead = true;
        anim.SetTrigger("isDead");
    }
}
