using UnityEngine;

public class InitialState : State {

    public float walkDuration = 53f;
    public float startTime = Time.time;
    private GameObject friend;
    private Rigidbody friendRB;
    private Material friendMaterial;
    private float initialSpeed;
    public float stateSpeed = 0.95f;
    private PlayerMovement playerMovement;
    
    public InitialState() {
        GameInput.SimulateInput(true);
        startTime = Time.time;
        GameObject player = GameManager.GetPlayer();
        GameInput.cameraEulerAngles = player.transform.eulerAngles;
        GameInput.cameraForward = player.transform.forward;
        friend = GameManager.GetFriend();
        friendRB = friend.GetComponent<Rigidbody>();
        friendMaterial = friend.GetComponentInChildren<SkinnedMeshRenderer>().material;
        friend.GetComponent<Animator>().SetTrigger("isWalking");
        playerMovement = GameManager.GetPlayer().GetComponent<PlayerMovement>();
        initialSpeed = playerMovement.movementSpeed;
        playerMovement.movementSpeed = stateSpeed;
    }

    public override void Update() {
        if(Time.time - startTime >= walkDuration) {
            GameState.Next();
            return;
        }

        //Simulate player movement
        float t = Time.time - startTime;
        GameInput.vertical = Mathf.SmoothStep(0f, 1f, t);

        //Move friend
        MoveFriend();

        //Ping pong friend color
        float intensity = Mathf.PingPong(Time.time * 0.5f, 1f);
        friendMaterial.SetColor("_EmissionColor", Color.white * intensity);
    }

    public override State Next() {
        StopFriend();
        GameInput.SimulateInput(false);
        GameState.SaveFriendFinalPosition();
        playerMovement.movementSpeed = initialSpeed;
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
        float finalSpeed = stateSpeed;
        Vector3 velocity = Direction.normalized * finalSpeed;
        velocity = Vector3.ClampMagnitude(velocity, finalSpeed); //Clamp velocity
        velocity.y = friendRB.velocity.y;

        friendRB.velocity = velocity;
    }

    private void StopFriend() {
        friendRB.velocity = Vector3.zero;
    }
}
