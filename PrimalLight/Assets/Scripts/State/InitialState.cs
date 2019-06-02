using UnityEngine;
using System.Threading.Tasks;

public class InitialState : State {

    public float walkDuration = 5f;
    public float startTime = Time.time;
    private GameObject friend;
    private Rigidbody friendRB;
    
    public InitialState() {
        GameInput.SimulateInput(true);
        GameInput.cameraForward = Vector3.forward;
        GameInput.cameraEulerAngles = Vector3.zero;
        startTime = Time.time;
        friend = GameManager.GetFriend();
        friendRB = friend.GetComponent<Rigidbody>();
        friend.GetComponent<Animator>().SetTrigger("isWalking");
    }

    public override void Update() {
        if(Time.time - startTime >= walkDuration) {
            GameState.Next();
            return;
        }

        //Simulate player movement
        float t = Time.time - startTime;
        GameInput.vertical = Mathf.SmoothStep(0f, 1f, t);

        MoveFriend();
    }

    public override State Next() {
        StopFriend();
        GameInput.SimulateInput(false);
        return new FriendDeathState();
    }

    private void MoveFriend() {
        //Input
        Vector3 input = new Vector3(GameInput.horizontal, 0.0f, GameInput.vertical);

        // the movement direction is the way the came is facing
        Vector3 Direction = GameInput.cameraForward * input.z +
                            Vector3.Cross(GameInput.cameraForward, Vector3.up) *  -input.x;

        friend.transform.localEulerAngles = new Vector3(friend.transform.localEulerAngles.x, GameInput.cameraEulerAngles.y, friend.transform.localEulerAngles.z);

        //Walk
        float finalSpeed = 3f;
        Vector3 velocity = Direction.normalized * finalSpeed;
        velocity = Vector3.ClampMagnitude(velocity, finalSpeed); //Clamp velocity
        velocity.y = friendRB.velocity.y;

        friendRB.velocity = velocity;
    }

    private void StopFriend() {
        friendRB.velocity = Vector3.zero;
    }
}
