using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    static GameUI current;
    TextMeshProUGUI objective;

    void Awake()
	{
		//If a Game UI exists and this isn't it...
		if (current != null && current != this)
		{
			//...destroy this and exit. There can only be one Game UI
			Destroy(gameObject);
			return;
		}

		//Set this as the current game UI
		current = this;

		//Persist this object between scene reloads
		DontDestroyOnLoad(gameObject);
	}

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
	
	void OnDestroy()
	{
		SceneManager.sceneLoaded -= OnSceneLoaded;
	}

	void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
		if(SceneManager.GetActiveScene().buildIndex == MenuManager.MENU_SCENE_INDEX)
			return;

		//Find Objective Text
		current.objective = GameObject.Find("Canvas/Objective").GetComponent<TextMeshProUGUI>();
    }

	void Update()
	{
        current.objective.text = GameState.GetMessage(); 
	}
}
