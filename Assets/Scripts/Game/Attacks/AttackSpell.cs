using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AttackSpell : MonoBehaviour
{
	public int damage;
	List<char> keys;

	void Start()
	{
		Transform player = transform.parent.transform;
		transform.position = player.position;

		var dir = GetDirection(player);

		// Set rotation based on direction
		float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.Euler(0, 0, angle - 90);

		// Fire the spell based on direction
		var rigid = GetComponent<Rigidbody2D>();
		rigid.rotation = transform.rotation.z;
		rigid.AddForce(0.04f * dir);
	}

	void Update()
	{
		// Destroy self if out of bounds
		Vector2 pos = transform.position;
		if (pos.x < -100 || pos.x > 200 || pos.y < -10 || pos.y > 30) Destroy(gameObject);
	}

	void OnTriggerEnter2D(Collider2D collider)
	{
		bool safeLayerOrEnemy = collider.gameObject.layer != 2 || collider.CompareTag("Enemy");

		// Deal damage to enemy
		if (collider.CompareTag("Enemy")) collider.GetComponent<EnemyHealth>().TakeDamage(damage);

		// Destroy self if collision is not with player or IgnoreRaycast layer except if enemy
		if (!collider.CompareTag("Player") && safeLayerOrEnemy) Destroy(gameObject);
	}

	// Gets the direction the spell is being fired
	Vector2 GetDirection(Transform player)
	{
		Vector2 dir = new(0, 0);

		// If no direction keys is pressed set the direction to the direction the player is facing
		if (!keys.Any(key => key is 'a' or 's' or 'd' or 'w'))
		{
			dir.x = player.GetComponent<SpriteRenderer>().flipX ? -1 : 1;
		}
		// Otherwise set the direction based on the keys pressed
		else
		{
			if (keys.Contains('a')) dir.x -= 1;
			if (keys.Contains('d')) dir.x += 1;
			if (keys.Contains('s')) dir.y -= 1;
			if (keys.Contains('w')) dir.y += 1;
		}

		// Reverse directions if player is rotated and return
		return player.eulerAngles.z != 0 ? -dir : dir;
	}

	public void SetKeys(List<char> keys) => this.keys = keys;
}
