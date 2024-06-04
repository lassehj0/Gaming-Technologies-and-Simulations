using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	[HideInInspector]
	public bool preparedToJump;

	readonly float speed = 4.0f;
	bool standingOnBouncy;
	Vector2 playerVelocity;
	Rigidbody2D rigid;
	Animator animator;
	new SpriteRenderer renderer;
	PlayerController controller;

	//Directions
	Vector2 down = Vector2.down;
	Vector2 up = Vector2.up;
	Vector2 left = Vector2.left;
	Vector2 right = Vector2.right;

	void Start()
	{
		animator = GetComponent<Animator>();
		renderer = GetComponent<SpriteRenderer>();
		rigid = GetComponent<Rigidbody2D>();
		controller = GetComponent<PlayerController>();
		controller.reverseDirections += SetDirections;
	}

	void Update()
	{
		SetMovement();
		SetRotation();

		animator.SetFloat("Speed", Mathf.Abs(playerVelocity.x));
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		var relativeVelo = collision.relativeVelocity.y * up.y;

		// If player is on a bouncy surface, but not jumoing onto it, then set bool standingOnBouncy to true
		if (collision.collider.CompareTag("Bouncy") && relativeVelo < 1) standingOnBouncy = true;
		else standingOnBouncy = false;
	}

	void SetMovement()
	{
		if (Input.GetKeyDown(KeyCode.LeftArrow))
			playerVelocity += left * speed;
		if (Input.GetKeyUp(KeyCode.RightArrow))
			playerVelocity += left * speed;

		if (Input.GetKeyDown(KeyCode.RightArrow))
			playerVelocity += right * speed;
		if (Input.GetKeyUp(KeyCode.LeftArrow))
			playerVelocity += right * speed;

		// If the player is standing on the ground or a bouncy surface and tries to jump, jump
		if ((standingOnBouncy || controller.IsOnGround()) && (Input.GetKeyDown(KeyCode.UpArrow) || preparedToJump))
		{
			rigid.velocityY = 0;
			rigid.AddForce(up * 180);
			preparedToJump = false;
		}
		// Otherwise if player is not on the ground but tries to jump set bool preparedToJump to true and debounce it
		else if (Input.GetKeyDown(KeyCode.UpArrow) && !controller.IsOnGround() && !standingOnBouncy)
		{
			preparedToJump = true;
			StartCoroutine(DebounceCooldown(100));
		}

		rigid.velocityX = playerVelocity.x;
	}

	// Reverses directions this is triggered from an event in the character controller
	void SetDirections()
	{
		down = down == Vector2.down ? Vector2.up : Vector2.down;
		up = up == Vector2.up ? Vector2.down : Vector2.up;
		left = left == Vector2.left ? Vector2.right : Vector2.left;
		right = right == Vector2.right ? Vector2.left : Vector2.right;

		playerVelocity.x = -playerVelocity.x;
	}

	// Sets which way the sprite is facing
	void SetRotation()
	{
		if (playerVelocity.x * right.x < 0 && !renderer.flipX) renderer.flipX = true;
		else if (playerVelocity.x * right.x > 0 && renderer.flipX) renderer.flipX = false;
	}

	// When the player has been prepared to jump for a set amount of time
	// set player to not be prepared to jump
	IEnumerator DebounceCooldown(float cooldownMilliseconds)
	{
		yield return new WaitForSeconds(cooldownMilliseconds / 1000);
		preparedToJump = false;
	}
}
