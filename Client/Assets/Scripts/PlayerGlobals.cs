using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GonePearShape.Client
{

    public class PlayerGlobals : MonoBehaviour
    {
        [Header("Account Variables")]
        [SerializeField] int AccountID; // The account ID of this user used to load and save data
        [SerializeField] int AccountNumberOfCharacters; // The amount of charaters this user has for the character select screen
        [SerializeField] string AccountUserName; // The username of this account
        [SerializeField] string[] AccountCharacterNames; // The names of all the characters this user has for the character select screen
        [SerializeField] int[] AccountCharacterLevels; // The value of each of this users characters level for the character select screen
        [SerializeField] string[] AccountCharacterClasses; // The name of each charcters class for the character select screen

        [Header("Current Character Variables")]
        [SerializeField] int CharacterID; // The Character ID of the currently selected character 
        [SerializeField] string CharacterName; // The Character Name of the currently selected character
        [SerializeField] string CharacterClass; // The Class of the currently selected character
        [SerializeField] string CharacterFaction; // The Faction of the currently selected character
        [SerializeField] int CharacterRank; // The Rank as a number of the currently selected character
        [SerializeField] string CharacterRankName; // The Rank Name  of the currently selected character
        [SerializeField] int CharacterLevel; // The Level of the currently selected character
        [SerializeField] int CharacterMaximumHealth; // The Maxium Health of the currently selected character
        [SerializeField] int CharacterCurrentHealth; // The Current Health of the currently selected character
        [SerializeField] int CharacterCurrency; // The amount of Currency held by the currently selected character
        [SerializeField] float CharacterPositionX; // The Current Transform.Position.X
        [SerializeField] float CharacterPositionY; // The Current Transform.Position.Y
        [SerializeField] float CharacterPositionZ; // The Current Transform.Position.Z

        void Awake()
        {
            AccountID = 0;
            DontDestroyOnLoad(this);
        }
    }
}
