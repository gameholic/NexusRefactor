
using UnityEditor;
using UnityEngine;
using GH.GameCard;
using GH.Player;
using System.Collections.Generic;
using System.IO;
using GH.AssetEditor;

namespace GH.Multiplay
{
    public class NetworkPrint : Photon.MonoBehaviour
    {

        //Below Variables should be Private
        public int photonId;
        private bool isLocal;
        public List<string> cardIds = new List<string>();
        public List<Card> _CardDeck = new List<Card>();       
        public PlayerHolder _PlayerHolder;
        public PlayerProfile _Profile;

        public PlayerProfile PlayerProfile
        {
            get { return _Profile; }
        }
        public List<Card> CardDeck
        {
            get { return _CardDeck; }
        } 
        public void AddCardToDeck(Card c)
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
        public List<string> GetStartingCardids
        {
            get { return cardIds; }
        }
        public PlayerProfile SetPlayerProfile { set { _Profile = value; } }

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
                playerProfile.Name = "MadeNewFromReadPlayerProfile";
            }
            return playerProfile;
        }
        void OnPhotonInstantiate(PhotonMessageInfo info)
        {
            photonId = photonView.ownerId;
            isLocal = photonView.isMine;
            Debug.Log("Intantiate Photon Network Print // photon id is " + photonId);


            //if (this.isLocal)
            //{
            //    Debug.Log("Manager Client: My print");
            //    _Profile = FileBridge.LoadProfile();
            //    SetProfile();
            //}         
            //When NetworkPrint bugs get fixed, delete below codes and use above codes
            if (NetworkManager.IsMaster)
            {
                if (this.isLocal)
                {
                    Debug.Log("Manager Client: My print");
                    _Profile = FileBridge.LoadProfile();
                    SetProfile();
                    //USING RPC TO SEND DATA
                }
                else
                {
                    Debug.Log("Manager Client: opponent print");
                    //How to call oppponent information?
                }
            }
            else
            {
                //if(!this.isLocal)
                if(this.isLocal)
                {
                    Debug.Log("Member Client: My print");
                    _Profile = FileBridge.LoadProfile();
                    SetProfile();
                }
                else
                {
                    Debug.Log("Manager Client: Opponent print");

                }
               
            }
            MultiplayManager.singleton.AddPlayer(this);
        }

        public void SetProfile()
        {
            Debug.LogFormat("==SET {0} PROFILE==", PlayerProfile.Name);
            for (int i = 0; i < PlayerProfile._DeckToPlay.Cards.Length; i++)
            {
                if (PlayerProfile.GetCardIds(i) != null)
                    cardIds.Add(PlayerProfile.GetCardIds(i));
            }
        }
    }
}