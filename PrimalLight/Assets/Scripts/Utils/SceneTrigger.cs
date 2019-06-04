using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTrigger : MonoBehaviour
{
    public LayerMask playerLayer;
    public int sceneIndex;
    //public Vector3 initialScenePosition;
    //public Vector3 initialSceneRotation;

    void OnTriggerEnter(Collider other)
    {
        if(Utils.MaskContainsLayer(playerLayer, other.gameObject.layer)) {
            SceneManager.LoadSceneAsync(sceneIndex);
        }
    }
}
