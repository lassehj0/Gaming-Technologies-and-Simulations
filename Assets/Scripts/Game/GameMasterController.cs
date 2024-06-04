using UnityEngine;

public class GameMasterController : MonoBehaviour
{
	[SerializeField]
	Canvas menu;

	void Start()
	{
		if (PlayerPrefs.GetString("loadGame") == true.ToString())
		{
			// Load in saved data
			menu.GetComponent<InGameMenuController>().LoadGame();
			PlayerPrefs.SetString("loadGame", false.ToString());
		}
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			// Open menu and pause game
			menu.gameObject.SetActive(true);
			Time.timeScale = 0;
		}
	}
}
