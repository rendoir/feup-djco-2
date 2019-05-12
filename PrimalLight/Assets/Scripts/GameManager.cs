using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
	//Singleton
	static GameManager current;
    bool isInputCaptured;

	//Handle player dying
	List<DeathObserver> deathObservers = new List<DeathObserver>();

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

	void Start() {
		Cursor.lockState = CursorLockMode.Locked;
	}

    public static void CaptureInput(bool shouldCapture) {
        current.isInputCaptured = shouldCapture;
    }

    public static bool IsInputCaptured() {
        return current.isInputCaptured;
    }

	public static void PlayerDied() {
		foreach (DeathObserver obs in current.deathObservers) {
			obs.OnPlayerDeath();
		}
	}

	public static void RegisterDeathObserver(DeathObserver obs) {
		current.deathObservers.Add(obs);
	}
}