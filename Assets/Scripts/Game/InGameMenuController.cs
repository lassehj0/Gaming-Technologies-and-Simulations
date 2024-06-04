using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InGameMenuController : MonoBehaviour
{
	[SerializeField]
	Canvas settingsCnvs;
	[SerializeField]
	Button[] buttons;

	// Controllers with save and load functionality
	[SerializeField]
	PlayerController player;
	[SerializeField]
	TilemapController grid;
	[SerializeField]
	LeverController lever;
	[SerializeField]
	CanvasController canvas;

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape)) Continue();
	}

	// Start game when exiting in game menu
	public void Continue()
	{
		Time.timeScale = 1;
		gameObject.SetActive(false);
		if (settingsCnvs.enabled)
			settingsCnvs.gameObject.SetActive(false);
	}

	// Opens settings canvas
	public void OpenSettings()
	{
		settingsCnvs.gameObject.SetActive(true);
	}

	// Goes to the main menu scene
	public void GoToMainMenu()
	{
		Time.timeScale = 1;
		SceneManager.LoadScene("MainMenu");
	}

	public void OpenDialog(string dialog) => HandleDialog(dialog, true);
	public void CloseDialog(string dialog) => HandleDialog(dialog, false);

	// Calls all of the saving functionality in the different objects
	public void SaveGame()
	{
		player.SaveData();
		grid.SaveData();
		lever.SaveData();
		canvas.SaveData();

		HandleDialog("SaveDialog", false);
	}

	// Calls all of the loading functionality in the different objects
	public void LoadGame()
	{
		if (PlayerPrefs.HasKey("yVelocity"))
		{
			player.LoadData();
			grid.LoadData();
			lever.LoadData();
			canvas.LoadData();

			HandleDialog("LoadDialog", false);
		}
		else
		{
			HandleDialog("LoadDialog", false);
			HandleDialog("NoSaveDialog", true);
		}
	}

	public void NoSaveData() => HandleDialog("NoSaveDialog", false);

	// Opens or closes a specified dialog window
	void HandleDialog(string dialog, bool open)
	{
		foreach (var button in buttons) button.interactable = !open;
		var saveDlg = transform.Find(dialog).gameObject;
		saveDlg.SetActive(open);
	}
}
