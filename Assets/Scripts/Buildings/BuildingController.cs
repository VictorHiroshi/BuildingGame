﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingController : MonoBehaviour {

	public float timeToGenerateCoins = 30f;
	public float timeToBuild = 15f;
	public int income = 10;
	public int cost = 1;
	public SpriteRenderer cantBuildImage;
	public Slider buildingProgressBar;

	private bool canBuild;
	private IEnumerator routine;
	private float timeCount;

	void Awake()
	{
		cantBuildImage.enabled = false;
		buildingProgressBar.gameObject.SetActive (false);
		canBuild = true;
	}

	public void OnTriggerEnter2D(Collider2D other)
	{
		if(other.gameObject.tag == "Building")
		{
			canBuild = false;
			cantBuildImage.enabled = true;
		}
	}

	public void OnTriggerExit2D(Collider2D other)
	{
		if(other.gameObject.tag == "Building")
		{
			canBuild = true;
			cantBuildImage.enabled = false;
		}
	}

	public void Build()
	{
		if(canBuild)
		{
			cantBuildImage.gameObject.SetActive (false);
			GameController.instance.Spend (cost);
			timeCount = 0f;

			routine = Constructing();

			StartCoroutine (routine);
		}
		else
		{
			Destroy (gameObject);
		}
	}

	private IEnumerator Constructing()
	{
		buildingProgressBar.gameObject.SetActive (true);

		while(timeCount < timeToBuild)
		{
			timeCount += Time.deltaTime;

			buildingProgressBar.value = timeCount / timeToBuild;

			yield return null;

		}

		buildingProgressBar.gameObject.SetActive (false);

		routine = GenerateMoney ();
		StartCoroutine (routine);
	}

	private IEnumerator GenerateMoney()
	{
		yield return null;

		while(true)
		{
			timeCount += Time.deltaTime;

			if(timeCount > timeToGenerateCoins)
			{
				timeCount -= timeToGenerateCoins;
				GameController.instance.Receive (income);
				Debug.Log ("Receive: " + income);

			}

			yield return null;
		}
	}
}
