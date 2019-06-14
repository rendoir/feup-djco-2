using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class ActivateArtifactState : State, InteractionObserver {

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
    private bool buttonPressed;
    private ArtifactButton button;
    private bool artifactAssembled;
    private InteractionTrigger pickupArtifactTrigger;
    public float piecesScale = 0.008f;

    public ActivateArtifactState() {
        foundMonument = false;
        buttonPressed = false;
        artifactAssembled = false;
        artifactActivator = GameManager.GetArtifactActivator();
        findMonumentTrigger = artifactActivator.GetComponentInChildren<StateTrigger>().gameObject.GetComponent<BoxCollider>();
        findMonumentTrigger.enabled = true;
        artifactPieces = GameObject.FindGameObjectsWithTag("ArtifactPieceActivator");
        button = artifactActivator.GetComponentInChildren<ArtifactButton>();
        button.SetState(this);
        pickupArtifactTrigger = artifactActivator.GetComponentInChildren<InteractionTrigger>();
        pickupArtifactTrigger.SetObserver(this);

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
        }
    }

    public override void Update() {
        if(foundMonument) {
            if(placedPieces >= GameManager.NUMBER_PIECES) {
                if(buttonPressed) {
                    if(!artifactAssembled) {
                        //Assemble artifact
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
        } else if(!artifactAssembled) {
            OnActivationOver();
            return this;
        } else return new SaveFriendState();
    }

    public override string GetMessage() {
        if(!foundMonument) 
            return "Find the monument to activate the artifact";
        else if(placedPieces < GameManager.NUMBER_PIECES)
            return "Place the artifact pieces on the pillars " + "(" + placedPieces + "/" + GameManager.NUMBER_PIECES + ")";
        else if(!artifactAssembled) 
            return "Activate the artifact";
        else return "Pick up the artifact";
    }

    private void OnActivationOver() {
        GameInput.CaptureInput(false);
        artifactAssembled = true;
        pickupArtifactTrigger.GetComponent<BoxCollider>().enabled = true;
        artifactActivator.GetComponentInChildren<AudioSource>().Play();
    }

    public void OnPlayerPlacePiece(int id) {
        artifactPieces[id].transform.localScale = new Vector3(piecesScale,piecesScale,piecesScale);
        placedPieces++;
    }

    public void OnButtonPressed(bool pressed) {
        if(placedPieces >= GameManager.NUMBER_PIECES && !artifactAssembled) {
            buttonPressed = pressed;
            GameInput.CaptureInput(true);
        }
    }

    public void OnPlayerInteract() {
        foreach (GameObject piece in artifactPieces)
            piece.SetActive(false);
        pickupArtifactTrigger.gameObject.SetActive(false);
        GameManager.GetArtifact().SetActive(true);
        GameState.Next();
    }

    public override void OnSceneLoaded(Scene scene) 
    {
        InitFriend();
    }

    public void InitFriend() {
        if(SceneManager.GetActiveScene().buildIndex == GameManager.MAIN_SCENE_INDEX) {
            GameObject friend = GameManager.GetFriend();
            Material friendMaterial = friend.GetComponentInChildren<SkinnedMeshRenderer>().material;
            friend.GetComponent<Animator>().SetTrigger("isDead");
            friendMaterial.SetColor("_EmissionColor", Color.white * 0f);
            friend.GetComponent<Rigidbody>().isKinematic = true;
            GameState.ResetFriendFinalPosition();
        }
    }
}
