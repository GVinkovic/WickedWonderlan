using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds : MonoBehaviour {

	public AudioClip footStep;
	public AudioSource ass;



	void Footstep(){

		ass.PlayOneShot (footStep);
	}
}
