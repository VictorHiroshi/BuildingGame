using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingController : MonoBehaviour {

	public float timeToGenerateCoins = 30f;
	public float timeToBuild = 15f;
	public int income = 10;
	public int cost = 1;
	public SpriteRenderer buildingImage;
	public SpriteRenderer cantBuildImage;
	public Slider buildingProgressBar;
	public ParticleSystem explosion;

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
			buildingImage.color = Color.red;

			canBuild = false;
			cantBuildImage.enabled = true;
		}
	}

	public void OnTriggerStay2D(Collider2D other)
	{
		if(other.gameObject.tag == "Building")
		{
			buildingImage.color = Color.red;

			canBuild = false;
			cantBuildImage.enabled = true;
		}
	}

	public void OnTriggerExit2D(Collider2D other)
	{
		if(other.gameObject.tag == "Building")
		{
			buildingImage.color = Color.white;

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

			buildingImage.color = Color.white;
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

		StartCoroutine (FinishBuilding());

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

				explosion.Play ();

				StartCoroutine (SpawnFlyingCoins (income));
			}

			yield return null;
		}
	}

	private IEnumerator FinishBuilding()
	{
		gameObject.transform.localScale += (Vector3.one * 0.2f);

		yield return new WaitForSeconds (0.2f);
		
		gameObject.transform.localScale -= (Vector3.one * 0.2f);
	}

	private IEnumerator SpawnFlyingCoins(int count)
	{
		WaitForSeconds delay = new WaitForSeconds (GameController.instance.delayBetweenCoinSpawn / count);

		for(int spawned = 0; spawned<count; spawned++)
		{
			Instantiate (GameController.instance.coinPrefab, gameObject.transform.position, Quaternion.identity);
			yield return delay;
		}

	}
}
