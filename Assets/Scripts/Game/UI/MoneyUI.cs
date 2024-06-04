using TMPro;
using UnityEngine;

public class MoneyUI : MonoBehaviour
{
	[SerializeField]
	SpriteRenderer coin;
	[SerializeField]
	TextMeshPro coinsText;
	[SerializeField]
	PlayerMoney playerMoney;

	int coins = 0;

	void Update()
	{
		DrawCoins();
	}

	void DrawCoins()
	{
		var playerCoins = playerMoney.coins;

		// Only do something if the amount of coins the player has changes
		if (coins != playerCoins)
		{
			coinsText.text = playerCoins.ToString();

			// If the lenght of coins changes, for example from 9 to 10, then change position of coin symbol
			if (coins.ToString().Length != playerCoins.ToString().Length)
			{
				coin.transform.localPosition = new Vector2(23f - playerCoins.ToString().Length * 16f, 0);
			}
			coins = playerCoins;
		}
	}
}
