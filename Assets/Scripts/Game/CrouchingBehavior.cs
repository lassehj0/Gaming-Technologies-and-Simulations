using UnityEngine;

public class CrouchingBehavior : StateMachineBehaviour
{
	// Sizes for crounching and uncrouching
	readonly Vector2 uncrouchSize = new() { x = 0.8141668f, y = 0.98f };
	readonly Vector2 uncrouchOffset = new() { x = -0.03135207f, y = 0 };
	readonly Vector2 crouchSize = new() { x = 0.6879458f, y = 0.5534936f };
	readonly Vector2 crouchOffset = new() { x = -0.03409603f, y = -0.2132544f };

	// Collider height for changing between horizontal and vertical direction
	readonly float changeDirHeight = 0.6879451f;

	float duration = 0.58333f / 5; // Same as animation duration
	float startHeight, changeDirTime, time;
	CapsuleCollider2D collider;
	bool isCrouching;

	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		collider = animator.GetComponent<CapsuleCollider2D>();
		startHeight = collider.size.y;

		// Checks whether the animation is playing forwards or backward
		// And therefore crouching or uncrouching
		isCrouching = stateInfo.speed > 0;

		var basePos = isCrouching ? uncrouchSize.y : crouchSize.y;
		var destPos = isCrouching ? crouchSize.y : uncrouchSize.y;

		// Calculates new timings based on how much the player needs to crouch/uncrouch
		duration *= Mathf.Abs(startHeight - destPos) / Mathf.Abs(basePos - destPos);
		changeDirTime = Mathf.Abs(changeDirHeight - destPos) / Mathf.Abs(basePos - destPos) * duration;
		time = 0;
	}

	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		time += Time.deltaTime;
		var currHeight = collider.size.y;

		if (isCrouching) Crouch(currHeight);
		else Uncrouch(currHeight);
	}

	void Crouch(float currHeight)
	{
		if ((time + Time.deltaTime >= changeDirTime && changeDirHeight < startHeight) || currHeight < changeDirHeight)
			collider.direction = CapsuleDirection2D.Horizontal;

		collider.size = Vector2.Lerp(uncrouchSize, crouchSize, time / duration);
		collider.offset = Vector2.Lerp(uncrouchOffset, crouchOffset, time / duration);
	}

	void Uncrouch(float currHeight)
	{
		if ((time + Time.deltaTime >= changeDirTime && changeDirHeight > startHeight) || currHeight > changeDirHeight)
			collider.direction = CapsuleDirection2D.Vertical;

		collider.size = Vector2.Lerp(crouchSize, uncrouchSize, time / duration);
		collider.offset = Vector2.Lerp(crouchOffset, uncrouchOffset, time / duration);
	}
}
