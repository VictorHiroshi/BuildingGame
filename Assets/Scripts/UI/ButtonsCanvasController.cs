using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonsCanvasController : MonoBehaviour {

	/*public Button housesButton;
	public Button factoryButton;
	public Button mallButton;
	public Button parkButton;
	public Button farmButton;*/

	public Dictionary<string, Button> buttons;

	private Canvas canvasObject;

	void Awake()
	{
		canvasObject = GetComponent <Canvas> ();
	}

	public void Hide()
	{
		canvasObject.enabled = false;
	}

	public void Show()
	{
		canvasObject.enabled = true;
	}
}
