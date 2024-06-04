using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Input;

public class PlayerCombat : MonoBehaviour
{
	[HideInInspector]
	public int maxHealth = 8, health = 8;

	Coroutine takingDamage = null;
	PlayerController controller;
	Object attackSpell = null;
	readonly List<char> keys = new();
	bool canAttack = true;

	//Directions
	Vector2 down = Vector2.down;
	Vector2 up = Vector2.up;
	Vector2 left = Vector2.left;
	Vector2 right = Vector2.right;

	void Start()
	{
		controller = GetComponent<PlayerController>();
		controller.reverseDirections += SetDirections;

		SetStartData();
	}

	void Update()
	{
		SetDirKeysDown();
		if (attackSpell != null && GetKeyDown(KeyCode.Space) && canAttack)
		{
			Attack();
			StartCoroutine(DebounceAttack());
		}
	}

	// Starts taking damage
	public void StartDamaging(int damage) =>
		takingDamage ??= StartCoroutine(StartTakingDamage(damage));

	// Take damage every 0.75 seconds until stopped
	IEnumerator StartTakingDamage(int damage)
	{
		while (true)
		{
			TakeDamage(damage);
			yield return new WaitForSeconds(0.75f);
		}
	}

	// Lose health equal to damage
	public void TakeDamage(int damage) => health -= damage;

	// Stop taking damage
	public void StopDamaging()
	{
		if (takingDamage != null)
		{
			StopCoroutine(takingDamage);
			takingDamage = null;
		}
	}

	// Set the attack spell
	public void SetAttackSpell(GameObject spell) => attackSpell = spell;

	// Attack by creating an attackSpell object
	void Attack()
	{
		GameObject attack = Instantiate(attackSpell) as GameObject;
		attack.transform.parent = transform;

		// Set the wasd keys being pressed in the spell which it uses
		// to determine in which direction to fire the attack spell
		attack.GetComponent<AttackSpell>().SetKeys(keys);
	}

	// Keep track of which wasd keys are pressed
	void SetDirKeysDown()
	{
		if (GetKeyDown(KeyCode.A)) keys.Add('a');
		if (GetKeyDown(KeyCode.S)) keys.Add('s');
		if (GetKeyDown(KeyCode.D)) keys.Add('d');
		if (GetKeyDown(KeyCode.W)) keys.Add('w');
		if (GetKeyUp(KeyCode.A)) keys.Remove('a');
		if (GetKeyUp(KeyCode.S)) keys.Remove('s');
		if (GetKeyUp(KeyCode.D)) keys.Remove('d');
		if (GetKeyUp(KeyCode.W)) keys.Remove('w');

		// If both a and d are pressed remove both since
		// these would indicate opposing directions
		if (keys.Contains('a') && keys.Contains('d'))
		{
			keys.Remove('a');
			keys.Remove('d');
		}

		// If both w and s are pressed remove both since
		// these would indicate opposing directions
		if (keys.Contains('w') && keys.Contains('s'))
		{
			keys.Remove('w');
			keys.Remove('s');
		}
	}

	// Cooldown until player can attack again
	IEnumerator DebounceAttack()
	{
		canAttack = false;
		yield return new WaitForSeconds(0.5f);
		canAttack = true;
	}

	// Sets new directions, this is used to have correct movement regardless of z rotation
	void SetDirections()
	{
		down = down == Vector2.down ? Vector2.up : Vector2.down;
		up = up == Vector2.up ? Vector2.down : Vector2.up;
		left = left == Vector2.left ? Vector2.right : Vector2.left;
		right = right == Vector2.right ? Vector2.left : Vector2.right;
	}

	// To set health and attack spell if they are set in static class
	void SetStartData()
	{
		maxHealth = PlayerInfo.maxHealth;
		health = maxHealth;

		if (PlayerInfo.attackSpell != null)
			SetAttackSpell(PlayerInfo.attackSpell);
	}

	// Set health and attack spell
	private void OnDestroy()
	{
		PlayerInfo.maxHealth = maxHealth;
		PlayerInfo.health = health;

		if (attackSpell != null)
			PlayerInfo.attackSpell = attackSpell as GameObject;
	}
}
