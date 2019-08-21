using UnityEngine;
using System;
using GH.GameCard;
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
        private Card[] _CarDeck;
        [SerializeField]
        private string[] _CardId;
        private int photonId;


#pragma warning restore 0649

        public string UniqueId
        {
            get { return uniqueId; }
        }
        //[SerializeField]
        //public ProfileData_Deck[] deckList;
        public string[] GetCardIds()
        {
            return _CardId;
        }
        public string Name
        {
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
