using UnityEngine;
using System.Threading.Tasks;

public class FriendDeathState : State {

    public float stateDuration = 7f;
    public float startTime = Time.time;
    private GameObject player;
    private GameObject friend;
    
    public FriendDeathState() {
        GameInput.CaptureInput(true);
        player = GameManager.GetPlayer();
        friend = GameManager.GetFriend();
        friend.GetComponent<Animator>().SetTrigger("isDead");
        player.GetComponent<Animator>().SetTrigger("startKneeling");
    }

    public override void Update() {
        if(Time.time - startTime >= stateDuration) {
            GameState.Next();
            return;
        }

        //Rotate player towards friend
        Vector3 targetDir = friend.transform.position - player.transform.position;
        targetDir.y = 0f;
        float step = 250f * Time.deltaTime;
        Vector3 newDir = Vector3.RotateTowards(player.transform.forward, targetDir, step, 0.0f);
        player.transform.rotation = Quaternion.LookRotation(newDir);
    }

    public override State Next() {
        player.GetComponent<Animator>().SetTrigger("stopKneeling");
        GameInput.CaptureInput(false);
        return new SageState();
    }
}
