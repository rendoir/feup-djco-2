using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
	//Singleton
	static GameManager current;
    bool isInputCaptured;

	//Player
	GameObject player;

	//Handle player dying
	List<DeathObserver> deathObservers = new List<DeathObserver>();
	public float restartTime = 6f;

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

		//Find Player
		player = GameObject.FindGameObjectWithTag("Player");
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
		foreach (DeathObserver obs in current.deathObservers)
			obs.OnPlayerDeath();

		//Restart
		current.StartCoroutine(current.RestartPlayer());
	}

	private IEnumerator RestartPlayer() {
        yield return new WaitForSeconds(current.restartTime);

		//TODO - Add checkpoints
		
		foreach (DeathObserver obs in current.deathObservers)
			obs.OnPlayerAlive();
    }

	public static void RegisterDeathObserver(DeathObserver obs) {
		current.deathObservers.Add(obs);
	}
}