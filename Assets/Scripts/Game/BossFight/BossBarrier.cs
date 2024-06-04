using UnityEngine;

public class BossBarrier : MonoBehaviour
{
	[SerializeField]
	new CameraController camera;
	[SerializeField]
	NpcController npc;
	[SerializeField]
	Transform player;
	[SerializeField]
	BossController boss;
	[SerializeField]
	GameObject healthBar;

	bool hasEnteredBossRoom;

	void Start()
	{
		npc.done += Done;
		boss.bossFightDone += () => SetActive(false);
	}

	void Update()
	{
		// If it is the first time the player enters the boss room
		if (!hasEnteredBossRoom && player.position.x < -51)
		{
			SetActive(true);
			healthBar.SetActive(true);
			hasEnteredBossRoom = true;
			camera.minXPos = -60;
			camera.maxXPos = -60;
		}
	}

	// When the first npc dialog is done
	void Done()
	{
		SetActive(false);
		boss.gameObject.SetActive(true);
		npc.done -= Done;
	}

	// Activates or disactivates the individual barriers around the boss room
	void SetActive(bool active)
	{
		foreach (Transform child in transform)
		{
			child.gameObject.SetActive(active);
		}
	}
}
