using UnityEngine;

public class BounceController : MonoBehaviour
{
	Rigidbody2D rigid;
	PlayerMovement playerMovement;

	void Start()
	{
		GameObject player = GameObject.Find("Player");
		rigid = player.GetComponent<Rigidbody2D>();
		playerMovement = player.GetComponent<PlayerMovement>();
	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		var otherObj = collision.gameObject;
		var dir = otherObj.transform.rotation.z == 0 ? Vector2.up : Vector2.down;

		// If player is falling down on object adjusted for rotation of player
		if (otherObj.CompareTag("Player") && collision.relativeVelocity.y * dir.y < -3)
		{
			// Bounce higher if player is also jumping
			var preparedToJump = playerMovement.preparedToJump;
			rigid.AddForce((preparedToJump ? 280 : 180) * dir);
		}
	}
}
