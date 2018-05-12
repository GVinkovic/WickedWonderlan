using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuScript : MonoBehaviour {

	public static bool GameIsPaused = false;
	public GameObject PauseMenuUI;
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Escape)) {

			if (GameIsPaused) 
			{
				Resume ();
			} else
			{
				Pause ();

			}
		}
	}

	public void Resume()
	{
		PauseMenuUI.SetActive (false);
		Time.timeScale = 1f;
		GameIsPaused = false;
		Cursor.visible = false;

	}

	void Pause()
	{
		PauseMenuUI.SetActive (true);
		Time.timeScale = 0f;
		GameIsPaused = true;
		Cursor.visible = true;
	}

	public void LoadMenu(int sceneIndex){
		Time.timeScale = 1f;
		SceneManager.LoadScene (sceneIndex);

	}

	public void QuitGame(){
		Application.Quit ();
	}
}
