using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BuildingType {Houses, Factory, Mall, Park, Farm};

public class GameController : MonoBehaviour {

	private CameraController cameraScript;
	private ButtonsCanvasController buttonCanvas;

	void Awake()
	{
		cameraScript = GameObject.FindObjectOfType<CameraController> ();

		if(cameraScript == null)
		{
			Debug.LogError ("No camera found in scene");
			Debug.Break ();
		}

		buttonCanvas = GameObject.FindObjectOfType<ButtonsCanvasController> ();

		if(buttonCanvas == null)
		{
			Debug.LogError ("No button canvas controller found in scene");
			Debug.Break ();
		}
	}

	public void BuildNew(GameObject building)
	{
		cameraScript.SetCanMoveTo (false);
		buttonCanvas.Hide ();

		if (Input.touchCount == 0) 
		{
			cameraScript.SetCanMoveTo (true);
			buttonCanvas.Show ();
			return;
		}
		
		Vector3 clickPosition = Input.GetTouch (0).position;

		GameObject instance = Instantiate (building, clickPosition, Quaternion.identity);

		StartCoroutine (PositionNewBuilding (instance));
	}

	private IEnumerator PositionNewBuilding(GameObject building)
	{
		while(Input.touchCount > 0)
		{
			Vector3 worldPosition = Camera.main.ScreenToWorldPoint (Input.GetTouch (0).position);

			//Guarantees that the buildings most on top of the screen will be drawed behind.
			worldPosition.z = worldPosition.y;

			building.transform.position = worldPosition;

			yield return null;
		}
			
		cameraScript.SetCanMoveTo (true);
		buttonCanvas.Show ();
	}
}
