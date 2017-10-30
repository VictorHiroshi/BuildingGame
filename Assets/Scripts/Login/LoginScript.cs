using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginScript : MonoBehaviour {

	public string url = "http://dev.pushstart.com.br/desafio/public/api";

	public InputField userNameField;
	public InputField passwordField;
	public Text messageText;

	private WaitForSeconds blinkTextDelay;

	private Login loginData;
	private GameData gameData;
	private bool logged;
	private string errorMessage;

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

		logged = false;
		loginData.username = userNameField.text;
		loginData.SetPasswordToHashSHA256 (passwordField.text);

		yield return StartCoroutine (POST (loginData));

		StopCoroutine (messageRoutine);

		if(logged)
		{
			messageText.text = "Succeded";

			GameController.instance.userName.text = gameData.profile.name;

			yield return new WaitForSeconds (2f);

			Destroy (gameObject);
		}
		else
		{
			messageText.text = errorMessage;
		}
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

	private IEnumerator POST(Login loginData)
	{
		WWWForm form = new WWWForm ();

		form.AddField ("username", loginData.username);
		form.AddField ("password", loginData.password);

		WWW www = new WWW (url + "/auth/login", form);

		yield return www;

		if(www.error == null)
		{
			gameData = JsonUtility.FromJson<GameData> (www.text);
			logged = true;
		}
		else
		{
			errorMessage = www.error;
		}
	}
}
