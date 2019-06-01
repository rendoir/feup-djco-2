using UnityEngine;
using System.Threading.Tasks;

public class FindArtifactPiecesState : State {
    public override void Update() {
        if(GameManager.GetNumberPieces() >= GameManager.NUMBER_PIECES)
            GameState.Next();
    }

    public override State Next() {
        return new NullState(); // TODO
    }

    public override string GetMessage() {
        return "Find the artifact pieces (" + GameManager.GetNumberPieces() + "/" + GameManager.NUMBER_PIECES + ")";
    }
}
