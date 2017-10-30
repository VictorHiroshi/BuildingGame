using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginScript : MonoBehaviour {

	public InputField userNameField;
	public InputField passwordField;
	public Text messageText;

	private WaitForSeconds blinkTextDelay;

	private Login loginData;

	void Start()
	{
		blinkTextDelay = new WaitForSeconds (0.5f);
	}

	public void ValidateLogin()
	{
		loginData = new Login ();
		StartCoroutine (CheckData ());
	}

	private IEnumerator CheckData()
	{
		IEnumerator messageRoutine = ShowDots ();
		StartCoroutine (messageRoutine);

		loginData.username = userNameField.text;
		loginData.SetPasswordToHashSHA256 (passwordField.text);

		StopCoroutine (messageRoutine);

		messageText.text = loginData.username + " - " + loginData.password;

		yield return new WaitForSeconds (3f);

		/*Destroy (gameObject);*/
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
