﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum BuildingType {Houses, Factory, Mall, Park, Farm};

public class GameController : MonoBehaviour {

	public bool touchInput = true;
	public static GameController instance;
	public int[] costsPerButton;
	public Text userName;
	public Text coinCount;

	private int wallet = 30;

	private CameraController cameraScript;
	private ButtonsCanvasController buttonCanvas;
	private InputManager inputManager;

	void Awake()
	{
		if(instance == null)
		{
			instance = this;
		}
		else
		{
			Destroy (this); 
		}
		/*if(Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
		{
			inputManager = new TouchInputManager ();
		}
		else
		{
			inputManager = new MouseInputManager ();
		}*/

		if(touchInput)
		{
			inputManager = new TouchInputManager ();
		}
		else
		{
			inputManager = new MouseInputManager ();
		}

		cameraScript = GameObject.FindObjectOfType<CameraController> ();

		if(cameraScript == null)
		{
			Debug.LogError ("No camera found in scene");
			Debug.Break ();
		}

		cameraScript.SetInputManager (inputManager);

		buttonCanvas = GameObject.FindObjectOfType<ButtonsCanvasController> ();

		if(buttonCanvas == null)
		{
			Debug.LogError ("No button canvas controller found in scene");
			Debug.Break ();
		}
	}

	void Start()
	{
		buttonCanvas.CheckButtons (wallet);
		coinCount.text = wallet.ToString ();
	}

	public void BuildNew(GameObject building)
	{
		cameraScript.SetCanMoveTo (false);
		buttonCanvas.Hide ();

		if (!Input.GetMouseButton (0)) 
		{
			cameraScript.SetCanMoveTo (true);
			buttonCanvas.Show ();
			return;
		}
		
		Vector3 clickPosition = Input.mousePosition;

		GameObject instance = Instantiate (building, clickPosition, Quaternion.identity);

		BuildingController buildingController = instance.GetComponent <BuildingController> ();

		StartCoroutine (PositionNewBuilding (instance, buildingController));
	}

	public void Spend(int coins)
	{
		wallet -= coins;
		buttonCanvas.CheckButtons (wallet);
		coinCount.text = wallet.ToString ();
	}

	public void Receive(int coins)
	{
		wallet += coins;
		buttonCanvas.CheckButtons (wallet);
		coinCount.text = wallet.ToString ();
	}

	private IEnumerator PositionNewBuilding(GameObject building, BuildingController buildingController)
	{
		while(Input.GetMouseButton (0))
		{
			Vector3 worldPosition = Camera.main.ScreenToWorldPoint (Input.mousePosition);

			//Guarantees that the buildings most on top of the screen will be drawed behind.
			worldPosition.z = worldPosition.y;

			building.transform.position = worldPosition;

			yield return null;
		}

		buildingController.Build ();

		cameraScript.SetCanMoveTo (true);
		buttonCanvas.Show ();
	}
}
