using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoPanel : MonoBehaviour {
	
	public Text message;
	public Image infoImage;
	public GameObject infoPanel;
	public GameObject rayCastBlocker;

	private bool isMonsterInfo = false;
	private bool gameOver = false;
	int monsterInfoPart = 0;

	public void HidePanel()
	{
		if(infoPanel.activeInHierarchy)
			infoPanel.SetActive (false);
		if(rayCastBlocker.activeInHierarchy)
			rayCastBlocker.SetActive (false);

		if(isMonsterInfo)
		{
			if (monsterInfoPart != 2) 
			{
				ShowMonsterInfo ();
			}
			else 
			{
				isMonsterInfo = false;
				GameController.instance.Pause ();
			}
		}

		if(gameOver)
		{
			GameController.instance.RestartGame ();
		}
	}

	public void ShowPanel()
	{
		infoPanel.SetActive (true);
		rayCastBlocker.SetActive (true);
	}

	public void DisplayInfo(BuildingController building)
	{
		ShowPanel ();

		infoImage.sprite = building.buildingImage.sprite;



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

	public void ShowMonsterInfo()
	{
		isMonsterInfo = true;
		ShowPanel ();

		infoImage.sprite = GameController.instance.monsterPrefab.GetComponent <SpriteRenderer> ().sprite;

		if(monsterInfoPart == 0) 
		{
			message.text = "Beware! This is the coin-eater! He will destroy your buildings when his open mouth passes over them.";
			monsterInfoPart++;
		}
		else if(monsterInfoPart == 1)
		{
			message.text = " To stop him you must use the coin button bellow to throw coins in his mouth. But you can only hit his mouth" +
			" when it's open. Good luck!";
			monsterInfoPart++;
		}

	}

	public void GameOverMessage()
	{
		ShowPanel ();
		infoImage.sprite = GameController.instance.monsterPrefab.GetComponent <SpriteRenderer> ().sprite;

		message.text = "GameOver";
		gameOver = true;
	}
}
