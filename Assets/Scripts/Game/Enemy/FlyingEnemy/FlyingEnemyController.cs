using UnityEngine;

public class FlyingEnemyController : MonoBehaviour
{
	[SerializeField]
	int damage;

	Animator animator;
	PlayerCombat player;

	void Start()
	{
		player = GameObject.Find("Player").GetComponent<PlayerCombat>();
		animator = GetComponent<Animator>();
	}

	// Called when attack animation ended to wait to attack again
	public void AttackEnded() => animator.SetTrigger("WaitToAttack");

	// Called when the attack animation is in the attack frame and then deals damage
	public void DealDamage() => player.TakeDamage(damage);
}
