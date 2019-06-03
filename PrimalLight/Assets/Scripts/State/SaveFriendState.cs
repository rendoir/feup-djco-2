using UnityEngine;

public class SaveFriendState : State {

    private bool enteredTrigger;
    public float stateDuration = 10f;
    public float startTime = -1;
    private GameObject player;
    private GameObject friend;
    private bool isNearFriend;
    private BoxCollider friendTrigger;
    private Animator playerAnimator;

    public SaveFriendState() {
        enteredTrigger = false;
        isNearFriend = false;
        player = GameManager.GetPlayer();
        friend = GameManager.GetFriend();
        friendTrigger = friend.GetComponentInChildren<BoxCollider>();
        friendTrigger.enabled = true;
        playerAnimator = player.GetComponent<Animator>();
    }

    public override void Update() {
        if(enteredTrigger) {
            float elapsed = Time.time - startTime;
            if(isNearFriend) {
                if(elapsed >= stateDuration) {
                    GameState.Next();
                    return;
                }

                playerAnimator.SetBool("isHelping", true);
            } else {
                
                //Walk towards the friend
                Vector3 targetDir = friend.transform.position - player.transform.position;
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
                    isNearFriend = true;
                    if(startTime < 0) {
                    startTime = Time.time;
                    playerAnimator.SetTrigger("startKneeling");
                    }
                }
                
            }
        }
    }

    public void OnFriendTrigger() {
        friendTrigger.enabled = false;
        enteredTrigger = true;
        GameInput.SimulateInput(true);
    }

    public override State Next() {
        if(!enteredTrigger) {
            OnFriendTrigger();
            return this;
        } else {
            playerAnimator.SetTrigger("stopKneeling");
            playerAnimator.SetBool("isHelping", false);
            GameInput.SimulateInput(false);
            GameInput.CaptureInput(false);
            return new NullState();
        }
    }

    public override string GetMessage() {
        if(!enteredTrigger) 
            return "Go back to your friend";
        else return "Use the artifact";
    }
}
