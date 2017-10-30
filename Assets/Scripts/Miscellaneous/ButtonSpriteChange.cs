using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSpriteChange : MonoBehaviour {

	public Image buttonImage;

	public Sprite spriteNotClicked;
	public Sprite spriteClicked;

	private bool clicked = false;

	public void Click()
	{
		clicked = !clicked;

		if(clicked)
		{
			buttonImage.sprite = spriteClicked;
		}
		else
		{
			buttonImage.sprite = spriteNotClicked;
		}
	}

}
