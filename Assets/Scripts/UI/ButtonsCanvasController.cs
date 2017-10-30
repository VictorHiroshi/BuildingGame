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

	public Canvas BuildingButtonsCanvasObject;

	private List <BuildingCost> buildings;

	void Awake()
	{
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
		BuildingButtonsCanvasObject.enabled = false;
	}

	public void Show()
	{
		BuildingButtonsCanvasObject.enabled = true;
	}

	public void CheckButtons(int availableCoins)
	{
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
