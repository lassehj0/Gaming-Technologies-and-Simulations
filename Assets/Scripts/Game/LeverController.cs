using System;
using System.Collections;
using UnityEngine;

public class LeverController : MonoBehaviour
{
	[SerializeField]
	string[] activatedText;
	[SerializeField]
	GameObject pressBtn;
	[SerializeField]
	CanvasController canvas;

	public event Action leverActivated;
	public event Action leverDeactivated;

	Animator animator;
	bool canSwitch, isOff = true;
	readonly float displacement = 0.1f;
	readonly float duration = 0.7f;
	Vector2 pos1, pos2;

	void Start()
	{
		animator = GetComponent<Animator>();
		pos1 = pressBtn.transform.localPosition;
		pos2 = pos1 + Vector2.up * displacement;
		StartCoroutine(StartCanvas(activatedText));
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.E) && canSwitch)
		{
			isOff = !isOff;
			animator.SetBool("IsOff", isOff);

			// Trigger lever interaction event
			if (isOff) leverDeactivated?.Invoke();
			else leverActivated?.Invoke();
		}
	}

	void OnTriggerEnter2D(Collider2D collider)
	{
		if (collider.gameObject.name == "Player")
			canSwitch = true;
	}

	void OnTriggerExit2D(Collider2D collider)
	{
		if (collider.gameObject.name == "Player")
			canSwitch = false;
	}

	public void SaveData() => PlayerPrefs.SetString("isOff", isOff.ToString());

	public void LoadData()
	{
		isOff = Convert.ToBoolean(PlayerPrefs.GetString("isOff"));
		animator.SetBool("IsOff", isOff);
	}

	// Floats a sign above lever until lever is activated
	IEnumerator StartCanvas(params string[] texts)
	{
		float time = 0;
		bool goUp = true;

		// While the lever has not been activated keep the sign floating above lever
		while (isOff)
		{
			time += Time.deltaTime;
			pressBtn.transform.localPosition = Vector2.Lerp(pos1, pos2, time / duration);

			if (time >= duration)
			{
				time = 0;
				goUp = !goUp;
				pos1 += (goUp ? Vector2.down : Vector2.up) * displacement;
				pos2 += (goUp ? Vector2.up : Vector2.down) * displacement;
			}
			yield return null;
		}

		// Deactivate lever sign and start text in canvas
		pressBtn.SetActive(false);
		StartCoroutine(canvas.StartText(texts));
	}
}
