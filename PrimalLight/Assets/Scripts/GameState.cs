using UnityEngine;
using UnityEngine.SceneManagement;

public class GameState : MonoBehaviour
{
    static GameState current;
    State state;
	string message;
	Vector3 finalFriendPosition;

    void Awake()
	{
		//If a Game State exists and this isn't it...
		if (current != null && current != this)
		{
			//...destroy this and exit. There can only be one Game State
			Destroy(gameObject);
			return;
		}

		//Set this as the current game state
		current = this;

		//Persist this object between scene reloads
		DontDestroyOnLoad(gameObject);
	}

	void Start() {
		current.state = new InitialState();
        //current.state = new NullState();
		message = current.state.GetMessage();
	}

	void Update()
	{
		current.state.Update();
	}

    static public void Next()
    {
        current.state = current.state.Next();
		//Debug.Log(current.state.GetType().Name);
    }

	static public string GetMessage()
	{
		string newMessage = current.state.GetMessage();
		if(newMessage != "" && newMessage != current.message)
			GameSound.Play("NewObjective");
		current.message = newMessage;
		return current.message;
	}

	void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

	void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
		if(current.state != null)
			current.state.OnSceneLoaded(scene);
    }

	public static void SaveFriendFinalPosition() {
		current.finalFriendPosition = GameManager.GetFriend().transform.position;
	}

	public static void ResetFriendFinalPosition() {
		GameManager.GetFriend().transform.position = current.finalFriendPosition;
	}
}
