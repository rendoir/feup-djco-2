using UnityEngine;

public class Health : MonoBehaviour, DeathObserver
{
    [Header("Health")]
    public float hitCooldown = 2f;
    public float regenerationRate = 0.1f;
    public float initialHealth = 20f;

    [Header("Components")]
    public Animator playerAnimator;
    public Renderer playerRenderer;

    private bool isDead;
    private float health;
    private float lastHitTime;

    void Start()
    {
        health = initialHealth;
        isDead = false;
        GameManager.RegisterDeathObserver(this);
        OnHealthChange();
    }

    void Update() {
        Regenerate();
    }

    private void Regenerate() {
        if(Time.time - lastHitTime > hitCooldown && !isDead) {
            health += Time.deltaTime * regenerationRate; //Regenerate
            health = Mathf.Min(health, initialHealth); //Clamp health
        }
    } 

    private void OnHealthChange() {
        //Check if player died
        if (health < 0) {
            if (!isDead)
                GameManager.PlayerDied();

            health = 0;
        }

        //Update color intensity
        playerRenderer.material.SetColor("_EmissionColor", Color.white * (health / initialHealth));

        //Restart hit cooldown
        lastHitTime = Time.time;
    }

    public void DamageOvertime(float healthLoss)
    {
        health -= Time.deltaTime * healthLoss;
        OnHealthChange();
    }

    public void Damage(float damage)
    {
        health -= damage;
        OnHealthChange();
    }

    public void ResetHealth() 
    {
        health = initialHealth;
    }

    public void OnPlayerDeath()
    {
        isDead = true;
        playerAnimator.SetTrigger("isDead");
    }

    public void OnPlayerAlive() {
        ResetHealth();
		isDead = false;
        playerAnimator.SetTrigger("isAlive");
	}
}
