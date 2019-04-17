using UnityEngine;

public class GameManager : MonoBehaviour
{
	//Singleton
	static GameManager current;
    bool isInputCaptured;

	void Awake()
	{
		//If a Game Manager exists and this isn't it...
		if (current != null && current != this)
		{
			//...destroy this and exit. There can only be one Game Manager
			Destroy(gameObject);
			return;
		}

		//Set this as the current game manager
		current = this;

		//Persist this object between scene reloads
		DontDestroyOnLoad(gameObject);
	}

    public static void CaptureInput(bool shouldCapture) {
        current.isInputCaptured = shouldCapture;
    }

    public static bool IsInputCaptured() {
        return current.isInputCaptured;
    }
}