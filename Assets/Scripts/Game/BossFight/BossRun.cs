using UnityEngine;

public class BossRun : StateMachineBehaviour
{
	BossController controller;
	Transform player;
	Rigidbody2D rigid;

	float minXPos, maxXPos, fearHeight, distToWall;
	public int bossDist = 6; // The distance to keep from the player if player is above boss
	public float speed = 4;

	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		rigid = animator.GetComponent<Rigidbody2D>();
		controller = animator.GetComponent<BossController>();
		player = GameObject.FindGameObjectWithTag("Player").transform;

		Vector2 extents = animator.GetComponent<BoxCollider2D>().bounds.extents;
		distToWall = extents.x;
		fearHeight = rigid.position.y - extents.y;

		maxXPos = -50.1f - distToWall;
		minXPos = -69.9f + distToWall;
	}

	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		controller.FlipBoss(); // Make sure boss is facing the player

		Vector2 target = new(player.position.x, rigid.position.y);

		// If player is above boss
		if (player.position.y < fearHeight)
		{
			var playerPosX = player.position.x;

			// Move boss to the left or right based on relative player position
			// Try to move a certain distance away from the player but stop at wall
			if (playerPosX < rigid.position.x)
			{
				target.x = playerPosX + bossDist > maxXPos ? maxXPos : playerPosX + bossDist;
			}
			else
			{
				target.x = playerPosX - bossDist < minXPos ? minXPos : playerPosX - bossDist;
			}
		}

		// If the target position is more than 0.1 away move towards target position
		// Otherwise change away from move state
		if (Mathf.Abs(rigid.position.x - target.x) > 0.1f)
		{
			Vector2 newPos = Vector2.MoveTowards(rigid.position, target, speed * Time.fixedDeltaTime);
			rigid.MovePosition(newPos);
		}
		else animator.SetBool("move", false);
	}
}
