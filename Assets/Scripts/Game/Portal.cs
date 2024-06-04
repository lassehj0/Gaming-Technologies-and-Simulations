using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
	[SerializeField]
	CanvasController canvas;

	readonly string[] cantEnterYet = new string[2]
	{
		"You can't enter yet.",
		"Talk to the paladin to get your first spell."
	};

	[HideInInspector]
	public bool beginnersRoad;

	bool contactWithPlayer;

	void Update()
	{
		if (contactWithPlayer && (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.UpArrow)))
		{
			switch (transform.name)
			{
				case "BeginnersRoad":
					// Check if player can enter BeginnersRoad scene and load scene if able
					if (beginnersRoad) SceneManager.LoadScene("BeginnersRoad");
					else StartCoroutine(canvas.StartText(cantEnterYet));
					break;
				case "Start":
					SceneManager.LoadScene("Start");
					break;
			}
		}
	}

	void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
			contactWithPlayer = true;
	}

	void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
			contactWithPlayer = false;
	}
}
