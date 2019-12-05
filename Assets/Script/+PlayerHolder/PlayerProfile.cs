using UnityEngine;
using System;
using GH.GameCard;

using GH.Player.ProfileData;
using UnityEngine.UI;

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
        public void SetDeckToPlay(string name)
        {
            foreach (ProfileData_Deck v in deckList)
            {
                if(v.Name==name)
                {
                    _DeckToPlay = v;
                    Debug.LogFormat("Deck using for game is sat: {0}", v.Name); 
                }
            }
            Debug.Log("SelectedDeck is "+_DeckToPlay.Name); //If this is logged as null, the deck isn't sat.
        }
        public string GetCardIds(int i)
        {
            if (_DeckToPlay == null)
                Debug.LogErrorFormat("{0}: DeckToPlayIsNull", name);
            else if (_DeckToPlay.Cards[i] == null)
                Debug.LogErrorFormat("{0}: CardInDeckIsNull",name);
            Debug.Log("Get CARD " + _DeckToPlay.Cards[i].name);
            return _DeckToPlay.Cards[i].name;
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
