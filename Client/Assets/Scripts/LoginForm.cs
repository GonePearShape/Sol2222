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
        private static string salt = InputUserName.text + "150FE5514030B1434A5DEAF491ECE92C0E6497447D6DE7BD6BA85E8CAE1E00A3"; 
		private static string pWord = salt + "31EFFE5477F15FA7680896D879365A8CA3B60E027BB7E95615EBB3B70F83D286"; // remove SHA string once connected to DB
		private static string shaPword;

		private void CheckLogin ()
		{
			shaPword = SHA.GenerateSHA256String (salt + InputPassWord.text);
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