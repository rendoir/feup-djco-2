using UnityEngine;
using System.Collections.Generic;

public class InteractionTrigger : MonoBehaviour
{
    public LayerMask playerLayer;
    public GameObject tooltip;
    private InteractionObserver observer;

    private bool isPlayerInside;

    void Start()
    {
        isPlayerInside = false;
    }

    void FixedUpdate()
    {
        if(Input.GetKeyDown(KeyCode.E) && isPlayerInside) {
            observer.OnPlayerInteract();
            tooltip.SetActive(false);
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
            isPlayerInside = false;
            tooltip.SetActive(false);
        }
    }
}
