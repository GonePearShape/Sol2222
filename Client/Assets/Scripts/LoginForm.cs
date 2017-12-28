using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using GonePearShape.Cryptography;

namespace GonePearShape.Client
{

	public class LoginForm : MonoBehaviour
	{

		[SerializeField] InputField InputUserName;
		[SerializeField] InputField InputPassWord;
		[SerializeField] GameObject logForm;
		[SerializeField] GameObject regForm;

		private static String uName = "username"; 
		// private static String pWord = SHA.GenerateSHA256String("Password1@");
		private static String pWord = "548D8CF86E2D301F6E1F5DC621CBA2E409E8E814BA35CA1FEEFF6B0B995D848F";
		private static String shaPword;

		private void CheckLogin ()
		{
			shaPword = SHA.GenerateSHA256String (InputPassWord.text);
			if (InputUserName.text != "" && InputPassWord.text != "")
			{
				if (InputUserName.text == uName && shaPword == pWord)
				{
					Debug.Log ("Login Sucessful:");
					LoadLevel ("main");
					// Load Character Scene
				} 
				else
				{
					Debug.Log ("Login Failed:");
				}
			}
		}
			
		public void ShowRegisterForm ()
		{
			Debug.Log ("Displaying Register Form:");
			ClearFields ();
			logForm.SetActive(false);
			regForm.SetActive (true);
		}

		public void ClearFields ()
		{
				InputUserName.text = "";
				InputPassWord.text = "";
		}

		public void LoadLevel (String level)
		{
			// SceneManager.LoadScene(level);
			try
			{
				SceneManager.LoadScene(level, LoadSceneMode.Single);
			}
			catch (Exception ex)
			{
				Debug.Log ("An Error occured: Could not load the scene " + level + " Error: " + ex);
			}
		}
	}
}