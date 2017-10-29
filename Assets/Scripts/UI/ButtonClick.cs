using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class ButtonClick : MonoBehaviour, IPointerDownHandler{

	public GameObject prefab;

	public void OnPointerDown(PointerEventData eventData)
	{
		if(eventData.button == PointerEventData.InputButton.Left)
		{
			GameController.instance.BuildNew (prefab);
		}
	}

	public int GetButtonCost()
	{
		return prefab.GetComponent <BuildingController> ().cost;
	}

}
