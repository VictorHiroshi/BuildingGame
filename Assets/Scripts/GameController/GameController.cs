using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum BuildingType {Houses, Factory, Mall, Park, Farm};

public class GameController : MonoBehaviour {

	public bool touchInput = true;
	public static GameController instance;
	public Text userName;
	public Text coinCount;
	public Image coinTarget;
	public InfoPanel infoPanel;
	public float coinSpeed = 1.5f;
	public float delayBetweenCoinSpawn = 0.2f;
	public GameObject coinPrefab;
	public AudioSource coinAudioSource;
	public AudioSource musicAudioSource;

	[HideInInspector] public bool showInfo;
	[HideInInspector] public bool cancelPlacement;

	private int wallet = 0;
	private bool isPaused;

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

		userName.text = " ";
		coinCount.text = " ";

		if(buttonCanvas == null)
		{
			Debug.LogError ("No button canvas controller found in scene");
			Debug.Break ();
		}

		infoPanel.HidePanel ();

		isPaused = false;
	}

	void Update () {
		if (Input.GetKeyUp(KeyCode.Escape))
		{
			Application.Quit();
		}
	}

	public void Logged(string nickName, int coins)
	{
		userName.text = nickName;
		Receive (coins);
	}

	public void Pause()
	{
		isPaused = !isPaused;

		if (isPaused) 
		{
			buttonCanvas.Hide ();

			Time.timeScale = 0;

			musicAudioSource.Pause ();
		} 
		else
		{
			buttonCanvas.Show ();

			Time.timeScale = 1;

			musicAudioSource.Play ();
		}
	}

	public void SwitchMusicPlayStatus()
	{
		if(musicAudioSource.isPlaying)
		{
			musicAudioSource.Pause ();
			coinAudioSource.volume = 0f;
		}
		else
		{
			musicAudioSource.Play ();
			coinAudioSource.volume = 1f;
		}
	}

	public void BuildNew(GameObject building)
	{
		cameraScript.SetCanMoveTo (false);
		buttonCanvas.PlacingBuildingPanel (true);

		if (!Input.GetMouseButton (0)) 
		{
			cameraScript.SetCanMoveTo (true);
			buttonCanvas.PlacingBuildingPanel (false);
			return;
		}
		
		Vector3 clickPosition = Input.mousePosition;

		GameObject instance = Instantiate (building, clickPosition, Quaternion.identity);

		BuildingController buildingController = instance.GetComponent <BuildingController> ();

		cancelPlacement = false;
		showInfo = false;

		StartCoroutine (PositionNewBuilding (instance, buildingController));
	}

	public void Spend(int coins)
	{
		wallet -= coins;
		buttonCanvas.CheckButtons (wallet);
		coinCount.text = wallet.ToString ();

		coinAudioSource.Play ();
	}

	public void Receive(int coins)
	{
		wallet += coins;
		buttonCanvas.CheckButtons (wallet);
		coinCount.text = wallet.ToString ();

		coinAudioSource.Play ();
	}

	public Vector3 GetCoinPosition()
	{
		return Camera.main.ScreenToWorldPoint (coinTarget.transform.position);
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

		if(showInfo)
		{
			infoPanel.DisplayInfo (buildingController);

			Destroy (buildingController.gameObject);
		}
		else if(cancelPlacement)
		{
			Destroy (buildingController.gameObject);
		}
		else
		{
			buildingController.Build ();
		}

		cameraScript.SetCanMoveTo (true);
		buttonCanvas.PlacingBuildingPanel (false);
	}
}
