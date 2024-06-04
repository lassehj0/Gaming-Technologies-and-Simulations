using System;
using UnityEngine;

public class AudioController : MonoBehaviour
{
	float volume = 0.2f;
	bool isMuted;

	void Start()
	{
		InitValues();
	}

	// Volume slider change
	public void SetVolume(float vol)
	{
		volume = vol / 100;
		AudioListener.volume = volume;
		Debug.Log(AudioListener.volume);

		if (volume == 0) isMuted = true;
		else if (isMuted) isMuted = false;
	}

	// Mute button pressed
	public void MuteBtnPressed()
	{
		isMuted = !isMuted;
		if (!isMuted && volume == 0) volume = 0.2f;
		AudioListener.volume = isMuted ? 0 : volume;
	}

	// Get values from PlayerPrefs or set them to default values if not set
	void InitValues()
	{
		if (PlayerPrefs.HasKey("isMuted"))
		{
			isMuted = Convert.ToBoolean(PlayerPrefs.GetString("isMuted"));
		}
		else
		{
			PlayerPrefs.SetString("isMuted", false.ToString());
		}

		if (PlayerPrefs.HasKey("volume"))
		{
			volume = PlayerPrefs.GetFloat("volume");
		}
		else
		{
			PlayerPrefs.SetFloat("volume", volume);
		}

		AudioListener.volume = isMuted ? 0 : volume;
	}
}
