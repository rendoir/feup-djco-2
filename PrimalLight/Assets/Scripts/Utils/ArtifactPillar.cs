using UnityEngine;

public class ArtifactPillar : InteractionObserver {
    private InteractionTrigger placePieceTrigger;
    private ActivateArtifactState state;
    private int id;

    public ArtifactPillar(GameObject pillar, int pieceID, ActivateArtifactState artifactState) {
        placePieceTrigger = pillar.GetComponentInChildren<InteractionTrigger>();
        placePieceTrigger.SetObserver(this);
        id = pieceID;
        state = artifactState;
    }

    public void OnPlayerInteract() {
        state.OnPlayerPlacePiece(id);
        placePieceTrigger.gameObject.SetActive(false);
    }
}
