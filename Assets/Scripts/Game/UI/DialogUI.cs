using System;
using System.Collections;
using TMPro;
using UnityEngine;
using static UnityEngine.Input;

public class DialogUI : MonoBehaviour
{
	[SerializeField]
	TextMeshPro textBox;

	public event Action done;

	[HideInInspector]
	public bool isRunning;
	readonly float textDelay = 0.075f;
	bool nextPressed = false;

	private void Update()
	{
		if (GetKeyDown(KeyCode.E) || GetKeyDown(KeyCode.Space)) nextPressed = true;
		else if (GetKeyUp(KeyCode.E) || GetKeyUp(KeyCode.Space)) nextPressed = false;
	}

	// Starts a dialog with the given text strings and calls a
	// callback function when done if defined
	public IEnumerator StartDialog(string[] dialogTexts, Func<int> func = null)
	{
		nextPressed = false;
		isRunning = true;
		SetActive(true);

		foreach (string text in dialogTexts)
		{
			textBox.text = "";
			foreach (char letter in text)
			{
				// Skips the letter by letter show if space is pressed
				if (nextPressed)
				{
					nextPressed = false;
					textBox.text = text;
					break;
				}
				textBox.text += letter;
				yield return new WaitForSeconds(textDelay);
			}

			// Does nothing until space is pressed and then moves on to the next string
			while (!nextPressed) yield return null;
			nextPressed = false;
		}

		SetActive(false);
		isRunning = false;
		done?.Invoke();

		// Call callback function if defined
		if (func != null) _ = func();
	}

	// Activates/deactivates all elements in the dialog
	void SetActive(bool active)
	{
		foreach (Transform child in transform)
		{
			child.gameObject.SetActive(active);
		}
	}
}
