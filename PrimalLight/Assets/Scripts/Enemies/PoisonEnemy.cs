using UnityEngine;

public class PoisonEnemy : MonoBehaviour
{
    public GameObject cloudPrefab;

    void Start()
    {
        GameObject obj = Instantiate(cloudPrefab, transform.position + transform.forward * 8f, Quaternion.Euler(270f,0f,0f));
    }

    void FixedUpdate()
    {

    }
}
