using UnityEngine;

public class ArtifactPillar : InteractionObserver {
    private InteractionTrigger placePieceTrigger;
    private ActivateArtifactState state;
    private Material material;
    private int id;

    public ArtifactPillar(GameObject pillar, int pieceID, ActivateArtifactState artifactState) {
        placePieceTrigger = pillar.GetComponentInChildren<InteractionTrigger>();
        placePieceTrigger.SetObserver(this);
        placePieceTrigger.gameObject.GetComponent<BoxCollider>().enabled = true;
        material = pillar.GetComponent<Renderer>().materials[2];
        id = pieceID;
        state = artifactState;
    }

    public void OnPlayerInteract() {
        material.SetColor("_EmissionColor", material.GetColor("_EmissionColor") * 20f);
        state.OnPlayerPlacePiece(id);
        placePieceTrigger.gameObject.SetActive(false);
    }
}
