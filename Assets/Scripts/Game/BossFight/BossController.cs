using System;
using UnityEngine;

public class BossController : MonoBehaviour
{
	[SerializeField]
	new CameraController camera;
	[SerializeField]
	BossHealthBar healthBar;

	public event Action bossFightDone;

	public int damage = 1, maxHealth = 4, maxHealthEnraged = 6;
	Transform player;
	new SpriteRenderer renderer;
	Animator animator;
	float topDist; // Distance from center to top of boss
	int health;
	bool isEnraged;

	void Start()
	{
		animator = GetComponent<Animator>();
		topDist = GetComponent<BoxCollider2D>().bounds.extents.y;
		renderer = gameObject.GetComponent<SpriteRenderer>();
		player = GameObject.FindGameObjectWithTag("Player").transform;
		healthBar.SetMaxHealth(maxHealth, health = maxHealth);
	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		var playerAbove = collision.transform.position.y < transform.position.y - topDist;
		if (collision.collider.CompareTag("Player"))
		{
			// If player is above and is hitting boss with a downward force.
			// Checks if relative velocity is more than 3 instead of less than
			// -3 since it is upside down.
			if (collision.relativeVelocity.y > 3 && playerAbove)
			{
				collision.rigidbody.AddForce(Vector2.down * 200);
				TakeDamage();
			}
			// Starts damaging player until stopped
			else player.GetComponent<PlayerCombat>().StartDamaging(damage);
		}
	}

	void OnCollisionExit2D(Collision2D collision)
	{
		if (collision.collider.CompareTag("Player"))
			player.GetComponent<PlayerCombat>().StopDamaging(); // Stops damaging player
	}

	void TakeDamage()
	{
		healthBar.SetHealth(health -= 1);

		// Enrages if not already enraged othwise dies
		if (health <= 0 && !isEnraged) Enrage();
		else if (health <= 0 && isEnraged) Die();
	}

	void Enrage()
	{
		isEnraged = true;
		animator.SetBool("isEnraged", true);
		healthBar.Enrage(maxHealthEnraged, health = maxHealthEnraged);
	}

	void Die()
	{
		bossFightDone?.Invoke();
		camera.minXPos = -120;
		camera.maxXPos = 20;
		healthBar.gameObject.SetActive(false);
		Destroy(gameObject);
	}

	public void FlipBoss()
	{
		renderer.flipX = player.position.x < transform.position.x;
	}

	void OnDestroy()
	{
		// Create a coin
		player.GetComponent<PlayerMoney>().CreateCoin(transform.position, rarity: 2, scaling: 2);
	}
}
