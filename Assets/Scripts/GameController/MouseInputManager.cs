using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

public class MouseInputManager : InputManager {

	private Vector3 mousePosition;

	public override bool IsMovingClick ()
	{
		return Input.GetMouseButton (0);
	}

	public override bool IsZooming ()
	{
		return Input.GetAxisRaw ("Mouse ScrollWheel") != 0;
	}

	public override void Zoom (Camera m_camera, float zoomSpeed)
	{
		//TODO: Zoom from scrollWheel.
		float modifier = Input.GetAxisRaw ("Mouse ScrollWheel") * zoomSpeed;

		if (modifier == 0)
			return;

		m_camera.orthographicSize += modifier;
	}

	public override Vector3 MovingRange ()
	{
		Vector3 range = Vector3.zero;

		if(Input.GetMouseButtonDown (0))
		{
			mousePosition = Input.mousePosition;
		}
		else
		{
			range = Input.mousePosition - mousePosition;
			mousePosition = Input.mousePosition;
		}

		return range;
	}

}
