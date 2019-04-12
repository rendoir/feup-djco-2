using UnityEngine;
using System.Collections.Generic;

public class InteractionTrigger : MonoBehaviour
{
    public LayerMask playerLayer;
    private InteractionObserver observer;

    private bool isPlayerInside;

    void Start()
    {
        isPlayerInside = false;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E) && isPlayerInside)
            observer.OnPlayerInteract();
    }

    public void SetObserver(InteractionObserver obs) {
        observer = obs;
    }

    void OnTriggerEnter(Collider other)
    {
        if(Utils.MaskContainsLayer(playerLayer, other.gameObject.layer))
            isPlayerInside = true;
    }

    void OnTriggerExit(Collider other)
    {
        if(Utils.MaskContainsLayer(playerLayer, other.gameObject.layer))
            isPlayerInside = false;
    }
}
