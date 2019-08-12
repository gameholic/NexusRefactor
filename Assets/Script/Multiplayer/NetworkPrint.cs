using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using GH.GameCard;
using System.IO;
namespace GH.Multiplay
{
    public class NetworkPrint : Photon.MonoBehaviour
    {
        public int photonId;
        private bool isLocal;
        string[] cardIds;
        private Dictionary<int, Card> _MyCards = new Dictionary<int, Card>();
        private List<Card> _CardDeck = new List<Card>();
        private PlayerHolder _PlayerHolder;
        public PlayerProfile _Profile;

        public PlayerProfile playerProfile
        {
            set { _Profile = value; }
            get { return _Profile; }
        }
        public List<Card> CardDeck
        {
            get { return _CardDeck; }
        } 
        private void AddCardToDeck(Card c)
        {
            _CardDeck.Add(c);
        }
        public PlayerHolder ThisPlayer
        {
            set { _PlayerHolder = value; }
            get {return  _PlayerHolder; }
        }
        public void AddCard(Card c)
        {
            _MyCards.Add(c.InstId, c);
            AddCardToDeck(c);
        }
        public bool IsLocal
        {
            get { return isLocal; }
        }
        public string[] GetStartingCardids()
        {
            return cardIds;
        }

        private string playerProfileFilePath = "/StreamingAssets/playerProfile.json";
        PlayerProfile ReadPlayerProfileJSON()
        {
            string filePath = Application.dataPath + playerProfileFilePath;
            PlayerProfile playerProfile;

            if (File.Exists(filePath))
            {
                string dataAsJson = File.ReadAllText(filePath);
                playerProfile = JsonUtility.FromJson<PlayerProfile>(dataAsJson);
            }
            else
            {
                playerProfile = new PlayerProfile();
            }

            return playerProfile;

        }

        public Card GetCard(int instId)
        {
            Card c = null;
            _MyCards.TryGetValue(instId, out c);
            return c;
        }
        void OnPhotonInstantiate(PhotonMessageInfo info)
        {            
            photonId = photonView.ownerId;
            isLocal = photonView.isMine;
            //object[] data = photonView.instantiationData;
            //cardIds = (string[])data[0];
            playerProfile = ReadPlayerProfileJSON();
            if(_Profile!=null)
                SetProfile();
            
            MultiplayManager.singleton.AddPlayer(this);
        }

        void SetProfile()
        {
            Debug.Log("SET PROFILE");
            cardIds = playerProfile.GetCardIds();
        }
    }

}