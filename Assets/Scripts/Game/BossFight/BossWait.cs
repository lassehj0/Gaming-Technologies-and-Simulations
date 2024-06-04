using UnityEngine;

public class BossWait : StateMachineBehaviour
{
	// Distance the boss will try to keep when player is above
	public int bossDist = 6;

	// Height the player needs to have for the boss to run away
	float fearHeight;

	readonly int maxXPos = -50;
	Transform player;
	BossController controller;

	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		// Sets fear height to the top of the box collider
		fearHeight = animator.transform.position.y - animator.GetComponent<BoxCollider2D>().bounds.extents.y;

		controller = animator.GetComponent<BossController>();
		player = GameObject.FindGameObjectWithTag("Player").transform;
	}

	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		controller.FlipBoss(); // Make sure boss is facing the player

		var playerInRange = Mathf.Abs(player.position.x - animator.transform.position.x) < bossDist;
		if ((player.position.x < maxXPos && player.position.y > fearHeight) || playerInRange)
		{
			animator.SetBool("move", true);
		}
	}
}
