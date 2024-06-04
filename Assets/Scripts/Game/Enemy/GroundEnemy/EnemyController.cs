using UnityEngine;

public class EnemyController : MonoBehaviour
{
	[SerializeField]
	Transform player;

	Animator animator;
	Rigidbody2D rigid;
	EnemyHealth health;
	new SpriteRenderer renderer;
	float speed;
	bool canMove = true;

	void Start()
	{
		animator = GetComponent<Animator>();
		rigid = GetComponent<Rigidbody2D>();
		renderer = GetComponent<SpriteRenderer>();
		health = GetComponent<EnemyHealth>();
	}

	void Update()
	{
		SetMovement();
		SetAnimation();
		SetFlip();
	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		var collName = collision.gameObject.name;
		// If colliding with player or another enemy stop moving
		if (collName == "Player" || collName.Contains("Enemy"))
		{
			canMove = false;
			speed = 0;
			rigid.velocityX = 0;
		}

		var playerAbove = (collision.transform.position.y - transform.position.y) * Vector2.down.y > 0.7f;
		if (collision.collider.CompareTag("Player"))
		{
			// If the player jumps on enemy push player up and take damage
			if (collision.relativeVelocity.y > 3 && playerAbove)
			{
				collision.rigidbody.AddForce(Vector2.down * 150);
				health.TakeDamage(1);
			}
			// Otherwise start damaging player
			else player.GetComponent<PlayerCombat>().StartDamaging(1);
		}
	}

	void OnCollisionExit2D(Collision2D collision)
	{
		var collName = collision.gameObject.name;
		// If enemy no longer in contact with player or enemy it can move again
		if (collName == "Player" || collName.Contains("Enemy")) canMove = true;

		// If no longer in contact with player stop damaging player
		if (collision.collider.CompareTag("Player"))
		{
			player.GetComponent<PlayerCombat>().StopDamaging();
		}
	}

	void SetMovement()
	{
		if (canMove)
		{
			var distFromPlayer = Mathf.Abs(player.position.x - transform.position.x);
			var distFromPlayerY = Mathf.Abs(player.position.y - transform.position.y);
			bool isLeft = player.position.x - transform.position.x < 0;

			// If player is right beside enemy, out of range or above enemy stop moving
			if (distFromPlayer < 0.2f || distFromPlayer >= 8 || distFromPlayerY > 4)
			{
				speed = 0;
			}
			// Otherwise set speed based on which side the player is on
			else speed = isLeft ? -1.75f : 1.75f;

			rigid.velocityX = speed;
		}
	}

	void SetAnimation() => animator.SetFloat("Speed", Mathf.Abs(speed));

	// Set sprite to face the player
	void SetFlip() => renderer.flipX = rigid.gravityScale > 0 ? speed > 0 : speed < 0;
}
