// Based on the EnemyAI from the Brackeys tutorial
// on youtube https://www.youtube.com/watch?v=jvtFUfJ6CP8 
// with added graph update to change path based on moving objects

using Pathfinding;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
	[HideInInspector]
	public Transform target;

	[SerializeField]
	float speed = 2f, nextWaypointDistance = 3f;
	[SerializeField]
	GridGraph graph;

	Path path;
	int currentWaypoint = 0;
	Vector3 boundSize = Vector3.zero;

	Seeker seeker;
	Rigidbody2D rigid;
	new SpriteRenderer renderer;

	void Start()
	{
		seeker = GetComponent<Seeker>();
		rigid = GetComponent<Rigidbody2D>();
		renderer = GetComponent<SpriteRenderer>();

		// Covers the area in which the enemy should move
		boundSize = new Vector3(20, 20, 1);

		target = null;

		// Updates graph on start
		UpdateGraph();

		// Keep on updating path every .2 seconds
		InvokeRepeating(nameof(UpdatePath), 0, .2f);
	}

	void UpdatePath()
	{
		// Only do something if there is a target
		// and the current path is done calculating
		if (target != null && seeker.IsDone())
		{
			// If the path is blocked update the graph
			if (IsPathBlocked())
				UpdateGraph();

			// Calculate a path
			seeker.StartPath(rigid.position, target.position, OnPathComplete);
		}
	}

	// Callback after the path has been calculated
	void OnPathComplete(Path p)
	{
		// If there isn't an error with the calculated path
		// set the path to this and the current waypoint to one
		if (!p.error)
		{
			path = p;
			currentWaypoint = 1;
		}
	}

	void FixedUpdate()
	{
		// Only do anything if there is a target and a path
		if (target != null && path != null)
		{
			// Checks if the end of the path has been reached
			if (currentWaypoint >= path.vectorPath.Count) return;

			// Only add force if player is not right in front of enemy
			Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rigid.position).normalized;
			Vector2 force = speed * Time.fixedDeltaTime * direction;
			rigid.AddForce(force);

			// If the next waypoint has been reached increment currentWaypoint
			float distance = Vector2.Distance(rigid.position, path.vectorPath[currentWaypoint]);
			if (distance < nextWaypointDistance)
			{
				currentWaypoint++;
			}

			// Flip sprite to face player
			if (force.x >= 0.1f) renderer.flipX = true;
			else if (force.x <= -0.1f) renderer.flipX = false;
		}
	}

	// Checks if the path the enemy is trying to move is blocked
	bool IsPathBlocked()
	{
		// Makes sure a path exists
		if (path == null || path.vectorPath == null) return false;

		// Check for each vector point
		foreach (Vector2 point in path.vectorPath)
		{
			// If an object in the Obstacle layer is in the current vector point path is blocked
			if (Physics2D.OverlapCircle(point, 0.1f, LayerMask.GetMask("Obstacle")) != null)
			{
				return true;
			}
		}

		return false;
	}

	void UpdateGraph()
	{
		Bounds bounds = new(transform.position, boundSize);
		AstarPath.active.UpdateGraphs(bounds);
	}
}
