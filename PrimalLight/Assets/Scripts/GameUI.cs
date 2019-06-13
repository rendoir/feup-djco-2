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

		//Only done once
		ShowGameName();
	}

	void ShowGameName() {
		if(SceneManager.GetActiveScene().buildIndex == GameManager.MAIN_SCENE_INDEX)
			GameObject.Find("Canvas/GameName").GetComponent<RawImage>().enabled = true;
	}

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

	void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
		//Find Objective Text
		current.objective = GameObject.Find("Canvas/Objective").GetComponent<TextMeshProUGUI>();
    }

	void Update()
	{
        current.objective.text = GameState.GetMessage(); 
	}
}
