using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveTreeScript : MonoBehaviour {

	public static bool TreeIsActive = false;
	public GameObject PassiveTreeUI;
	public GameObject StatsUI;
	public InputManager inputManagerDatabase;


	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(inputManagerDatabase.PassiveSkillTreeCode)) {

		
			if (TreeIsActive) 
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

        PassiveTreeUI.SetActive (false);
		StatsUI.SetActive (false);
		TreeIsActive = false;
	//	Cursor.visible = false;

	}

	void Pause()
    {
        PlayerManager.instance.WindowOpened();

        PassiveTreeUI.SetActive (true);
		StatsUI.SetActive (true);
		TreeIsActive = true;
	//	Cursor.visible = true;
	}


}
