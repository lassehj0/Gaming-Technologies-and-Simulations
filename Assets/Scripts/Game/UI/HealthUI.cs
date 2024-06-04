using System.Linq;
using UnityEngine;

public class HealthUI : MonoBehaviour
{
	[SerializeField]
	Sprite[] heartSprites;
	[SerializeField]
	Transform hearts, player;

	PlayerCombat playerCombat;
	int maxHealth = 0, health = 0;

	void Start()
	{
		playerCombat = player.GetComponent<PlayerCombat>();
	}

	void Update()
	{
		DrawHearts();
	}

	void DrawHearts()
	{
		var playerMaxHealth = playerCombat.maxHealth;
		var playerHealth = playerCombat.health;

		// If health or max health is changed
		if (playerMaxHealth != maxHealth || playerHealth != health)
		{
			maxHealth = playerMaxHealth;
			health = playerHealth;

			// Destroy all children to have a clean slate
			foreach (Transform child in hearts) Destroy(child.gameObject);

			var emptyHealth = playerMaxHealth - playerHealth;
			var xPos = 0;

			// Find out which heart sprites needs to be drawn and draw them
			while (playerHealth > 0 || emptyHealth > 0)
			{
				if (playerHealth > 1)
				{
					DrawHeart("FullHeart", xPos);
					playerHealth -= 2;
				}
				else if (playerHealth == 1 && emptyHealth >= 1)
				{
					DrawHeart("FullHeartHalfFull", xPos);
					playerHealth = 0;
					emptyHealth -= 1;
				}
				else if (playerHealth == 1)
				{
					DrawHeart("HalfHeart", xPos);
					playerHealth = 0;
				}
				else if (emptyHealth > 1)
				{
					DrawHeart("FullHeartEmpty", xPos);
					emptyHealth -= 2;
				}
				else if (emptyHealth == 1)
				{
					DrawHeart("HalfHeartEmpty", xPos);
					emptyHealth = 0;
				}

				xPos -= 30;
			}
		}
	}

	// Draw a heart sprite
	void DrawHeart(string spriteName, int xPos)
	{
		var heart = new GameObject(spriteName);

		// Set parent to the empty game object that contains the heart sprites
		// and set scale, rotation, and position relative to parent
		heart.transform.SetParent(hearts);
		heart.transform.localScale = new Vector2(0.8f, 0.8f);
		heart.transform.localRotation = Quaternion.identity;
		heart.transform.localPosition = new Vector3(xPos, 0, 0);

		// Add sprite
		var heartRenderer = heart.AddComponent<SpriteRenderer>();
		heartRenderer.sprite = heartSprites.ToList().First(sprite => sprite.name == spriteName);
	}
}
