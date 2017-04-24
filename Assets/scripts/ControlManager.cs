using UnityEngine;
using System;
using System.Collections;

public enum GravityDirection {
	Up,
	Down,
	Right,
	Left
}

public class ControlManager : MonoBehaviour
{
	public static event Action<GravityDirection> Swipe;
	private bool swiping = false;
	private Vector2 lastPosition;
	private Vector2 initialPosition;

	void Update () {
		if (Input.touchCount == 0) {
			if (swiping) {
				if (Swipe != null) {
					Vector2 direction = initialPosition - lastPosition;
					if (direction.sqrMagnitude < 2000) {
						swiping = false;
						return;
					}
					if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y)) {
						if (direction.x < 0) {
							Debug.Log ("Swipe Right");
							Swipe (GravityDirection.Right);
						} else {
							Debug.Log ("Swipe Left");
							Swipe (GravityDirection.Left);
						}
					} else {
						if (direction.y < 0) {
							Debug.Log ("Swipe Up");
							Swipe (GravityDirection.Up);
						} else {
							Debug.Log ("Swipe Down");
							Swipe (GravityDirection.Down);
						}
					}
				}
				swiping = false;
			} else {
				return;
			}
		} else {
			if (swiping) {
				lastPosition = Input.GetTouch (0).position;
			} else {
				swiping = true;
				initialPosition = Input.GetTouch (0).position;
			}
		}
	}
}

