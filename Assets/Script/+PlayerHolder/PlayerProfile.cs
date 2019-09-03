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
        public ProfileData_Deck[] deckList;
        public ProfileData_Deck _DeckToPlay;
        private int photonId;
#pragma warning restore 0649

        public string UniqueId
        {
            get { return uniqueId; }
        }
        public void SetDeckName(string name)
        {
            foreach (ProfileData_Deck v in deckList)
            {
                if(v.Name==name)
                {
                    _DeckToPlay = v;
                    Debug.LogFormat("Deck using for game is sat: {0}", v.Name); 
                }
            }
        }
        public string GetCardIds(int i)
        {
            if (_DeckToPlay == null)
                Debug.LogErrorFormat("{0}: DeckToPlayIsNull", name);
            if (_DeckToPlay.Cards[i] == null)
                Debug.LogErrorFormat("{0}: CardInDeckIsNull",name);
            return _DeckToPlay.Cards[i].Data.Name;
        }
        
        public string Name
        {
            set { name = value; }
            get { return name; }
        }
        public int PhotonId
        {
            set { photonId = value; }
            get { return photonId; }
        }
        public Sprite PlayerAvatar
        { get { return playerAvatar; } }
    }
}
