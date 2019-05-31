using UnityEngine;
using System.Threading.Tasks;

public class FriendDeathState : State {

    public int stateDuration = 3;
    private Quaternion target;
    
    public FriendDeathState() {
        GameInput.CaptureInput(true);
        target = Quaternion.Euler(0f, 90f, 0f);

        //After x seconds, 
        Task.Delay(stateDuration * 1000).ContinueWith(t => {
            GameState.Next();
        });
    }

    public override void Update() {
        //Turn player to friend
        //Play cinematic or animation?

        //Debug.Log("death update");
        
        float step = 300f * Time.deltaTime;
        GameManager.GetPlayer().transform.rotation = Quaternion.RotateTowards(GameManager.GetPlayer().transform.rotation, target, step);
    }

    public override State Next() {
        GameInput.CaptureInput(false);
        return new SageState();
    }
}
