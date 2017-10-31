using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TrashButtonScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

	public Color clickedColor;

	private Image buttonImage;
	private Color naturalColor;

	void Awake()
	{
		buttonImage = GetComponent <Image> ();
		naturalColor = buttonImage.color;
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		buttonImage.color = clickedColor;
		GameController.instance.cancelPlacement = true;
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		StopAllCoroutines ();
		StartCoroutine (DelayButtonExit ());
	}

	private IEnumerator DelayButtonExit()
	{
		yield return null;
		buttonImage.color = naturalColor;
		GameController.instance.cancelPlacement = false;
	}
}

