using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Boundary {
	public float xMin;
	public float xMax;
	public float yMin;
	public float yMax;

	public void IncrementBoundary(float value)
	{
		xMin -= value*2;
		xMax += value*2;
		yMin -= value;
		yMax += value;
	}
}

public class CameraController : MonoBehaviour {

	public float cameraSpeed = 2.0f;
	public float zoomSpeed = 0.5f;
	public float maxOrtographicSize = 5.5f;
	public float minOrtographicSize = 3.0f;
	public Boundary cameraBoundaries;

	private Camera m_camera;

	void Awake()
	{
		m_camera = GetComponentInChildren<Camera> ();

		if (m_camera == null) {
			Debug.LogError ("No camera attached to cameraRig");
			Debug.Break ();
		}

	}

	void Update()
	{
		if(Input.touchCount == 2)
		{
			PinchToZoom ();
		}
	}

	public void PinchToZoom()
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
