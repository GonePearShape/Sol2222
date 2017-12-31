using System;
using UnityEngine;
using UnityEngine.UI;
using GonePearShape.Cryptography;

namespace GonePearShape.Client
{
	public class RegisterForm : MonoBehaviour
	{
		[SerializeField] InputField InputUserName;
		[SerializeField] InputField InputPassWord;
		[SerializeField] InputField InputEmail;
		[SerializeField] InputField InputDOB;
		[SerializeField] GameObject logForm;
		[SerializeField] GameObject regForm;

		private static String uName = "username";
		private static int uNameMinLength = 8;
		private static int uNameMaxLength = 16;
        private static string salt = InputUserName.text + "150FE5514030B1434A5DEAF491ECE92C0E6497447D6DE7BD6BA85E8CAE1E00A3";
        private static string pWord = salt + "31EFFE5477F15FA7680896D879365A8CA3B60E027BB7E95615EBB3B70F83D286"; // remove SHA string once connected to DB
        private static String shaPword;

		private void CheckLogin ()
		{
			shaPword = SHA.GenerateSHA256String (salt + InputPassWord.text);
			if (InputUserName.text != "" && InputPassWord.text != "")
			{
				if (InputUserName.text == uName && shaPword == pWord)
				{
					Debug.Log ("Login Sucessful:");
				} 
				else
				{
					Debug.Log ("Login Failed:");
				}
			}
		}

		private void CheckRegister()
		{
			// check username is avaiable and conforms, no special chars, AlphaNumeric only
			// check password conforms to secuirty rules, under 16 chars must contain at least one UpperCase and one special char
			// check email address conforms
			// check DOB is correct

			// check the username complies
			// check the username is available
			if (InputUserName.text != "" && InputPassWord.text != "" && InputEmail.text != "" && InputDOB.text != "")
			{
#region UserName Validation
				if (InputUserName.text.Length >= uNameMinLength && InputUserName.text.Length <= uNameMaxLength)
				{
					if (CheckUpperCase (InputUserName.text) == false && CheckHasNumber (InputUserName.text) == false)
					{
						if (CheckUserNameAvailable (InputUserName.text) == true)
						{
							Debug.Log ("Registration Sucessful:");
							ShowLogInForm ();
						}
						else
						{
							Debug.Log ("Registration Failed: Username unavailable");
						}
					}
					else
					{
						Debug.Log ("Registration Failed: Validation");
					}
				}
				else
				{
					Debug.Log ("Registration Failed: Username Length is invalid:");
				}
			}
			else
			{
				Debug.Log ("Registration Failed: Empty strings");	
			}
#endregion
		}

		private bool CheckUserName (String s)
		{
			return false;
		}

		private bool CheckPassWord (String s)
		{
			return false;
		}

		private bool CheckEmail (String s)
		{
			return false;
		}

		private bool CheckDOB (String s)
		{
			return false;
		}

		private bool CheckLowerCase(String s)
		{
			for (int i = 0; i < s.Length; i++)
			{
				if (char.IsLower(s[i]))
				{
					Debug.Log ("Username contains lower case:");
					return true;
				}
			}
			Debug.Log ("Username does not contain lower case:");
			return false;
		}

		private bool CheckUpperCase(String s)
		{
			for (int i = 0; i < s.Length; i++)
			{
				if (char.IsUpper(s[i]))
				{
					Debug.Log ("Username contains UPPER CASE:");
					return true;
				}
			}
			Debug.Log ("Username does not contain UPPER CASE:");
			return false;
		}

		private bool CheckHasNumber(String s)
		{
			for (int i = 0; i < s.Length; i++)
			{
				if (char.IsNumber(s[i]))
				{
					Debug.Log ("Username contains a NUMBER:");
					return true;
				}
			}
			Debug.Log ("Username does not contain a NUMBER:");
			return false;
		}

		private bool CheckUserNameAvailable(String s)
		{
			// Check to see if the username already exsists
			if (s != uName)
			{
				Debug.Log ("Username is available:");
				return true;
			}
			else
			{
				Debug.Log ("Username is unavailable:");
				return false;
			}
		}
	

		public void ShowLogInForm ()
		{
			Debug.Log ("Displaying Login Form:");
			logForm.SetActive(true);
			regForm.SetActive (false);
			ClearFields ();

		}

		public void ClearFields ()
		{
				InputUserName.text = "";
				InputPassWord.text = "";
				InputEmail.text = "";
				InputDOB.text = "";
		}
	}
}