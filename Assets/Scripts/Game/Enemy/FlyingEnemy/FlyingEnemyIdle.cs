using UnityEngine;

public class FlyingEnemyIdle : StateMachineBehaviour
{
	[SerializeField]
	float maxDistance, attackRange, attackCooldown;

	Transform player = null;
	float time = 1;

	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		if (player == null)
			player = GameObject.Find("Player").transform;
	}

	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		float distance = Vector2.Distance(player.position, animator.transform.position);
		time += Time.deltaTime;

		// If player is in attack range and attack cooldown is over
		// reset time and trigger the attack
		if (distance <= attackRange && time >= attackCooldown)
		{
			time = 0;
			animator.SetTrigger("Attack");
		}
		// Otherwise if the player is not within attack range, start moving again
		else if (distance > attackRange && distance <= maxDistance)
		{
			animator.SetBool("CanAttack", false);
			animator.SetBool("PlayerInRange", true);
		}
	}
}
