using UnityEngine;

public class AppleOfEden : MonoBehaviour, InteractionObserver
{
    public InteractionTrigger interactionTrigger;


    void Start() {
        interactionTrigger.SetObserver(this);
    }

    public void OnPlayerInteract() {
        Debug.Log("Player Interacted");
    }
}
