using System.Linq;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
	[SerializeField]
	PlayerMoney money;

	public int maxHealth = 4;
	AnimationClip[] clips;
	Animator animator;
	int health;

	void Start()
	{
		animator = GetComponent<Animator>();
		clips = animator.runtimeAnimatorController.animationClips;
		health = maxHealth;
	}

	public void TakeDamage(int damage)
	{
		health -= damage;
		if (health <= 0)
		{
			// If the enemy has a death animation trigger the Dead action
			if (ContainsAnimation("Death")) animator.SetTrigger("Dead");
			// Otherwise just destroy
			else Die();
		}
		// If the enemy is still alive after being damaged and has a Hurt
		// animation then trigger the TakeDamage action
		else if (ContainsAnimation("Hurt"))
		{
			animator.SetTrigger("TakeDamage");
		}
	}

	//Death animation is done
	public void IsDoneDying() => Die();

	//Checks if the animator has an animation with a specific name
	bool ContainsAnimation(string animationName) =>
		clips.Any(clip => clip.name == animationName);

	void Die()
	{
		// Gets the coin (bronze/silver/gold) rarity based on the size of this enemy
		int rarity = GetComponent<EnemyHealth>().maxHealth <= 2 ? 0 : 1;

		// Gets a slightly lower position and creates coin with that position
		Vector2 pos = (Vector2)transform.position + 0.1f * Vector2.up;
		money.CreateCoin(pos, rarity);

		Destroy(gameObject);
	}
}
