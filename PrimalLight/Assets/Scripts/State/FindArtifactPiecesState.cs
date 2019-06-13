using UnityEngine;
using UnityEngine.SceneManagement;

public class FindArtifactPiecesState : State {
    public override void Update() {
        if(GameManager.GetNumberPieces() >= GameManager.NUMBER_PIECES)
            GameState.Next();
    }

    public override State Next() {
        if(GameManager.GetNumberPieces() >= GameManager.NUMBER_PIECES && 
           SceneManager.GetActiveScene().buildIndex == GameManager.MAIN_SCENE_INDEX)
            return new ActivateArtifactState();
        else return this;
    }

    public override string GetMessage() {
        if(GameManager.GetNumberPieces() < GameManager.NUMBER_PIECES)
            return "Find the artifact pieces (" + GameManager.GetNumberPieces() + "/" + GameManager.NUMBER_PIECES + ")";
        else return "Find the monument to activate the artifact";
    }

    public override void OnSceneLoaded(Scene scene) 
    {
        InitFriend();
        GameState.Next();
    }

    public void InitFriend() {
        if(SceneManager.GetActiveScene().buildIndex == GameManager.MAIN_SCENE_INDEX) {
            GameObject friend = GameManager.GetFriend();
            Material friendMaterial = friend.GetComponentInChildren<SkinnedMeshRenderer>().material;
            friend.GetComponent<Rigidbody>().isKinematic = true;
            friend.GetComponent<Animator>().SetTrigger("isDead");
            friendMaterial.SetColor("_EmissionColor", Color.white * 0f);
            GameState.ResetFriendFinalPosition();
        }
    }
}
