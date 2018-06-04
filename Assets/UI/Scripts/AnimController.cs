using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimController : MonoBehaviour {

	public Animator anim;
	// Use this for initialization
	/*void Start () {
		anim = GameObject.Find ("Main Camera").GetComponent<Animator> ();
	}
	*/


	public void TaskOnClick (int animationIndex)
	{
		anim.SetFloat ("Animate", animationIndex);
	}

}
