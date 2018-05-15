using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelection : MonoBehaviour {

	private GameObject[] characterList;
	private int index = 0;


	private void Start(){
		index = PlayerPrefs.GetInt ("CharacterSelected");
		characterList = new GameObject[2];

		for (int i = 0; i < 2; i++) 
			characterList [i] = transform.GetChild(i).gameObject;

			
		foreach (GameObject go in characterList) 
				go.SetActive (false);
			
		if (characterList [index])
			characterList [index].SetActive (true);
	}

	public void toggleLeft()
	{
		characterList [index].SetActive (false);

		index--;
		if (index < 0)
			index = characterList.Length - 1; 

		characterList [index].SetActive (true);


	}

	public void toggleRight()
	{
		characterList [index].SetActive (false);

		index++;
		if (index == characterList.Length)
			index = 0; 

		characterList [index].SetActive (true);


	}

	public void ConfirmButton(int SceneIndex){

		PlayerPrefs.SetInt ("CharacterSelected", index);
        LoadGameScript.LoadGameName = "Profile" + SaveGame.GetSaveGameNames().Count + 1;
		SceneManager.LoadScene (SceneIndex);
	}

}
