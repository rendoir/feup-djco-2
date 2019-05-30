using UnityEngine;
using System.Threading.Tasks;

public class InitialState : State {

    public int stateDuration = 5;
    private float startTime;
    
    public InitialState() {
        GameInput.SimulateInput(true);
        GameInput.cameraForward = Vector3.forward;
        GameInput.cameraEulerAngles = Vector3.zero;
        startTime = Time.time;

        //After x seconds, 
        Task.Delay(stateDuration * 1000).ContinueWith(t => {
            GameState.Next();
        });
    }

    public override void Update() {
        //Move both players
        //Fade friend color and death animation
        //Play cinematic or animations?

        float t = Time.time - startTime;
        GameInput.vertical = Mathf.SmoothStep(0f, 1f, t);
    }

    public override State Next() {
        GameInput.SimulateInput(false);
        return new FriendDeathState(); // TODO
    }
}
