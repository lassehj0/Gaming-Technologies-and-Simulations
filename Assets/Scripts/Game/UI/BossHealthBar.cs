using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar : MonoBehaviour
{
	[SerializeField]
	Slider slider;
	[SerializeField]
	RectTransform redFill;

	// The time it takes for the healt bar value to change using Lerp
	readonly float duration = 0.25f;

	public void SetMaxHealth(int maxHealth, int health)
	{
		slider.maxValue = maxHealth;
		slider.value = health;
	}

	public void SetHealth(int health)
	{
		StartCoroutine(ChangeHealth(health));
	}

	// Changes the healt bar slider value using Lerp
	IEnumerator ChangeHealth(int health)
	{
		float time = 0, prevHealth = slider.value;

		while (time < duration)
		{
			slider.value = Mathf.Lerp(prevHealth, health, time / duration);
			time += Time.deltaTime;
			yield return null;
		}

		slider.value = health;
	}

	public void Enrage(int maxHealth, int health) => StartCoroutine(EnrageProcess(maxHealth, health));

	IEnumerator EnrageProcess(int maxHealth, int health)
	{
		// Waits for slider value to be 0 since it changes using Lerp
		while (slider.value != 0)
		{
			yield return null;
		}

		// Changes to the red healt bar and sets max health
		slider.fillRect = redFill;
		SetMaxHealth(maxHealth, health);
	}
}
