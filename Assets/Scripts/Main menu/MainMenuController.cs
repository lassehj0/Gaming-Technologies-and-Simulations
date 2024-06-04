using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
	[SerializeField]
	Button newGameBtn, continueBtn, settingsBtn, exitBtn;
	[SerializeField]
	Canvas settings;

	public void StartNewGame()
	{
		PlayerPrefs.SetString("loadGame", false.ToString());
		SceneManager.LoadScene("Start");
	}

	// Start game with loaded data
	// This is not fully functional since not all values are currently being saved
	// This was developed in the beggining and then not prioritized
	// Does work but not all things are being loaded and saved
	public void ContinueGame()
	{
		if (PlayerPrefs.HasKey("yVelocity"))
		{
			PlayerPrefs.SetString("loadGame", true.ToString());
			SceneManager.LoadScene("Start");
		}
	}

	// Activate setting canvas and disable menu canvas
	public void OpenSettings()
	{
		gameObject.SetActive(false);
		settings.gameObject.SetActive(true);
	}

	public void Exit()
	{
		Application.Quit();
	}
}
