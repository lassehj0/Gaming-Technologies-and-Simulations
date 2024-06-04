using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraController : MonoBehaviour
{
	[SerializeField]
	GameObject player;

	[HideInInspector]
	public float minXPos, maxXPos;

	readonly int maxYPos = 6;
	Vector3 velocity = Vector3.zero;
	readonly float smoothTime = 0.3f, displacement = 1;

	void Start()
	{
		// Setting minimum x position very low unless start scene
		if (SceneManager.GetActiveScene().name != "Start") minXPos = -120;
		else minXPos = -40;

		maxXPos = 200;

		Resources.UnloadUnusedAssets();
	}

	void FixedUpdate()
	{
		var playerPos = player.transform.position;
		var cameraPos = transform.position;

		// Setting camera y position to player position plus/minus displacement
		if (playerPos.y - cameraPos.y < -displacement && cameraPos.y >= -maxYPos) cameraPos.y = playerPos.y + displacement;
		else if (playerPos.y - cameraPos.y > displacement && cameraPos.y <= maxYPos) cameraPos.y = playerPos.y - displacement;

		// If camera is at the max or min x position stop it there
		if (playerPos.x <= minXPos) cameraPos.x = minXPos;
		else if (playerPos.x >= maxXPos) cameraPos.x = maxXPos;
		// Setting camera x position to player position plus/minus displacement
		else if (playerPos.x - cameraPos.x < -displacement) cameraPos.x = playerPos.x + displacement;
		else if (playerPos.x - cameraPos.x > displacement) cameraPos.x = playerPos.x - displacement;

		// Follow player rotation
		transform.eulerAngles = player.transform.eulerAngles;
		// Follow player position smoothly
		transform.position = Vector3.SmoothDamp(transform.position, cameraPos, ref velocity, smoothTime);
	}
}
