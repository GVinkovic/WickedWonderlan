using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;


public class LoadGameScript : MonoBehaviour {

	private Button[] buttons;
	private Text[] texts;
	public static string LoadGameName;

	// Use this for initialization
	void Start ()
	{
		//spremanje save game naziva i datuma u text buttona
		buttons = GetComponentsInChildren<Button> ();
		int i = 0;
			foreach (var profilName in SaveGame.GetSaveGameNames()) {
			DateTime dt = SaveGame.getSaveTime (profilName);
			texts = buttons [i].GetComponentsInChildren<Text> ();
				for (int j = 0; j <1; j++) {
					texts [j].text = profilName;
					texts [j + 1].text = dt.ToString("dd.MM.yyyy, HH:mm:ss");
				}
			if (i < buttons.Length-1) {
				i++;
			} else {
				i = 0;
			}
			}
	}
	//spremanje naziva saveGamea 
	public void StoreLoad(){
		LoadGameName = EventSystem.current.currentSelectedGameObject.GetComponentInChildren<Text>().text.ToString();
	}

	//pozivanje druge scene
	public void Load(int SceneIndex){
		if(LoadGameName != null && LoadGameName != "Button"){
		SceneManager.LoadScene(SceneIndex);
		}
	}
}
