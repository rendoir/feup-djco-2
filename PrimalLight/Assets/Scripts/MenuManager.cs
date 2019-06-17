using UnityEngine;
using UnityEngine.SceneManagement;


[DefaultExecutionOrder(-250)]
public class MenuManager : MonoBehaviour
{
    public static int MENU_SCENE_INDEX = 0;


	void Start() {
		Cursor.lockState = CursorLockMode.None;

        //Try to find game manager and destroy it
        GameObject gameManager = GameObject.Find("Game Manager");
        Destroy(gameManager);
	}

    public void Play() {
        SceneManager.LoadScene(GameManager.MAIN_SCENE_INDEX);
    }

    public void Exit() {
        Application.Quit();
    }
}