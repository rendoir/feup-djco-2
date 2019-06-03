using UnityEngine;
using System.Threading.Tasks;

public class FindArtifactPiecesState : State {
    public override void Update() {
        if(GameManager.GetNumberPieces() >= GameManager.NUMBER_PIECES)
            GameState.Next();
        //GameManager.OnRewardObtained(); //TODO REMOVE
    }

    public override State Next() {
        return new SaveFriendState(); // TODO REPLACE WITH ACTIVATE ARTIFACT STATE
    }

    public override string GetMessage() {
        return "Find the artifact pieces (" + GameManager.GetNumberPieces() + "/" + GameManager.NUMBER_PIECES + ")";
    }
}
