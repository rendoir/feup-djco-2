using UnityEngine;

public class PoisonBall : MonoBehaviour
{
    public GameObject cloudPrefab;

    void OnCollisionEnter(Collision collision) {
        GameObject obj = Instantiate(cloudPrefab, transform.position, Quaternion.Euler(270f,0f,0f));
        Destroy(gameObject);
    }
}
