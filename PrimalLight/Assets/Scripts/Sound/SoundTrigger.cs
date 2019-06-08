using UnityEngine;
using System.Collections.Generic;

public class SoundTrigger : MonoBehaviour
{
    public LayerMask playerLayer;
    public string clipName;

    void OnTriggerEnter(Collider other)
    {
        if(Utils.MaskContainsLayer(playerLayer, other.gameObject.layer)) {
            GameSound.Play(clipName);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(Utils.MaskContainsLayer(playerLayer, other.gameObject.layer)) {
            GameSound.Stop(clipName);
        }
    }
}
