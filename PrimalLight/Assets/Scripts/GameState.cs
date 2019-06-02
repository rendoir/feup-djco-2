using UnityEngine;

public class GameState : MonoBehaviour
{
    static GameState current;
    State state;

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
		//current.state = new InitialState();
		//current.state = new FindArtifactPiecesState();
		//current.state = new SageState();
        current.state = new NullState();
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
		return current.state.GetMessage();
	}
}
