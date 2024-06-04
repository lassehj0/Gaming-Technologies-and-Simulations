using UnityEngine;

public class PlayerMoney : MonoBehaviour
{
	[SerializeField]
	Object[] coinsPrefabs;

	[HideInInspector]
	public int coins = 0;

	void Start()
	{
		coins = PlayerInfo.money;
	}

	void OnTriggerEnter2D(Collider2D collision)
	{
		// Determines how much money to gain based on which coin is collided with
		switch (collision.name)
		{
			case string name when name.Contains("BronzeCoin"):
				coins += 1;
				Destroy(collision.gameObject);
				break;
			case string name when name.Contains("SilverCoin"):
				coins += 2;
				Destroy(collision.gameObject);
				break;
			case string name when name.Contains("GoldCoin"):
				coins += 4;
				Destroy(collision.gameObject);
				break;
		}
	}

	// Creates a coin based on info given
	public void CreateCoin(Vector2 pos, int rarity = 0, int scaling = 1)
	{
		var coin = Instantiate(coinsPrefabs[rarity], pos, transform.rotation) as GameObject;
		coin.transform.localScale = new Vector2(scaling, scaling);
		coin.layer = 2;

		coin.GetComponent<Animator>().SetInteger("rarity", rarity);
	}

	void OnDestroy()
	{
		PlayerInfo.money = coins;
	}
}
