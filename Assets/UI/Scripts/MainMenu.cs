using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

	public Button btnContinue;
 
	public void QuitGame ()
	{
		Application.Quit ();
	}

	public void ContinueActivator(){
		if (LoadGameScript.LoadGameName != null) {

			btnContinue.gameObject.SetActive (true);
		}
	}

	public void ContinueGame(int SceneIndex){
		if (LoadGameScript.LoadGameName != null) {
			SceneManager.LoadScene (SceneIndex);
		}
	}


}
