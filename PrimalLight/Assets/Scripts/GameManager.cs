using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
	//Singleton
	static GameManager current;
    bool isInputCaptured;

	//Player
	GameObject player;
	Vector3 initialPosition;
	Quaternion initialRotation;

	//Handle player dying
	List<DeathObserver> deathObservers = new List<DeathObserver>();
	GameObject[] checkpoints;
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
	}

	void Start() {
		Cursor.lockState = CursorLockMode.Locked;
	}

	void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

	void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
		//Find Player
		player = GameObject.FindGameObjectWithTag("Player");
		initialPosition = player.transform.position;
		initialRotation = player.transform.localRotation;

		//Find Checkpoints
		checkpoints = GameObject.FindGameObjectsWithTag("Checkpoint");
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
		
		MovePlayerToClosestCheckpoint();

		//Notify observers that the player is alive
		foreach (DeathObserver obs in current.deathObservers)
			obs.OnPlayerAlive();
    }

	public void MovePlayerToClosestCheckpoint()
	{
		//If no checkpoints in the scene, move to initial position
		Vector3 closestCheckpoint = initialPosition;
		float currentDistance = Vector3.Distance(closestCheckpoint, player.transform.position);
		
		foreach(GameObject checkpoint in checkpoints) {
			float checkpointDistance = Vector3.Distance(checkpoint.transform.position, player.transform.position);
			if(checkpointDistance < currentDistance) {
				closestCheckpoint = checkpoint.transform.position;
				currentDistance = checkpointDistance;
			}
		}

		player.transform.position = closestCheckpoint;
		player.transform.localRotation = initialRotation;
	} 

	public static void RegisterDeathObserver(DeathObserver obs) {
		current.deathObservers.Add(obs);
	}
}