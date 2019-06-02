using UnityEngine;
using System.Threading.Tasks;

public class SageState : State {

    private bool enteredTrigger;
    public float talkDuration = 4f;
    public float startTime = -1;
    private GameObject player;
    private GameObject sage;
    
    public SageState() {
        enteredTrigger = false;
        player = GameManager.GetPlayer();
        sage = GameManager.GetSage();
    }

    public override void Update() {
        if(enteredTrigger) {
            if(startTime > 0 && Time.time - startTime >= talkDuration) {
                GameState.Next();
                return;
            }

            //Walk towards the sage
            Vector3 targetDir = sage.transform.position - player.transform.position;
            targetDir.y = 0f;
            if(targetDir.magnitude > 1.5f) {
                float step = 50f * Time.deltaTime;
                Vector3 newDir = Vector3.RotateTowards(player.transform.forward, targetDir, step, 0.0f);
                GameInput.cameraEulerAngles = Quaternion.LookRotation(newDir).eulerAngles;
                GameInput.vertical = 1f;
                GameInput.cameraForward = newDir;
            } else {
                GameInput.SimulateInput(false);
                GameInput.CaptureInput(true);
                if(startTime < 0)
                   startTime = Time.time;
            }
        }
    }

    public void OnSageTrigger() {
        enteredTrigger = true;
        GameInput.SimulateInput(true);
    }

    public override State Next() {
        if(!enteredTrigger) {
            OnSageTrigger();
            return this;
        } else {
            GameInput.SimulateInput(false);
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
