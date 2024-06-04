using System;
using UnityEngine;

public class NpcController : MonoBehaviour
{
	[SerializeField]
	new CameraController camera;
	[SerializeField]
	GameObject dialogBox, FireBolt;
	[SerializeField]
	string[] dialogTexts, bossSlainDialog;
	[SerializeField]
	BossController bossController;
	[SerializeField]
	Portal portal;

	public event Action done;

	new SpriteRenderer renderer;
	DialogUI dialogUI;
	bool playerInContact, bossSlain;

	void Start()
	{
		renderer = GetComponent<SpriteRenderer>();
		bossController.bossFightDone += () => bossSlain = true;
		dialogUI = dialogBox.GetComponent<DialogUI>();
		dialogUI.done += Done;
	}

	void Update()
	{
		if (playerInContact && Input.GetKeyDown(KeyCode.E) && !dialogUI.isRunning)
		{
			if (!bossSlain)
			{
				// Start dialog telling player to slay boss
				StartCoroutine(dialogUI.StartDialog(dialogTexts));
			}
			else
			{
				// Start dialog after boss slain
				StartCoroutine(dialogUI.StartDialog(bossSlainDialog, () =>
				{
					// Set an attack spell for the player after dialog is finished
					GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCombat>().SetAttackSpell(FireBolt);
					portal.beginnersRoad = true;
					return 0;
				}));
			}
		}
	}

	void Done()
	{
		// First dialog. Set camera to be able to move to the boss room
		done?.Invoke();
		if (!bossSlain) camera.minXPos = -60;
	}

	void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.CompareTag("Player"))
			playerInContact = true;
	}

	void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.gameObject.CompareTag("Player"))
			playerInContact = false;
	}

	void OnTriggerStay2D(Collider2D collision)
	{
		// If collision is with player then flip to face the player
		if (collision.gameObject.CompareTag("Player"))
			renderer.flipX = collision.transform.position.x > transform.position.x;
	}
}
