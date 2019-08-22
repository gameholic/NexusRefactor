using GH.GameCard;
using GH.Player;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
namespace GH.Multiplay
{
    public class NetworkPrint : Photon.MonoBehaviour
    {
        public int photonId;
        private bool isLocal;
        string[] cardIds;
        private List<Card> _CardDeck = new List<Card>();
        private PlayerHolder _PlayerHolder;
        public PlayerProfile _Profile;

        public PlayerProfile PlayerProfile
        {
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
        void OnPhotonInstantiate(PhotonMessageInfo info)
        {            
            photonId = photonView.ownerId;
            isLocal = photonView.isMine;
            //object[] data = photonView.instantiationData;
            //cardIds = (string[])data[0];
            _Profile = ReadPlayerProfileJSON();
            if(_Profile!=null)
                SetProfile();
            
            MultiplayManager.singleton.AddPlayer(this);
        }

        void SetProfile()
        {
            Debug.Log("SET PROFILE");
            cardIds = PlayerProfile.GetCardIds();
        }
    }

}