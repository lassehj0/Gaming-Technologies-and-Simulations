using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CanvasController : MonoBehaviour
{
	[SerializeField]
	TextMeshPro textField;

	List<string> remainingTexts;
	readonly float textDelay = 0.05f;
	readonly float delayBetweenTexts = 0.6f;

	void Start()
	{
		// If it is the start scene then show start text
		if (SceneManager.GetActiveScene().name == "Start")
		{
			StartCoroutine(StartText(
				"You can use the arrow keys to move around.",
				"Use E to interact with characters and objects."
			));
		}
		remainingTexts = new List<string>();
	}

	public void SaveData()
	{
		var stringToSave = "";
		foreach (string text in remainingTexts)
			stringToSave += text + ";";

		// The texts that hasn't been showed when game is saved
		PlayerPrefs.SetString("remainingMessages", stringToSave);
	}

	public void LoadData()
	{
		StopAllCoroutines();
		var messagesList = PlayerPrefs.GetString("remainingMessages").Split(";");
		StartCoroutine(StartText(messagesList));
	}

	// Shows text on the screen with a typing effect
	public IEnumerator StartText(params string[] texts)
	{
		remainingTexts = texts.ToList();
		foreach (string text in texts)
		{
			textField.text = "";
			foreach (char letter in text)
			{
				textField.text += letter;
				yield return new WaitForSeconds(textDelay);
			}
			yield return new WaitForSeconds(delayBetweenTexts);
			remainingTexts.Remove(text);
		}

		textField.text = "";
	}
}
