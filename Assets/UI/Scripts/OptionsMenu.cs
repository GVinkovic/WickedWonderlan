using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour {

	public AudioMixer MusicMixer, SFXMixer;
	Resolution[] resolutions;
	public Dropdown resolutionDropdown, textureDropdown, shadowDropdown;
	public Toggle fullScreen;
	public Slider SFXslider, musicSlider;	 
	int currentResolutionIndex = 0;

	void Start(){
		resolutions = Screen.resolutions;

		resolutionDropdown.ClearOptions ();

		List<string> options = new List<string> ();

		for (int i = 0; i < resolutions.Length; i++) {
			string option = resolutions [i].width + " x " + resolutions [i].height;
			options.Add (option);

			if (resolutions [i].width == Screen.currentResolution.width && resolutions [i].height == Screen.currentResolution.height) {

				currentResolutionIndex = i;
			}
		}
		resolutionDropdown.AddOptions (options);
		resolutionDropdown.value = currentResolutionIndex;
		resolutionDropdown.RefreshShownValue ();

		loadPrefs ();

	}

	public void loadPrefs()
	{

		if(PlayerPrefs.HasKey("sfxVolume")){
			float f = PlayerPrefs.GetFloat ("sfxVolume");
			setSFXVolume (f);
		}

		if(PlayerPrefs.HasKey("mVolume")){
			float f = PlayerPrefs.GetFloat ("mVolume");
			setMusicVolume (f);
		}

		if(PlayerPrefs.HasKey("fullScreen")){
			int i = PlayerPrefs.GetInt ("MusicVolume");

			if (i != 0)
				setFullscreen (true);
			else
				setFullscreen (false);
		}

		if(PlayerPrefs.HasKey("textureIndex")){
			int i = PlayerPrefs.GetInt ("textureIndex");
			setTextureQuality (i);
		}

		if(PlayerPrefs.HasKey("shadowIndex")){
			int i = PlayerPrefs.GetInt ("shadowIndex");
			setShadowQuality (i);
		}

	}

	public void setMusicVolume(float volume){
		MusicMixer.SetFloat ("MusicVolume", volume);
		PlayerPrefs.SetFloat ("mVolume", volume);
		musicSlider.value = volume;
	}

	public void setSFXVolume(float volume){
		SFXMixer.SetFloat ("SFXVolume", volume);
		PlayerPrefs.SetFloat ("sfxVolume", volume);
		SFXslider.value = volume;
	}

	public void setFullscreen(bool isFullScreen)
	{	
		if (isFullScreen) {
			fullScreen.isOn = true;
		} else {
			fullScreen.isOn = false;
		}

		Screen.fullScreen = isFullScreen;
		PlayerPrefs.SetInt ("fullScreen", ((isFullScreen) ? 1:0));
		fullScreen.isOn = isFullScreen;
	}

	public void setResolution(int resolutionIndex){

		Resolution resolution = resolutions [resolutionIndex];
		Screen.SetResolution (resolution.width, resolution.height, Screen.fullScreen);
		PlayerPrefs.SetInt ("resolutionIndex", resolutionIndex);
	}

	public void setTextureQuality(int tQualityIndex){
		QualitySettings.masterTextureLimit = tQualityIndex;
		PlayerPrefs.SetInt ("textureIndex", tQualityIndex);
		textureDropdown.value = tQualityIndex;
	}

	public void setShadowQuality(int s){
		if (s == 0) {
			QualitySettings.shadowResolution = ShadowResolution.High;
		}else if (s == 1) {
			QualitySettings.shadowResolution = ShadowResolution.Medium;
		}else if(s == 2) {
			QualitySettings.shadowResolution = ShadowResolution.Low;
		}
		PlayerPrefs.SetInt ("shadowIndex", s);
		shadowDropdown.value = s;
	}
}
