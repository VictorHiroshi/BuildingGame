using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchInputManager : InputManager {


	public override bool IsMovingClick ()
	{
		return Input.touchCount == 1;
	}

	public override bool IsZooming ()
	{
		return Input.touchCount >= 2;
	}

	public override void Zoom(Camera m_camera, float zoomSpeed)
	{
		Touch touchZero = Input.GetTouch(0);
		Touch touchOne = Input.GetTouch(1);

		Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
		Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

		float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
		float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

		float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

		m_camera.orthographicSize += deltaMagnitudeDiff * zoomSpeed * Time.deltaTime;
	}

	public override Vector3 MovingRange ()
	{
		return Input.GetTouch (0).deltaPosition;
	}
}
