using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimController : MonoBehaviour {

	public Animator anim;
	public Button settingsBtn;
	public string nam;
	public float number;
	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
		Button btn = settingsBtn.GetComponent<Button> ();
		btn.onClick.AddListener (TaskOnClick);
	}
	


	public void TaskOnClick ()
	{
		anim.SetFloat (nam, number);
	}

}
