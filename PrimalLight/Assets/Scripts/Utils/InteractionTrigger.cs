using UnityEngine;
using System.Collections.Generic;

public class InteractionTrigger : MonoBehaviour
{
    public LayerMask playerLayer;
    public GameObject tooltip;
    private InteractionObserver observer;

    private bool isPlayerInside;
    private bool isPlayerInteracting;

    void Start()
    {
        isPlayerInside = false;
        isPlayerInteracting = false;
    }

    void FixedUpdate()
    {
        if(GameInput.interactPressed && isPlayerInside) {
            isPlayerInteracting = !isPlayerInteracting;
            observer.OnPlayerInteract();
            tooltip.SetActive(!isPlayerInteracting && isPlayerInside);
        }
    }

    public void SetObserver(InteractionObserver obs) {
        observer = obs;
    }

    void OnTriggerEnter(Collider other)
    {
        if(Utils.MaskContainsLayer(playerLayer, other.gameObject.layer)) {
            isPlayerInside = true;
            tooltip.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(Utils.MaskContainsLayer(playerLayer, other.gameObject.layer)) {
            isPlayerInteracting = false;
            isPlayerInside = false;
            tooltip.SetActive(false);
        }
    }
}
