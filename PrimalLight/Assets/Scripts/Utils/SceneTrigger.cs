using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTrigger : MonoBehaviour
{
    public LayerMask playerLayer;
    public int sceneIndex;
    public bool overrideTransform = false;
    public Vector3 initialScenePosition;
    public Vector3 initialSceneRotation;

    private bool loadingScene = false;

    void OnTriggerEnter(Collider other)
    {
        if(Utils.MaskContainsLayer(playerLayer, other.gameObject.layer) && !loadingScene) {
            SceneManager.sceneLoaded += OnSceneLoaded;
            loadingScene = true;
            SceneManager.LoadSceneAsync(sceneIndex);
        }
    }

	void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(loadingScene) {
            loadingScene = false;

            if(overrideTransform) {
                GameManager.GetPlayer().transform.position = initialScenePosition;
                GameManager.GetPlayer().transform.rotation = Quaternion.Euler(initialSceneRotation);
            }

            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    } 
}
