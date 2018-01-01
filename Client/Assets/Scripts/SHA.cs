/*
 * PUBLISHER = "Gone Pear Shape";
 * VERSION = 1.1;
 * FACEBOOK = "https://www.facebook.com/GonePearShape";
 * TWITTER = "https://twitter.com/GonePearShape";
 * WEB = "http://gonepearshape.com";
*/

/* USAGE:
To use this class in other classes in Unity use:
	using GonePearShape.Cryptography;

Then use:
	String mySHAPassword = SHA.GenerateSHA256String("My Password");

Or if you would like to just convert a string to SHA256 and copy it from the Console use:
	SHA.PrintToConsole("String I'd Like Converted");
*/
#region using statements
using System;
using System.Text;
using System.Security.Cryptography;
using UnityEngine;
#endregion

namespace GonePearShape.Cryptography
{
	public class SHA : MonoBehaviour
	{
#region method GenerateSHA256String
		// Takes a String, converts it to SHA256 and returns the result
		public static String GenerateSHA256String ( String inputString )
		{
			SHA256 sha256 = SHA256Managed.Create ( );
			byte[] bytes = Encoding.UTF8.GetBytes ( inputString );
			byte[] hash = sha256.ComputeHash ( bytes );
			return GetStringFromHash ( hash );
		}
#endregion

#region method GetStringFromHash
		// Takes an Array of Bytes, hashes to a String and returns the result
		private static String GetStringFromHash ( byte[] hash )
		{
			StringBuilder result = new StringBuilder ( );
			for ( int i = 0; i < hash.Length; i ++ )
			{
				result.Append (hash[i].ToString ( "X2" ) );
			}

			return result.ToString ( );
		}
#endregion

#region method PrintToConsole
		// Takes a String, converts it to SHA256 and prints it to the Console
		public static void PrintToConsole ( string s )
		{
			Debug.Log (SHA.GenerateSHA256String ( s ) );
		}
#endregion

	} // End class

} // End namespace