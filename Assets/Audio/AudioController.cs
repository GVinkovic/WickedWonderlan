using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Audio;

public class AudioController : MonoBehaviour {


	public Sound[] sounds;
	public AudioMixerGroup SFXMixer;

	void Awake(){

		DontDestroyOnLoad (gameObject);
		foreach (Sound s in sounds) {
			s.source = gameObject.AddComponent<AudioSource> ();
			s.source.clip = s.clip;
			s.source.outputAudioMixerGroup = SFXMixer;
			s.source.volume = s.volume;
			s.source.pitch = s.pitch;
			s.source.loop = s.loop;
		}
	}

	public void Play(string name){

		Sound s = Array.Find (sounds, sound => sound.name == name);
		if (s == null)
			return;
		s.source.spatialBlend = 1f;
		s.source.Play ();
	}
}

//POZIVANJE -- gameObject.GetComponent<AudioController>().Play ("MagicShot");
