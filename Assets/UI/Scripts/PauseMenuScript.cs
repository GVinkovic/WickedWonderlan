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
        PlayerManager.instance.WindowClosed();
        PauseMenuUI.SetActive (false);
		Time.timeScale = 1f;
		GameIsPaused = false;
		//Cursor.visible = false;
	}
	void Pause()
	{
        PlayerManager.instance.WindowOpened();
		PauseMenuUI.SetActive (true);
		Time.timeScale = 0f;
		GameIsPaused = true;
		//Cursor.visible = true;
	}

	public void LoadMenu(int sceneIndex){
		Time.timeScale = 1f;
		SceneManager.LoadScene (sceneIndex);
	}

	public void SaveGamee(){
		//SaveGame.Save ("SavedGame" + SaveGame.GetSaveGameNames ().Count);
		SaveGame.Save(LoadGameScript.LoadGameName);
		Resume ();
	}

	public void QuitGame(){
		Application.Quit ();
	}

}
