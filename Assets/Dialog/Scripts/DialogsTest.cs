using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogsTest : MonoBehaviour {

    public string ScenarioName;
    public bool simpleDialog;
	// Use this for initialization
	void Start () {
        if (simpleDialog)
            GameManager.GetDialogMgr.BeginDialog(GameManager.GetDialogsCollection.getDialogList(ScenarioName));
        else
        {
            GameManager.GetButtonsDialogMgr.BeginDialog(GameManager.GetDialogsCollection.getDialogList(ScenarioName));
            Cursor.visible = true;
        } 

	}
	

}
