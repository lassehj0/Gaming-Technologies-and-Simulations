using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour
{
	[SerializeField]
	Canvas mainMenu;

	//Volume
	[SerializeField]
	Button muteBtn;
	[SerializeField]
	Sprite mute, unmute;
	[SerializeField]
	Slider volumeSlider;
	[SerializeField]
	TextMeshProUGUI volumeText;

	SpriteRenderer muteRenderer;
	bool isMuted = false;
	float volume;

	//FPS
	[SerializeField]
	Slider fpsSlider;
	[SerializeField]
	TextMeshProUGUI fpsText;
	[SerializeField]
	Toggle vSyncTgl, showFpsTgl;

	void Start()
	{
		InitValues();
		muteRenderer = muteBtn.transform.GetChild(0).GetComponent<SpriteRenderer>();
		muteRenderer.sprite = isMuted ? mute : unmute;
	}

	// Button press event from mute button
	public void MuteBtnPressed()
	{
		isMuted = !isMuted;
		muteRenderer.sprite = isMuted ? mute : unmute;

		if (!isMuted)
		{
			// If it is not muted but no value is set then set to a default value
			if (volume == 0) volume = 0.2f;

			volumeSlider.SetValueWithoutNotify(volume * 100);
		}
		else volumeSlider.SetValueWithoutNotify(0);
	}

	public void Back()
	{
		//Save values
		PlayerPrefs.SetString("isMuted", isMuted.ToString());
		PlayerPrefs.SetFloat("volume", volume);
		PlayerPrefs.SetInt("fpsCap", (int)fpsSlider.value * 10);
		PlayerPrefs.SetString("vSyncEnabled", vSyncTgl.isOn.ToString());
		PlayerPrefs.SetString("showFps", showFpsTgl.isOn.ToString());

		// Switch back to menu canvas
		gameObject.SetActive(false);
		mainMenu.gameObject.SetActive(true);
	}

	// Slider change event from volume slider
	public void SetVolume(float vol)
	{
		volume = vol / 100;
		if (volume == 0)
		{
			isMuted = true;
			muteRenderer.sprite = isMuted ? mute : unmute;
		}
		else if (isMuted)
		{
			isMuted = false;
			muteRenderer.sprite = isMuted ? mute : unmute;
		}
		volumeText.text = volume * 100 + "%";
	}

	// Slider change event from FPS slider
	public void FpsCapChange(float fpsCap)
	{
		if (fpsCap == 25) fpsText.text = "UNLIMITED";
		else fpsText.text = (fpsCap * 10).ToString();
	}

	// Get values from PlayerPrefs
	void InitValues()
	{
		//Volume
		isMuted = Convert.ToBoolean(PlayerPrefs.GetString("isMuted"));

		volume = PlayerPrefs.GetFloat("volume");
		volumeSlider.SetValueWithoutNotify(isMuted ? 0 : volume * 100);
		volumeText.text = volume * 100 + "%";

		//FPS
		vSyncTgl.isOn = Convert.ToBoolean(PlayerPrefs.GetString("vSyncEnabled"));

		var temp = PlayerPrefs.GetFloat("fpsCap");
		fpsText.text = temp == 25 ? "UNLIMITED" : temp.ToString();
		fpsSlider.value = temp / 10;

		showFpsTgl.isOn = Convert.ToBoolean(PlayerPrefs.GetString("showFps"));
	}
}
