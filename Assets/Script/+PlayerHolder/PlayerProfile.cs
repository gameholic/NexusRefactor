using UnityEngine;
using System;
using GH.GameCard;

using GH.Player.ProfileData;
namespace GH.Player
{
    [Serializable]
    public class PlayerProfile              //Player Profile needs a lot changes
    {

#pragma warning disable 0649


        [SerializeField]
        private string uniqueId;
        [SerializeField]
        private string name;
        [SerializeField]
        private Sprite playerAvatar;

        [SerializeField]
        public ProfileData_Deck[] deckList = new ProfileData_Deck[5];

        [SerializeField]
        private ProfileData_Deck _SelectedDeck;
        private int photonId;


#pragma warning restore 0649

        public string UniqueId
        {
            get { return uniqueId; }
        }
        //[SerializeField]
        //public ProfileData_Deck[] deckList;
        public void SetDeckName(string name)
        {
            foreach (ProfileData_Deck v in deckList)
            {
                if(v.Name==name)
                {
                    _SelectedDeck = v;
                    Debug.LogFormat("Deck using for game is sat: {0}", v.Name); 
                }
            }
        }


        public string[] GetCardIds()
        {
            return _SelectedDeck.Cards;
        }
        public string Name
        {
            set { name = value; }
            get { return name; }
        }

        //public string GetCardIds(int i)
        //{
        //    return _Card[i].properties[i].stringValue;              //1st property of Card is card name
        //}
        public int PhotonId
        {
            set { photonId = value; }
            get { return photonId; }
        }
        public Sprite PlayerAvatar
        { get { return playerAvatar; } }
    }
}
