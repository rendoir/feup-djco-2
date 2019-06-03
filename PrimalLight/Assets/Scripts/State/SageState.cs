using UnityEngine;

public class SageState : State {

    private bool enteredTrigger;
    public float talkDuration = 6.5f;
    public float pointDuration = 2.25f;
    public float startTime = -1;
    private GameObject player;
    private GameObject sage;
    private bool isNearSage;
    private bool isPointing;
    private Quaternion sageTargetRotation;
    private BoxCollider sageTrigger;
    
    public SageState() {
        enteredTrigger = false;
        isNearSage = false;
        isPointing = false;
        player = GameManager.GetPlayer();
        sage = GameManager.GetSage();
        sageTargetRotation = Quaternion.Euler(sage.transform.rotation.x, sage.transform.rotation.y + 90f, sage.transform.rotation.z);
        sageTrigger = sage.GetComponentInChildren<BoxCollider>();
        sageTrigger.enabled = true;
    }

    public override void Update() {
        if(enteredTrigger) {
            float elapsed = Time.time - startTime;
            if(isNearSage && elapsed >= talkDuration) {
                if(elapsed >= talkDuration + pointDuration) {
                    GameState.Next();
                    return;
                }
                
                //Update sage
                sage.transform.rotation = Quaternion.RotateTowards(sage.transform.rotation, sageTargetRotation, 250f * Time.deltaTime);
                if(!isPointing) {
                    sage.GetComponent<Animator>().SetTrigger("isPointing");
                    isPointing = true;
                }
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
                isNearSage = true;
                if(startTime < 0) {
                   startTime = Time.time;
                   player.GetComponent<Animator>().SetTrigger("isYelling");
                }
            }
        }
    }

    public void OnSageTrigger() {
        sageTrigger.enabled = false;
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
