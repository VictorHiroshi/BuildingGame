using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonsCanvasController : MonoBehaviour {

	public Canvas BuildingButtonsCanvasObject;

	public Color buttonAllowedColor;
	public Color buttonForbidenColor;

	private List <ButtonClick> buttons;

	void Awake()
	{
		buttons = new List<ButtonClick> ();
		foreach(ButtonClick buttonScript in GetComponentsInChildren <ButtonClick> ())
		{
			buttons.Add (buttonScript);
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
