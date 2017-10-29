using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingController : MonoBehaviour {

	public float timeToGenerateCoins = 30f;
	public int income = 10;
	public int cost = 1;
	public SpriteRenderer cantBuildImage;

	private bool canBuild;

	void Awake()
	{
		cantBuildImage.enabled = false;
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
		}
		else
		{
			Destroy (gameObject);
		}
	}
}
