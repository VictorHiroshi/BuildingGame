using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonsCanvasController : MonoBehaviour {

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
