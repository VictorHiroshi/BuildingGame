using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonsCanvasController : MonoBehaviour {

	public Canvas BuildingButtonsCanvasObject;

	public Color buttonAllowedColor;
	public Color buttonForbidenColor;

	private TrashButtonScript trashButton;
	private InfoButtonScript infoButton;
	private ThrowingCoinScript throwButton;
	private List <ButtonClick> buttons;

	void Awake()
	{
		buttons = new List<ButtonClick> ();
		foreach(ButtonClick buttonScript in GetComponentsInChildren <ButtonClick> ())
		{
			buttons.Add (buttonScript);
		}

		trashButton = GetComponentInChildren <TrashButtonScript> ();
		infoButton = GetComponentInChildren <InfoButtonScript> ();
		throwButton = GetComponentInChildren <ThrowingCoinScript> ();

		trashButton.gameObject.SetActive (false);
		infoButton.gameObject.SetActive (false);
		throwButton.gameObject.SetActive (false);

	}

	public void Hide()
	{
		BuildingButtonsCanvasObject.enabled = false;
	}

	public void Show()
	{
		BuildingButtonsCanvasObject.enabled = true;
	}

	public void PlacingBuildingPanel(bool value)
	{
		if(value)
		{
			foreach (ButtonClick button in buttons) 
			{
				button.gameObject.SetActive (false);
			}

			trashButton.gameObject.SetActive (true);
			infoButton.gameObject.SetActive (true);
		}
		else
		{
			trashButton.gameObject.SetActive (false);
			infoButton.gameObject.SetActive (false);

			foreach (ButtonClick button in buttons) 
			{
				button.gameObject.SetActive (true);
			}
		}
	}

	public void SetMonsterAttackPanelTo(bool value)
	{
		if(value)
		{
			trashButton.gameObject.SetActive (false);
			infoButton.gameObject.SetActive (false);

			foreach (ButtonClick button in buttons) 
			{
				button.gameObject.SetActive (false);
			}

			throwButton.gameObject.SetActive (true);
		}
		else
		{
			trashButton.gameObject.SetActive (false);
			infoButton.gameObject.SetActive (false);

			foreach (ButtonClick button in buttons) 
			{
				button.gameObject.SetActive (true);
			}

			throwButton.gameObject.SetActive (false);
		}
	}

	public void CheckButtons(int availableCoins)
	{
		foreach(ButtonClick button in buttons)
		{
			if(button.GetButtonCost() > availableCoins)
			{
				button.SetButtonTo (false, buttonForbidenColor);
			}
			else
			{
				button.SetButtonTo (true, buttonAllowedColor);
			}
		}
	}
}
