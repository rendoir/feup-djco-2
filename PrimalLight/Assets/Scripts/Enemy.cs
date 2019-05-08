using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Health")]
    private float health;
    public float initialHealth = 20f;
    public float healthLoss = 5f;

    [Header("Effect")]
    public Renderer meshRenderer;
    private Material material;

    private bool isHit;

    void Start()
    {
        health = initialHealth;
        isHit = false;
        material = meshRenderer.material;
    }

    void FixedUpdate()
    {
        if(isHit) {
            float emission = Mathf.PingPong(Time.time * 2f, 0.2f) + 0.2f;
            Color baseColor = Color.red;
            Color finalColor = baseColor * emission;
            finalColor.a = 1;
            material.SetColor ("_EmissionColor", finalColor);
        } else {
            material.SetColor ("_EmissionColor", Color.black);
        }

        isHit = false;
    }

    public void OnHit() {
        isHit = true;
        health -= Time.deltaTime * healthLoss;
        
        //Enemy died
        if(health < 0) {
            health = 0;
            gameObject.SetActive(false);
        }
    } 
}
