using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[System.Serializable]
public class BuildingCost
{
	public GameObject button;
	public int cost;
}

public class ButtonsCanvasController : MonoBehaviour {

	private List <BuildingCost> buildings;
	private Canvas canvasObject;

	void Awake()
	{
		canvasObject = GetComponent <Canvas> ();
		buildings = new List<BuildingCost>();

		BuildingCost instanceBC;

		foreach(ButtonClick buttonScript in GetComponentsInChildren <ButtonClick> ())
		{
			instanceBC = new BuildingCost ();

			instanceBC.button = buttonScript.gameObject;
			instanceBC.cost = buttonScript.GetButtonCost ();

			buildings.Add (instanceBC);
		}
	}

	public void Hide()
	{
		canvasObject.enabled = false;
	}

	public void Show()
	{
		canvasObject.enabled = true;
	}

	public void CheckButtons(int availableCoins)
	{
		Debug.Log ("Coins: " + availableCoins);
		foreach(BuildingCost building in buildings)
		{
			if(building.cost > availableCoins)
			{
				building.button.SetActive (false);
			}
			else
			{
				building.button.SetActive (true);
			}
		}
	}
}
