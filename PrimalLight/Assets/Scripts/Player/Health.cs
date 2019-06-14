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
        OnDamage();
    }

    void Update() {
        Regenerate();
    }

    private void Regenerate() {
        if(Time.time - lastHitTime > hitCooldown && !isDead) {
            health += Time.deltaTime * regenerationRate; //Regenerate
            health = Mathf.Min(health, initialHealth); //Clamp health
            UpdateColor();
        }
    } 

    private void OnDamage() {
        //Check if player died
        if (health < 0) {
            if (!isDead)
                GameManager.PlayerDied();

            health = 0;
        }

        UpdateColor();

        //Restart hit cooldown
        lastHitTime = Time.time;
    }

    private void UpdateColor() {
        playerRenderer.material.SetColor("_EmissionColor", Color.white * (health / initialHealth));
    }

    public void DamageOvertime(float healthLoss)
    {
        health -= Time.deltaTime * healthLoss;
        OnDamage();
    }

    public void Damage(float damage)
    {
        health -= damage;
        OnDamage();
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
