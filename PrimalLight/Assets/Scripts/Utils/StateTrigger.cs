using UnityEngine;
using System.Collections.Generic;

public class StateTrigger : MonoBehaviour
{
    public LayerMask playerLayer;

    void OnTriggerEnter(Collider other)
    {
        if(Utils.MaskContainsLayer(playerLayer, other.gameObject.layer)) {
            GameState.Next();
            Destroy(gameObject);
        }
    }
}
