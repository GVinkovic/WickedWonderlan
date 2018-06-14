using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.PostProcessing;

public class GameOverMenuScript : MonoBehaviour {

	public GameObject GameOverMenuUI;
	public Button LoadBtn, MainMenuBtn, QuitBtn;
	public Camera MainCam;
	PostProcessingBehaviour PFX;
	// Use this for initialization
	void Start () {
		
		LoadBtn.onClick.AddListener (setLoadOnClick);
		MainMenuBtn.onClick.AddListener (setMenuOnClick);
		QuitBtn.onClick.AddListener (setQuitOnClick);
		PFX = MainCam.GetComponent<PostProcessingBehaviour> ();
		
	}

	 public void loadMenu(){

		PlayerManager.instance.WindowOpened();
		GameOverMenuUI.SetActive (true);
		Time.timeScale = 0.1f;
		PFX.enabled = true;

		//TODO disable input
	}

	void setMenuOnClick(){

		SceneManager.LoadScene (0);
		Time.timeScale = 1f;
		PlayerManager.instance.WindowClosed();
	}

	void setLoadOnClick(){
		if (LoadGameScript.LoadGameName != null) {
			PlayerManager.instance.WindowClosed();
			SceneManager.LoadScene (2);
		}

	}


	void setQuitOnClick(){
		Application.Quit ();
	}
}
