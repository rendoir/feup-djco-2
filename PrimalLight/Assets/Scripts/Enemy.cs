using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Health")]
    private float health;
    public float initialHealth = 20f;
    public float healthLoss = 5f;

    [Header("Effect")]
    public Renderer meshRenderer;
    public AnimationControl animationControl;
    private Material material;

    private bool isHit;
    private bool isDead;

    void Start()
    {
        health = initialHealth;
        isHit = false;
        isDead = false;
        material = meshRenderer.material;
    }

    void FixedUpdate()
    {
        UpdateHitEffect();

        isHit = false;
    }

    public void OnHit()
    {
        isHit = true;
        health -= Time.deltaTime * healthLoss;

        //Enemy died
        if (health < 0)
        {
            if (!isDead)
            {
                OnDeath();
            }

            health = 0;
        }
    }

    void UpdateHitEffect()
    {
        if (!isDead && isHit) {
            float emission = Mathf.PingPong(Time.time * 2f, 0.2f) + 0.2f;
            Color baseColor = Color.red;
            Color finalColor = baseColor * emission;
            finalColor.a = 1;
            material.SetColor("_EmissionColor", finalColor);

        }
        else {
            material.SetColor("_EmissionColor", Color.black);
        }
    }

    void OnDeath()
    {
        isDead = true;
        animationControl.SetAnimation("isDead" + Random.Range(1, 4));
    }
}
