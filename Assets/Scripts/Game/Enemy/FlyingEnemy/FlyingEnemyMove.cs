using UnityEngine;

public class FlyingEnemyMove : StateMachineBehaviour
{
	[SerializeField]
	float maxDistance, attackRange;

	Transform player;

	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		player = GameObject.Find("Player").transform;

		// Set AI target to player so it starts moving
		animator.GetComponent<EnemyAI>().target = player;
	}

	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		float distance = Vector2.Distance(player.position, animator.transform.position);

		if (distance > maxDistance)
		{
			// Set player not in range and therefore stop moving
			animator.SetBool("PlayerInRange", false);
			animator.SetBool("CanAttack", false);
		}
		else if (distance <= attackRange)
		{
			// Player is in attack range, start attacking
			animator.SetBool("CanAttack", true);
			animator.SetBool("PlayerInRange", false);
		}
	}

	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		// No target for AI to keep it from moving
		animator.GetComponent<EnemyAI>().target = null;
	}
}
