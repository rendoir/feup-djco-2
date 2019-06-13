using UnityEngine;

public class SoundTrigger : MonoBehaviour
{
    public LayerMask playerLayer;
    public string clipName;

    void OnTriggerEnter(Collider other)
    {
        if(Utils.MaskContainsLayer(playerLayer, other.gameObject.layer)) {
            StartCoroutine(GameSound.FadeIn(clipName));
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(Utils.MaskContainsLayer(playerLayer, other.gameObject.layer)) {
            StartCoroutine(GameSound.FadeOut(clipName));
        }
    }
}
