using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
	[SerializeField]
	Sprite btnUp, btnDown;

	new SpriteRenderer renderer;
	readonly float divisor = 22.5f;

	void Start()
	{
		renderer = GetComponent<SpriteRenderer>();
	}

	// When button is clicked change to click sprite and move text
	public void OnPointerDown(PointerEventData eventData)
	{
		renderer.sprite = btnDown;
		MoveText(Vector3.down);
	}

	// When button is unclicked change to unclick sprite and move text
	public void OnPointerUp(PointerEventData eventData)
	{
		renderer.sprite = btnUp;
		MoveText(Vector3.up);
	}

	// Moves the text to make the button click look better
	void MoveText(Vector3 dir)
	{
		foreach (Transform child in transform)
		{
			if (child.name != "Dummy")
			{
				gameObject.transform.GetChild(0).position += dir / divisor;
				break;
			}
		}
	}
}
