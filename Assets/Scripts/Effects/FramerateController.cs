using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class FramerateController : MonoBehaviour
{
	[SerializeField]
	TextMeshProUGUI fpsCounter;

	bool vSyncEnabled, fpsCapEnabled, showFps;
	int fpsCap, maxScreenFramerate;

	void Start()
	{
		// Setting max refresh rate of the display
		maxScreenFramerate = Mathf.RoundToInt((float)Screen.currentResolution.refreshRateRatio.value);
		InitValues();

		StartCoroutine(nameof(DisplayFPS));
		fpsCounter.enabled = showFps;
	}

	IEnumerator DisplayFPS()
	{
		while (showFps)
		{
			// Use smoothDeltaTime if timeScale is 1 since it is more stable
			if (Time.timeScale == 1) fpsCounter.text = Mathf.RoundToInt(1 / Time.smoothDeltaTime).ToString();

			// Otherwise use unscaledDeltaTime since it works when timeScale is 0
			else fpsCounter.text = Mathf.RoundToInt(1f / Time.unscaledDeltaTime).ToString();
			yield return new WaitForSecondsRealtime(0.15f);
		}
	}

	// Changes whether current FPS should be shown
	public void ShowFpsSwitched(bool showFps)
	{
		if (showFps) StartCoroutine(nameof(DisplayFPS));
		fpsCounter.enabled = showFps;
	}

	// Changes whether vSync is on or off
	public void VSyncSwitch(bool vSyncEnabled)
	{
		this.vSyncEnabled = vSyncEnabled;
		Application.targetFrameRate = vSyncEnabled ? maxScreenFramerate :
										fpsCapEnabled ? fpsCap : -1;
	}

	// Changes FPS cap
	public void FpsCapChange(float fpsCap)
	{
		// If FPS cap is maxed disable FPS cap
		if (fpsCap == 25) fpsCapEnabled = false;
		else fpsCapEnabled = true;

		this.fpsCap = (int)fpsCap * 10;
		Application.targetFrameRate = vSyncEnabled ? maxScreenFramerate :
										fpsCapEnabled ? this.fpsCap : -1;
	}

	// Get vales from PlayerPrefs if values not set then set them to standard values
	void InitValues()
	{
		if (PlayerPrefs.HasKey("vSyncEnabled"))
		{
			vSyncEnabled = Convert.ToBoolean(PlayerPrefs.GetString("vSyncEnabled"));
		}
		else
		{
			PlayerPrefs.SetString("vSyncEnabled", true.ToString());
		}

		if (PlayerPrefs.HasKey("fpsCapEnabled"))
		{
			fpsCapEnabled = Convert.ToBoolean(PlayerPrefs.GetString("fpsCapEnabled"));
		}
		else
		{
			PlayerPrefs.SetString("fpsCapEnabled", true.ToString());
		}

		if (PlayerPrefs.HasKey("fpsCap"))
		{
			fpsCap = PlayerPrefs.GetInt("fpsCap");
		}
		else
		{
			PlayerPrefs.SetFloat("fpsCap", 60);
		}

		if (PlayerPrefs.HasKey("showFps"))
		{
			showFps = Convert.ToBoolean(PlayerPrefs.GetString("showFps"));
		}
		else
		{
			PlayerPrefs.SetString("showFps", true.ToString());
			showFps = true;
		}

		Application.targetFrameRate = vSyncEnabled ? maxScreenFramerate :
										fpsCapEnabled ? fpsCap : -1;
	}
}
