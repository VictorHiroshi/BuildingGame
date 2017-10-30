using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginScript : MonoBehaviour {

	public InputField userNameField;
	public InputField passwordField;
	public Text messageText;

	private WaitForSeconds blinkTextDelay;

	private string userName;
	private string password;

	void Start()
	{
		blinkTextDelay = new WaitForSeconds (0.5f);
	}

	public void ValidateLogin()
	{
		StartCoroutine (CheckData ());
	}

	private IEnumerator CheckData()
	{
		IEnumerator messageRoutine = ShowDots ();
		StartCoroutine (messageRoutine);

		userName = userNameField.text;
		password = passwordField.text;

		yield return new WaitForSeconds (6f);

		StopCoroutine (messageRoutine);

		messageText.text = userName + " - " + password;
	}

	private IEnumerator ShowDots()
	{
		string[] dots = { ".", "..", "..." };
		int index = 0;

		while(true)
		{
			index = (index + 1) % 3;
			messageText.text = dots [index];
			yield return blinkTextDelay;
		}
	}
}
