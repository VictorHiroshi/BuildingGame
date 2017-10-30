using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.Remoting.Messaging;

public class LoginScript : MonoBehaviour {

	public string url = "http://dev.pushstart.com.br/desafio/public/api";

	public InputField userNameField;
	public InputField passwordField;
	public Text messageText;
	public Button confirmButton;
	public GameObject rayCastBlocker;
	public GameObject dataPanel;

	private WaitForSeconds blinkTextDelay;

	private Login loginData;
	private GameData gameData;
	private bool logged;

	void Start()
	{
		blinkTextDelay = new WaitForSeconds (0.6f);
	}

	public void ValidateLogin()
	{
		confirmButton.interactable = false;
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

		yield return StartCoroutine (FetchGameData ());

		if(logged)
		{
			logged = false;
			yield return StartCoroutine (FetchAdditionalData ());
		}

		StopCoroutine (messageRoutine);

		if(logged)
		{
			messageText.text = "Succeded";

			yield return new WaitForSeconds (2f);

			Destroy (rayCastBlocker);
			Destroy (dataPanel);
		}
		else
		{
			confirmButton.interactable = true;
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

	private IEnumerator FetchGameData()
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
			messageText.text = www.error;
		}
	}

	private IEnumerator FetchAdditionalData()
	{
		Dictionary<string, string> headers = new Dictionary<string, string> ();

		headers.Add ("X-Authorization", gameData.token);

		WWW www = new WWW (url + "/status", null, headers);

		yield return www;

		if(www.error == null)
		{
			//set user nickname and coins.
			//GameController.instance.userName.text = gameData.profile.name;
			AdditionalData data = JsonUtility.FromJson<AdditionalData> (www.text);

			logged = true;

			GameController.instance.Logged (data.nickname, data.money);
		}
		else
		{
			messageText.text = www.error;
		}
	}
}
