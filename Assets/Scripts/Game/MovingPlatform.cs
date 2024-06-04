using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
	[SerializeField]
	Transform start, end;
	[SerializeField]
	float duration;

	float time = 0;
	bool movingToEnd = true;
	Vector2 startPos, endPos;

	void Start()
	{
		startPos = start.position;
		endPos = end.position;
	}

	void FixedUpdate()
	{
		// When platform has reached one end switch ends
		if (time >= duration)
		{
			movingToEnd = !movingToEnd;
			startPos = movingToEnd ? start.position : end.position;
			endPos = movingToEnd ? end.position : start.position;
			time = 0;
		}

		// Smoothly move from start to end
		float timeStep = Mathf.SmoothStep(0.0f, 1.0f, time / duration);
		transform.position = Vector2.Lerp(startPos, endPos, timeStep);
		time += Time.fixedDeltaTime;
	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("Player") && collision.relativeVelocity.y < -1)
		{
			// Set player as child of platform
			collision.gameObject.transform.SetParent(transform);
		}
	}

	void OnCollisionExit2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("Player"))
		{
			// Unset player as child of platform
			collision.gameObject.transform.SetParent(null);
		}
	}
}
