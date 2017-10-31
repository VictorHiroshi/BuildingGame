using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class ButtonClick : MonoBehaviour, IPointerDownHandler{

	public GameObject prefab;

	private bool active;
	private Image buttonImage;

	void Awake()
	{
		active = false;
		buttonImage = GetComponent <Image> ();
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		if(active)
		{
			if (eventData.button == PointerEventData.InputButton.Left) {
				GameController.instance.BuildNew (prefab);
			}
		}
	}

	public int GetButtonCost()
	{
		return prefab.GetComponent <BuildingController> ().cost;
	}

	public void SetButtonTo(bool value, Color newColor)
	{
		active = value;
		buttonImage.color = newColor;
	}
}
