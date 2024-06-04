using System.Collections;
using UnityEngine;

public class TilemapController : MonoBehaviour
{
	[SerializeField]
	LeverController lever;
	[SerializeField]
	Transform purpleWorld;
	[SerializeField]
	GameObject reverseEnemies;

	Coroutine runningCoroutine;
	string movingDirection = "none";

	void Start()
	{
		lever.leverActivated += LeverActivated;
		lever.leverDeactivated += LeverDeactivated;

		if (SceneInfo.prevScene == "BeginnersRoad")
			purpleWorld.position += 21 * Vector3.down;
	}

	// Move purple tilemap down
	void LeverActivated()
	{
		if (runningCoroutine != null)
			StopCoroutine(runningCoroutine);

		var mapPos = purpleWorld.position;
		if (mapPos.y == 40) purpleWorld.position = mapPos + Vector3.down * 18;

		movingDirection = "down";
		var duration = Mathf.Abs(19 - purpleWorld.position.y) / 2;
		runningCoroutine = StartCoroutine(MoveTilemap(19, duration));
	}

	// Move purple tilemap up
	void LeverDeactivated()
	{
		if (runningCoroutine != null)
			StopCoroutine(runningCoroutine);

		movingDirection = "up";
		var duration = Mathf.Abs(22 - purpleWorld.position.y) / 2;
		runningCoroutine = StartCoroutine(MoveTilemap(22, duration));
	}

	public void SaveData()
	{
		PlayerPrefs.SetFloat("purpleTilemapY", purpleWorld.position.y);
		PlayerPrefs.SetString("movingDirection", movingDirection);
	}

	public void LoadData()
	{
		StopAllCoroutines();

		var xPos = purpleWorld.position.x;
		purpleWorld.position = new Vector3(xPos, PlayerPrefs.GetFloat("purpleTilemapY"), 0);

		var moveDir = PlayerPrefs.GetString("movingDirection");
		if (moveDir == "up") LeverDeactivated();
		else if (moveDir == "down") LeverActivated();
	}

	// Moves the purple tilemap to a new position
	IEnumerator MoveTilemap(float endY, float duration)
	{
		float time = 0;
		float shakeSpeed = 80.0f;
		float shakeAmount = .04f;
		Vector2 startMarker = purpleWorld.position;
		Vector2 endMarker = startMarker;
		endMarker.y = endY;

		// If tilemap is in a reachable position activate the enemies
		if (endY > 20) ActivateEnemies(false);

		while (time < duration)
		{
			// Move towards new position
			purpleWorld.position = Vector2.Lerp(startMarker, endMarker, time / duration);

			// Shake tilemap
			purpleWorld.position += new Vector3(Mathf.Sin(Time.time * shakeSpeed) * shakeAmount, 0, 0);

			time += Time.deltaTime;
			yield return null;
		}

		if (endY > 20) endMarker += Vector2.up * 18;
		else ActivateEnemies(true);

		purpleWorld.position = endMarker;
		movingDirection = "none";
		runningCoroutine = null;
	}

	void ActivateEnemies(bool activate)
	{
		foreach (Transform child in reverseEnemies.transform)
		{
			child.gameObject.SetActive(activate);
		}
	}
}
