using UnityEngine;
using System.Collections.Generic;

public class ActivateArtifactState : State {

    private bool enteredTrigger;
    public float activationDuration = 4f;
    public float startTime = -1;
    private GameObject artifactActivator;
    private BoxCollider artifactActivatorTrigger;
    
    private GameObject[] artifactPieces;
    private List<Vector3> artifactStartPositions = new List<Vector3>();
    private List<Quaternion> artifactStartRotations = new List<Quaternion>();
    public Vector3 artifactPosition;
    public Quaternion artifactRotation;
    public float elapsedArtifact = 0f;

    public ActivateArtifactState() {
        enteredTrigger = false;
        artifactActivator = GameManager.GetArtifactActivator();
        artifactActivatorTrigger = artifactActivator.GetComponentInChildren<BoxCollider>();
        artifactActivatorTrigger.enabled = true;
        artifactPieces = GameObject.FindGameObjectsWithTag("ArtifactPiece");

        //Save initial transform
        foreach (GameObject piece in artifactPieces) {
            artifactStartPositions.Add(piece.transform.position);
            artifactStartRotations.Add(piece.transform.rotation);
        }

        //Calculate centroid
        artifactPosition = new Vector3(
            (artifactPieces[0].transform.parent.position.x + artifactPieces[1].transform.parent.position.x + artifactPieces[2].transform.parent.position.x)/3,
            (artifactPieces[0].transform.parent.position.y + artifactPieces[1].transform.parent.position.y + artifactPieces[2].transform.parent.position.y)/3 + 2f,
            (artifactPieces[0].transform.parent.position.z + artifactPieces[1].transform.parent.position.z + artifactPieces[2].transform.parent.position.z)/3);
        artifactRotation = Quaternion.Euler(Vector3.zero);
    }

    public override void Update() {
        if(enteredTrigger) {
            if(Time.time - startTime >= activationDuration) {
                GameState.Next();
                return;
            }

            //Move and rotate towards one point
            elapsedArtifact += Time.deltaTime;
            for(int i = 0; i < artifactPieces.Length; i++) {
                artifactPieces[i].transform.position = Vector3.Lerp(artifactStartPositions[i], artifactPosition, elapsedArtifact / activationDuration);
                artifactPieces[i].transform.rotation = Quaternion.Lerp(artifactStartRotations[i], artifactRotation, elapsedArtifact / activationDuration);
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
