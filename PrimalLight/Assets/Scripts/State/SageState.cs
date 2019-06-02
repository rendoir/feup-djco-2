using UnityEngine;
using System.Threading.Tasks;

public class SageState : State {

    private bool enteredTrigger;
    public float stateDuration = 5f;
    public float startTime = Time.time;
    
    public SageState() {
        enteredTrigger = false;
    }

    public override void Update() {
        if(enteredTrigger) {
            if(Time.time - startTime >= stateDuration) {
                GameState.Next();
                return;
            }
        }

        //if(enteredTrigger) {
            //Walk player towards sage
            //if(isNear)
                //Play cinematic or animation?
                //if(isOver)
                    //GameState.Next();
        //}
    }

    public void OnSageTrigger() {
        enteredTrigger = true;
        GameInput.CaptureInput(true);
    }

    public override State Next() {
        if(!enteredTrigger) {
            OnSageTrigger();
            return this;
        } else {
            GameInput.CaptureInput(false);
            return new FindArtifactPiecesState();
        }
    }

    public override string GetMessage() {
        if(!enteredTrigger) 
            return "Find the sage";
        else return "";
    }
}
