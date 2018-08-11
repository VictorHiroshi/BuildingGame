using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InputManager : ScriptableObject {

	public abstract bool IsMovingClick ();
	public abstract bool IsZooming ();
	public abstract void Zoom (Camera m_camera, float zoomSpeed);
	public abstract Vector3 MovingRange ();
}
