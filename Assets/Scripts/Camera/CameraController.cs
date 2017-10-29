using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Boundary {
	public float xMin;
	public float xMax;
	public float yMin;
	public float yMax;
}

public class CameraController : MonoBehaviour {

	public bool invertControls = false;
	public float cameraSpeed = 2.0f;
	public float zoomSpeed = 0.5f;
	public float maxOrtographicSize = 5.5f;
	public float minOrtographicSize = 3.0f;
	public Boundary cameraBoundaries;

	private bool canMove;

	private Camera m_camera;

	void Awake()
	{
		m_camera = GetComponentInChildren<Camera> ();

		if (m_camera == null) {
			Debug.LogError ("No camera attached to cameraRig");
			Debug.Break ();
		}

		canMove = true;
	}

	void Update()
	{
		if (!canMove)
			return;
		
		if(Input.touchCount == 1)
		{
			MoveCameraRig ();
		}

		else if(Input.touchCount == 2)
		{
			PinchToZoom ();
		}
	}

	public void SetCanMoveTo(bool value)
	{
		canMove = value;
	}

	private void MoveCameraRig()
	{
		Touch touch = Input.GetTouch (0);

		Vector3 movement = touch.deltaPosition * cameraSpeed * Time.deltaTime;
		if(!invertControls)
		{
			movement.y *= -1;
			movement.x *= -1;
		}

		transform.position += movement;

		CheckBoundaries ();
	}

	private void PinchToZoom()
	{
		Touch touchZero = Input.GetTouch(0);
		Touch touchOne = Input.GetTouch(1);

		Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
		Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

		float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
		float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

		float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

		m_camera.orthographicSize += deltaMagnitudeDiff * zoomSpeed * Time.deltaTime;

		m_camera.orthographicSize = Mathf.Max (m_camera.orthographicSize, minOrtographicSize);
		m_camera.orthographicSize = Mathf.Min (m_camera.orthographicSize, maxOrtographicSize);
	}

	private void CheckBoundaries ()
	{
		Vector3 correctedPosition = transform.position;

		if(transform.position.x < cameraBoundaries.xMin){
			correctedPosition.x = cameraBoundaries.xMin;
		}
		if(transform.position.x > cameraBoundaries.xMax){
			correctedPosition.x = cameraBoundaries.xMax;
		}
		if(transform.position.y < cameraBoundaries.yMin){
			correctedPosition.y = cameraBoundaries.yMin;
		}
		if(transform.position.y > cameraBoundaries.yMax){
			correctedPosition.y = cameraBoundaries.yMax;
		}

		transform.position = correctedPosition;
	}
}
