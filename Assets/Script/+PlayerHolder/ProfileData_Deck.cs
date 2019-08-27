using UnityEngine;
using UnityEditor;

using System;
using System.Collections.Generic;
using GH.GameCard;


namespace GH.Player.ProfileData
{
    [System.Serializable]
    public class ProfileData_Deck
    {
        public string Name;
        public Card[] Cards = new Card[30];
    }

}