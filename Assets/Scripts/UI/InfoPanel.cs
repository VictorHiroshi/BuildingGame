using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoPanel : MonoBehaviour {
	
	public Text message;
	public Image buildingImage;
	public GameObject infoPanel;
	public GameObject rayCastBlocker;


	public void HidePanel()
	{
		if(infoPanel.activeInHierarchy)
			infoPanel.SetActive (false);
		if(rayCastBlocker.activeInHierarchy)
			rayCastBlocker.SetActive (false);
	}

	public void ShowPanel()
	{
		infoPanel.SetActive (true);
		rayCastBlocker.SetActive (true);
	}

	public void DisplayInfo(BuildingController building)
	{
		ShowPanel ();

		buildingImage.sprite = building.buildingImage.sprite;



		string newMessage = string.Format ("This is a {0}. It costs {1} coin{2}. ", 
			                    building.name, 
			                    building.cost, 
			                    (building.cost > 1 ? "s" : ""));

		newMessage += string.Format ("When finished this building generates {0} coin{1} every {2} second{3}. ",
								building.income,
								building.income > 1 ? "s" : "",
								building.timeToGenerateCoins,
								(building.timeToGenerateCoins > 1 ? "s" : ""));

		newMessage += string.Format ("It takes {0} second{1} to be done.",
								building.timeToBuild,
								(building.timeToBuild > 1 ? "s" : ""));

		message.text = newMessage;
	}
}
