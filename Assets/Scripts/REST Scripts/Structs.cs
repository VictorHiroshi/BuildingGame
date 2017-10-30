using System;
using System.Security.Cryptography;
using System.Text;

public class Login {

	public string username;
	public string password;

	public void SetPasswordToHashSHA256(string originalPassword)
	{
		byte[] bytes = Encoding.UTF8.GetBytes(originalPassword);
		SHA256Managed hashstring = new SHA256Managed();
		byte[] hash = hashstring.ComputeHash(bytes);
		string hashString = string.Empty;
		foreach (byte x in hash)
		{
			hashString += String.Format("{0:x2}", x);
		}

		password = hashString;
	}
}
