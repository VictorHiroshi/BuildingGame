﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public enum BuildingType {Houses, Factory, Mall, Park, Farm};

public class GameController : MonoBehaviour {

	public int throwableCoinCost = 5;
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
	public GameObject monsterPrefab;
	public float timeBetweenMonsterSpawn;


	[HideInInspector] public bool showInfo;
	[HideInInspector] public bool cancelPlacement;
	[HideInInspector] public CameraController cameraScript;
	[HideInInspector] public int buildingCount;

	private int wallet = 0;
	private bool isPaused;
	private bool showedMonsterMessage;
	private bool isMonsterActive;
	private bool gameOver;
	private ButtonsCanvasController buttonCanvas;
	private InputManager inputManager;
	private WaitForSeconds monsterSpawnDelay;
	private IEnumerator monsterRoutine;
	private IEnumerator buildingRoutine;
	private BuildingController buildingInstance;

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

		monsterSpawnDelay = new WaitForSeconds (timeBetweenMonsterSpawn);
		showedMonsterMessage = false;
		isMonsterActive = false;
		buildingCount = 0;
		gameOver = false;
	}

	void Update () {
		if (Input.GetKeyUp(KeyCode.Escape))
		{
			Application.Quit();
		}

		if (buildingCount <= 0 && isMonsterActive && !CanThrowCoins() && !gameOver)
		{
			gameOver = true;
			infoPanel.GameOverMessage ();
		}
	}

	public void RestartGame()
	{
		SceneManager.LoadScene (0);
	}

	public bool CanThrowCoins()
	{
		return wallet >= throwableCoinCost;
	}

	public void Logged(string nickName, int coins)
	{
		userName.text = nickName;
		Receive (coins);
		StartCoroutine (ManageMonsterSpawn ());
	}

	public void Pause()
	{
		isPaused = !isPaused;

		if (isPaused) 
		{
			Time.timeScale = 0;

			musicAudioSource.Pause ();

			if (buildingInstance != null) 
			{
				Destroy (buildingInstance.gameObject);
				cameraScript.SetCanMoveTo (true);
				buttonCanvas.PlacingBuildingPanel (false);
				StopCoroutine (buildingRoutine);
			}

			buttonCanvas.Hide ();

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

		buildingInstance = instance.GetComponent <BuildingController> ();

		cancelPlacement = false;
		showInfo = false;

		buildingRoutine = PositionNewBuilding ();
		StartCoroutine (buildingRoutine);
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

	public void DefeatMonster()
	{
		StartCoroutine (ManageMonsterSpawn ());
		buttonCanvas.SetMonsterAttackPanelTo (false);
		isMonsterActive = false;
	}

	private IEnumerator PositionNewBuilding()
	{
		while(Input.GetMouseButton (0))
		{
			Vector3 worldPosition = Camera.main.ScreenToWorldPoint (Input.mousePosition);

			//Guarantees that the buildings most on top of the screen will be drawed behind.
			worldPosition.z = worldPosition.y;

			buildingInstance.gameObject.transform.position = worldPosition;

			yield return null;
		}

		if(showInfo)
		{
			infoPanel.DisplayInfo (buildingInstance);

			Destroy (buildingInstance.gameObject);
		}
		else if(cancelPlacement)
		{
			Destroy (buildingInstance.gameObject);
		}
		else
		{
			buildingInstance.Build ();
		}

		buildingInstance = null;
		cameraScript.SetCanMoveTo (true);
		buttonCanvas.PlacingBuildingPanel (false);
	}

	private IEnumerator ManageMonsterSpawn()
	{
		yield return monsterSpawnDelay;
		SpawnMonster ();

		yield return new WaitForSeconds (0.3f);
		buttonCanvas.SetMonsterAttackPanelTo (true);
	}

	private void SpawnMonster()
	{
		isMonsterActive = true;
		Instantiate (monsterPrefab);
		buttonCanvas.SetMonsterAttackPanelTo (true);

		if (!showedMonsterMessage) 
		{
			showedMonsterMessage = true;
			infoPanel.ShowMonsterInfo ();
			Pause ();
		}
	}
}
