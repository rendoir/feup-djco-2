using UnityEngine;

public class ActivateArtifactState : State {

    private bool enteredTrigger;
    public float activationDuration = 4f;
    public float startTime = -1;
    private GameObject artifactActivator;
    private BoxCollider artifactActivatorTrigger;

    public ActivateArtifactState() {
        enteredTrigger = false;
        artifactActivator = GameManager.GetArtifactActivator();
        artifactActivatorTrigger = artifactActivator.GetComponentInChildren<BoxCollider>();
        artifactActivatorTrigger.enabled = true;
    }

    public override void Update() {
        if(enteredTrigger) {
            if(Time.time - startTime >= activationDuration) {
                GameState.Next();
                return;
            }
        }
    }

    public void OnActivatorTrigger() {
        artifactActivatorTrigger.enabled = false;
        enteredTrigger = true;
        GameInput.CaptureInput(true);
        startTime = Time.time;
    }

    public override State Next() {
        if(!enteredTrigger) {
            OnActivatorTrigger();
            return this;
        } else {
            GiveArtifactToPlayer();
            GameInput.CaptureInput(false);
            return new SaveFriendState();
        }
    }

    public override string GetMessage() {
        if(!enteredTrigger) 
            return "Find the monument to activate the artifact";
        else return "Activate the artifact";
    }

    private void GiveArtifactToPlayer() {
        GameManager.GetArtifact().SetActive(true);
    }
}
