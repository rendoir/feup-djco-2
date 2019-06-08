using UnityEngine;
using System.Collections.Generic;

public class ActivateArtifactState : State {

    private bool foundMonument;
    public float assemblePiecesDuration = 4f;
    private GameObject artifactActivator;
    private BoxCollider findMonumentTrigger;
    
    private GameObject[] artifactPieces;
    private List<Vector3> artifactStartPositions = new List<Vector3>();
    private List<Quaternion> artifactStartRotations = new List<Quaternion>();
    private List<ArtifactPillar> artifactPillars = new List<ArtifactPillar>();
    public Vector3 artifactPosition;
    public Quaternion artifactRotation;
    private float elapsedTime = 0f;
    private int placedPieces = 0;

    public ActivateArtifactState() {
        foundMonument = false;
        artifactActivator = GameManager.GetArtifactActivator();
        findMonumentTrigger = artifactActivator.GetComponentInChildren<StateTrigger>().gameObject.GetComponent<BoxCollider>();
        findMonumentTrigger.enabled = true;
        artifactPieces = GameObject.FindGameObjectsWithTag("ArtifactPiece");

        //Calculate centroid
        artifactPosition = new Vector3(
            (artifactPieces[0].transform.parent.position.x + artifactPieces[1].transform.parent.position.x + artifactPieces[2].transform.parent.position.x)/3,
            (artifactPieces[0].transform.parent.position.y + artifactPieces[1].transform.parent.position.y + artifactPieces[2].transform.parent.position.y)/3 + 2f,
            (artifactPieces[0].transform.parent.position.z + artifactPieces[1].transform.parent.position.z + artifactPieces[2].transform.parent.position.z)/3);
        artifactRotation = Quaternion.Euler(Vector3.zero);

        //Save initial transform
        for(int i = 0; i < artifactPieces.Length; i++) {
            artifactStartPositions.Add(artifactPieces[i].transform.position);
            artifactStartRotations.Add(artifactPieces[i].transform.rotation);
            artifactPillars.Add(new ArtifactPillar(artifactPieces[i].transform.parent.gameObject, i, this));
            artifactPieces[i].SetActive(false);
        }
    }

    public override void Update() {
        if(foundMonument) {
            if(placedPieces >= GameManager.NUMBER_PIECES) {
                if(elapsedTime >= assemblePiecesDuration) {
                    GameState.Next();
                    return;
                }

                //Move and rotate towards one point
                elapsedTime += Time.deltaTime;
                for(int i = 0; i < artifactPieces.Length; i++) {
                    artifactPieces[i].transform.position = Vector3.Lerp(artifactStartPositions[i], artifactPosition, elapsedTime / assemblePiecesDuration);
                    artifactPieces[i].transform.rotation = Quaternion.Lerp(artifactStartRotations[i], artifactRotation, elapsedTime / assemblePiecesDuration);
                }
            }
        }
    }

    public void OnFoundMonument() {
        findMonumentTrigger.enabled = false;
        foundMonument = true;
    }

    public override State Next() {
        if(!foundMonument) {
            OnFoundMonument();
            return this;
        } else {
            OnActivationOver();
            return new SaveFriendState();
        }
    }

    public override string GetMessage() {
        if(!foundMonument) 
            return "Find the monument to activate the artifact";
        else if(placedPieces < GameManager.NUMBER_PIECES)
            return "Place the artifact pieces on the pillars " + "(" + placedPieces + "/" + GameManager.NUMBER_PIECES + ")";
        else return "Activate the artifact";
    }

    private void OnActivationOver() {
        GameManager.GetArtifact().SetActive(true);
        GameInput.CaptureInput(false);
    }

    public void OnPlayerPlacePiece(int id) {
        artifactPieces[id].SetActive(true);
        placedPieces++;
    }
}

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
