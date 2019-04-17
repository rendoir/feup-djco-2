using UnityEngine;

public class Mirrors : MonoBehaviour, InteractionObserver
{
    public InteractionTrigger interactionTrigger;


    void Start() {
        interactionTrigger.SetObserver(this);
    }

    public void OnPlayerInteract() {
        Debug.Log("Player Interacted");
    }
}
