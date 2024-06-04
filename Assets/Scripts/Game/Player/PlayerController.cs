using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
	public event Action reverseDirections;

	//Components
	Rigidbody2D rigid;
	new SpriteRenderer renderer;
	Animator animator;
	PlayerCombat combat;
	PlayerMoney money;
	RaycastHit2D hit;

	//Values
	float distToGround;
	bool isOnGround, isRotating;

	//Directions
	Vector2 down = Vector2.down;
	Vector2 up = Vector2.up;
	Vector2 left = Vector2.left;
	Vector2 right = Vector2.right;

	void Start()
	{
		distToGround = GetComponent<Collider2D>().bounds.extents.y;
		rigid = GetComponent<Rigidbody2D>();
		renderer = GetComponent<SpriteRenderer>();
		animator = GetComponent<Animator>();
		combat = GetComponent<PlayerCombat>();
		money = GetComponent<PlayerMoney>();
	}

	void Update()
	{
		SetAnimatorVariables();
		CheckNearestPlatforms();
		CheckForDeath();
	}

	void CheckForDeath()
	{
		// Checks if the player is out of bounds or out of health
		if (transform.position.y >= 15 || transform.position.y <= -15 || combat.health <= 0)
		{
			// Decrease coins and reload scene
			money.coins = money.coins >= 2 ? money.coins - 2 : 0;
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}
	}

	// Sets the variables in the animator associated with the player
	void SetAnimatorVariables()
	{
		float velocity = up.y > 0 ? rigid.velocityY : -rigid.velocityY;

		animator.SetBool("TryCrouching", Input.GetKey(KeyCode.DownArrow));
		animator.SetBool("IsJumping", !isOnGround && velocity > 0);
		animator.SetBool("IsFalling", !isOnGround && velocity < 0);
	}

	// Saves player data
	public void SaveData()
	{
		PlayerPrefs.SetFloat("playerX", transform.position.x);
		PlayerPrefs.SetFloat("playerY", transform.position.y);
		PlayerPrefs.SetFloat("yVelocity", rigid.velocityY);
		PlayerPrefs.SetString("isRotated", rigid.gravityScale < 0 ? "True" : "False");
		PlayerPrefs.SetString("flipX", renderer.flipX.ToString());
	}

	// Loads player data
	public void LoadData()
	{
		// Sets data if player is upside down
		if (Convert.ToBoolean(PlayerPrefs.GetString("isRotated")))
		{
			rigid.gravityScale = -1;
			left = Vector2.right;
			right = Vector2.left;
			up = Vector2.down;
			down = Vector2.up;
			transform.rotation = new Quaternion(0, 0, 180, 0);
			reverseDirections?.Invoke();
		}

		transform.position = new Vector3(PlayerPrefs.GetFloat("playerX"), PlayerPrefs.GetFloat("playerY"), 0);
		renderer.flipX = Convert.ToBoolean(PlayerPrefs.GetString("flipX"));

		rigid.velocityY = PlayerPrefs.GetFloat("yVelocity");

		StopAllCoroutines();
	}

	// Checks above and below player to see if player should be rotated
	void CheckNearestPlatforms()
	{
		// Checks if a hit is made right below player
		hit = Physics2D.Raycast(transform.position, down, distToGround + 0.1f);

		// Determines if player is on the ground based on the hit
		isOnGround = hit.collider != null && hit.collider.CompareTag("Ground");

		if (!isOnGround && !isRotating)
		{
			var hitUp = Physics2D.Raycast(transform.position, up);
			var hitDown = Physics2D.Raycast(transform.position, down);

			bool shouldRotate = !isRotating && hitUp.collider != null;
			bool bothHit = hitUp.collider != null && hitDown.collider != null;
			bool upIsCloser = hitUp.distance < hitDown.distance;
			bool upIsOnlyOptionAndWithinRange = hitDown.collider == null && hitUp.distance < 5.1f;
			bool upIsDifferent = bothHit && hitUp.collider.name != hitDown.collider.name;

			bool shouldApplyRotation = shouldRotate && (upIsCloser || upIsOnlyOptionAndWithinRange) && upIsDifferent;

			// Draws the raycasts that are being made to determine if player should rotate
			DrawRaycasting(shouldApplyRotation);

			// If all the conditions are met then rotate the player
			if (shouldApplyRotation) StartCoroutine(RotatePlayer());
		}
	}

	// Rotate the player on the z axis to 180 degrees
	IEnumerator RotatePlayer()
	{
		isRotating = true;
		rigid.gravityScale = -rigid.gravityScale;
		var startRot = transform.rotation;
		var endRot = Quaternion.Euler(0, 0, startRot.z == 0 ? 180 : 0);
		float time = 0, duration = 0.3f;

		// Slerp from the current rotation to the end rotation over the specified duration
		while (time < duration)
		{
			transform.rotation = Quaternion.Slerp(startRot, endRot, time / duration);
			time += Time.deltaTime;
			yield return null;
		}

		transform.rotation = endRot;

		//Set directions
		down = down == Vector2.down ? Vector2.up : Vector2.down;
		up = up == Vector2.up ? Vector2.down : Vector2.up;
		left = left == Vector2.left ? Vector2.right : Vector2.left;
		right = right == Vector2.right ? Vector2.left : Vector2.right;

		// Invoke event so that other scripts know that player is rotated
		reverseDirections?.Invoke();
		isRotating = false;
	}

	// Draws the raycasts being made by this script
	void DrawRaycasting(bool shouldRotate)
	{
		if (!isOnGround)
		{
			var hitUp = Physics2D.Raycast(transform.position, up);
			var hitDown = Physics2D.Raycast(transform.position, down);
			var upDir = up * hitUp.distance;
			var downDir = down * hitDown.distance;

			if (shouldRotate)
			{
				Debug.DrawRay(transform.position, upDir, Color.green);
				Debug.DrawRay(transform.position, downDir, Color.red);
			}
			else
			{
				Debug.DrawRay(transform.position, upDir, Color.red);
				Debug.DrawRay(transform.position, downDir, Color.green);
			}
		}
		else
		{
			Debug.DrawRay(transform.position, down * hit.distance, Color.yellow);
		}
	}

	public bool IsOnGround() => isOnGround;
}
