using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ThrowingCoinScript : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler {
	public GameObject throwingCoinPrefab;
	public Color activeButtonColor;
	public Color inactiveButtonColor;

	private bool active;
	private Image buttonImage;
	private bool mouseOverButton;

	void Awake()
	{
		active = false;
		buttonImage = GetComponent <Image> ();
	}

	void Update()
	{
		if(GameController.instance.CanThrowCoins ())
		{
			buttonImage.color = activeButtonColor;
		}
		else
		{
			buttonImage.color = inactiveButtonColor;
		}
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		if(active && GameController.instance.CanThrowCoins ())
		{
			GameController.instance.cameraScript.SetCanMoveTo (false);

			if (eventData.button == PointerEventData.InputButton.Left) {
				InstantiateCoin ();
				SetButtonTo (false, inactiveButtonColor);
			}
		}
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		SetButtonTo (true, activeButtonColor);
		GameController.instance.cameraScript.SetCanMoveTo (true);
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		mouseOverButton = true;
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		mouseOverButton = false;
	}
		

	public void SetButtonTo(bool value, Color newColor)
	{
		active = value;
		buttonImage.color = newColor;
	}

	private void InstantiateCoin()
	{
		Vector3 clickPosition = Input.mousePosition;

		GameObject instance = Instantiate (throwingCoinPrefab, clickPosition, Quaternion.identity);

		/*cancelInstance = false;*/

		StartCoroutine (MoveCoin (instance));
	}

	private IEnumerator MoveCoin(GameObject coin)
	{
		while(Input.GetMouseButton (0))
		{
			Vector3 worldPosition = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			worldPosition.z = -1f;

			coin.transform.position = worldPosition;

			yield return null;
		}

		ThrowableCoin coinScript = coin.GetComponent <ThrowableCoin> ();

		if(mouseOverButton || coinScript == null)
		{
			Destroy (coin.gameObject);
		}
		else
		{
			coinScript.Throw ();
		}
	}

}
