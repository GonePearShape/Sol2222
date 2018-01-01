/*
 * PUBLISHER = "Gone Pear Shape";
 * VERSION = 0.3;
 * FACEBOOK = "https://www.facebook.com/GonePearShape";
 * TWITTER = "https://twitter.com/GonePearShape";
 * WEB = "http://gonepearshape.com";
*/

#region Using Statements
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GonePearShape.Client;
#endregion

namespace GonePearShape.Client
{

    public class PlayerGlobals : MonoBehaviour
    {
        #region Account Variables
        [Header("Account Variables")]
        [SerializeField] int _AccountID; // The account ID of this user used to load and save data [0]
        [SerializeField] string _AccountFirstName; // The Firstname of this user [1]
        [SerializeField] string _AccountLastName; // The Lastname of this user [2]
        [SerializeField] string _AccountDOB; // The Date of Birth of this user [3]
        [SerializeField] string _AccountEmail; // The Email of this user [4]
        [SerializeField] string _AccountUserName; // The username of this account [5]
        [SerializeField] string _AccountPassWord; // The password of this account SHA256 [6]
        [SerializeField] int _AccountNewsLetter; // The account Newsletter subscription status [7]
        [SerializeField] int _AccountIsBanned; // If the Account isBanned [8]
        [SerializeField] string _AccountMAC; // The stored MAC of this user [9]
        #endregion

        #region Current Character Variables
        [Header("Current Character Variables")]
        [SerializeField] int _AccountNumberOfCharacters; // The amount of charaters this user has for the character select screen
        [SerializeField] string[] _AccountCharacterNames; // The names of all the characters this user has for the character select screen
        [SerializeField] int[] _AccountCharacterLevels; // The value of each of this users characters level for the character select screen
        [SerializeField] string[] _AccountCharacterClasses; // The name of each charcters class for the character select screen
        [SerializeField] int _CharacterID; // The Character ID of the currently selected character 
        [SerializeField] string _CharacterName; // The Character Name of the currently selected character
        [SerializeField] string _CharacterClass; // The Class of the currently selected character
        [SerializeField] string _CharacterFaction; // The Faction of the currently selected character
        [SerializeField] int _CharacterRank; // The Rank as a number of the currently selected character
        [SerializeField] string _CharacterRankName; // The Rank Name  of the currently selected character
        [SerializeField] int _CharacterLevel; // The Level of the currently selected character
        [SerializeField] int _CharacterMaximumHealth; // The Maxium Health of the currently selected character
        [SerializeField] int _CharacterCurrentHealth; // The Current Health of the currently selected character
        [SerializeField] int _CharacterCurrency; // The amount of Currency held by the currently selected character
        [SerializeField] float _CharacterPositionX; // The Current Transform.Position.X
        [SerializeField] float _CharacterPositionY; // The Current Transform.Position.Y
        [SerializeField] float _CharacterPositionZ; // The Current Transform.Position.Z
        #endregion

        #region Account Variables
        // Account variables
        public int AccountID
        {
            get { return _AccountID; }
            set { _AccountID = value; }
        }

        public string AccountFirstName
        {
            get { return _AccountFirstName; }
            set { _AccountFirstName = value; }
        }

        public string AccountLastName
        {
            get { return _AccountLastName; }
            set { _AccountLastName = value; }
        }

        public string AccountDOB
        {
            get { return _AccountDOB; }
            set { _AccountDOB = value; }
        }

        public string AccountEmail
        {
            get { return _AccountEmail; }
            set { _AccountEmail = value; }
        }

        public string AccountUserName
        {
            get { return _AccountUserName; }
            set { _AccountUserName = value; }
        }

        public string AccountPassWord
        {
            get { return _AccountPassWord; }
            set { _AccountPassWord = value; }
        }

        public int AccountNewsLetter
        {
            get { return _AccountNewsLetter; }
            set { _AccountNewsLetter = value; }
        }

        public int AccountIsBanned
        {
            get { return _AccountIsBanned; }
            set { _AccountIsBanned = value; }
        }

        public string AccountMAC
        {
            get { return _AccountMAC; }
            set { _AccountMAC = value; }
        }

        #endregion

        #region Character Variables
        // Character Variables
        public int AccountNumberOfCharacters
        {
            get { return _AccountNumberOfCharacters; }
            set { _AccountNumberOfCharacters = value; }
        }

        public string[] AccountCharacterNames
        {
            get { return _AccountCharacterNames; }
            set { _AccountCharacterNames = value; }
        }

        public int[] AccountCharacterLevels
        {
            get { return _AccountCharacterLevels; }
            set { _AccountCharacterLevels = value; }
        }

        public string[] AccountCharacterClasses
        {
            get { return _AccountCharacterClasses; }
            set { _AccountCharacterClasses = value; }
        }

        public int CharacterID
        {
            get { return _CharacterID; }
            set { _CharacterID = value; }
        }

        public string CharacterName
        {
            get { return _CharacterName; }
            set { _CharacterName = value; }
        }

        public string CharacterClass
        {
            get { return _CharacterClass; }
            set { _CharacterClass = value; }
        }

        public string CharacterFaction
        {
            get { return _CharacterFaction; }
            set { _CharacterFaction = value; }
        }

        public int CharacterRank
        {
            get { return _CharacterRank; }
            set { _CharacterRank = value; }
        }

        public string CharacterRankName
        {
            get { return _CharacterRankName; }
            set { _CharacterRankName = value; }
        }

        public int CharacterLevel
        {
            get { return _CharacterLevel; }
            set { _CharacterLevel = value; }
        }

        public int CharacterMaximumHealth
        {
            get { return _CharacterMaximumHealth; }
            set { _CharacterMaximumHealth = value; }
        }

        public int CharacterCurrentHealth
        {
            get { return _CharacterCurrentHealth; }
            set { _CharacterCurrentHealth = value; }
        }

        public int CharacterCurrency
        {
            get { return _CharacterCurrency; }
            set { _CharacterCurrency = value; }
        }

        public float CharacterPositionX
        {
            get { return _CharacterPositionX; }
            set { _CharacterPositionX = value; }
        }

        public float CharacterPositionY
        {
            get { return _CharacterPositionY; }
            set { _CharacterPositionY = value; }
        }

        public float CharacterPositionZ
        {
            get { return _CharacterPositionZ; }
            set { _CharacterPositionZ = value; }
        }
        #endregion

        #region Methods
        void Awake()
        {
            DontDestroyOnLoad(this);
        }
        #endregion

    }
}
