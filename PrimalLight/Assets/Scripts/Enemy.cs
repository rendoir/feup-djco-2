using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Health")]
    private float health;
    public float initialHealth = 20f;
    public float healthLoss = 5f;

    void Start()
    {
        health = initialHealth;
    }

    void FixedUpdate()
    {
        Debug.Log(health); //TODO Remove
    }

    public void OnHit() {
        health -= Time.deltaTime * healthLoss;
        
        //Enemy died
        if(health < 0) {
            health = 0;
            gameObject.SetActive(false);
        }
    } 
}
