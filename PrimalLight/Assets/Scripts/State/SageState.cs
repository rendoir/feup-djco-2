using UnityEngine;
using System.Threading.Tasks;

public class SageState : State {

    private bool enteredTrigger;
    public int stateDuration = 5;
    
    public SageState() {
        enteredTrigger = false;
    }

    public override void Update() {
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
        
        //After x seconds, 
        Task.Delay(stateDuration * 1000).ContinueWith(t => {
            GameState.Next();
        });
    }

    public override State Next() {
        if(!enteredTrigger) {
            OnSageTrigger();
            return this;
        } else {
            GameInput.CaptureInput(false);
            return new NullState(); // TODO
        }
    }
}
