using System.Collections.Generic;
using UnityEngine;
using GH.GameCard;

namespace GH.Multiplay

{
    /// <summary>
    /// Responsible for sending all events
    /// </summary>
    public class MultiplayManager : Photon.MonoBehaviour
    {
        #region Variables
        public static MultiplayManager singleton;
        private List<NetworkPrint> _Players = new List<NetworkPrint>();
        private NetworkPrint localPlayerNWPrint;
        private Transform _MultiplayerReferences;
        
        //Now the game is on between two players so List might not be best way.

        [SerializeField]
        private PlayerHolder localPlayerHolder;
        [SerializeField]
        private PlayerHolder clientPlayerHolder;
        private bool gameStarted;
        private bool countPlayers; // To check game scene is loaded.
        #endregion  

        public bool CountPlayer
        {
            set { countPlayers = value; }
            get { return countPlayers; }
        }
        public GameController GC
        {
            get { return GameController.singleton; }
        }

        public NetworkPrint AddPlayers
        {
            set { _Players.Add(value); }
        }
        public List<NetworkPrint> Players
        {
            get { return _Players; }
        }
        public Transform MultiplayerReferences
        {
            get { return _MultiplayerReferences; }
        }
        private NetworkPrint GetPlayer(int photonID)
        {
            for(int i=0; i< Players.Count; i++)
            {
                if(Players[i].photonId == photonID)
                {
                    return Players[i]; 
                }
            }
            return null;
        }
        #region Init
        void OnPhotonInstantiate(PhotonMessageInfo info)
        {
            singleton = this;
            DontDestroyOnLoad(this.gameObject);

            InstantiateNetworkPrint();
            NetworkManager.singleton.LoadGameScene();
        }
         
        /// <summary>
        /// Respoonsible for adding / assigning Photon id to our player
        /// </summary>
        void InstantiateNetworkPrint()
        {
            Debug.Log("HowManyThisRun");
            PlayerProfile profile = Resources.Load("Player Profile")as PlayerProfile;
            object[] data = new object[1];
            data[0] = profile.GetCardIds();
            
            PhotonNetwork.Instantiate("NetworkPrint",Vector3.zero,Quaternion.identity,0,data);
        }
        #endregion

        #region Tick
        private void Update()
        {
            if (!gameStarted && countPlayers)//countPlayer is to make logic works after game scene is loaded
            {
                if (Players.Count > 1)
                {
                    gameStarted = true;
                    StartMatch();
                }
            }
        }
        #endregion

        #region Starting the Match  
        public void StartMatch()
        {
            List<int> playerId = new List<int>();
            List<int> cardInstId = new List<int>();
            List<string> cardName = new List<string>();
            ResourceManager rm = GC.ResourceManager;
            


            foreach(NetworkPrint p in Players)
            {
                foreach (string id in p.GetStartingCardids())
                {
                    Card c = rm.GetCardInstFromDeck(id);
                    playerId.Add(p.photonId);
                    cardInstId.Add(c.InstId);
                    cardName.Add(id);

                    if(p.IsLocal)
                    {
                        localPlayerHolder.PhotonId = p.photonId;
                        localPlayerHolder.AddCardToAllCardInst(c);
                    }
                }
                
                //if (p.IsLocal)
                //{
                //    localPlayerHolder.PhotonId = p.photonId;
                //    localPlayerHolder.allCards.Clear();
                //    localPlayerHolder.allCards.AddRange(p.GetStartingCardids());

                //}
                //else
                //{
                //    clientPlayerHolder.PhotonId = p.photonId;
                //    clientPlayerHolder.allCards.Clear();
                //    clientPlayerHolder.allCards.AddRange(p.GetStartingCardids());

                //    foreach (string id in p.GetStartingCardids())
                //    {
                //        Card c = rm.GetCardFromDict(id);
                //        localPlayerHolder.AddCardToAllCardInst(c);
                //    }
                //}
            
            }
            if(NetworkManager.singleton.IsMaster)
            {
                photonView.RPC("RPC_InitGame", PhotonTargets.All,1);
            }
        }
        [PunRPC]
        public void RPC_PlayerCreatesCard(int photonId,int cardId,string cardName)
        {
            if(NetworkManager.singleton.IsMaster)
            {
                //master can manage cards at line 105~108
                return;
            }
            Card c = GC.ResourceManager.GetCardInstFromDeck(cardName);
            c.InstId = cardId;
            NetworkPrint p = GetPlayer(photonId);
        }

        [PunRPC]
        public void RPC_InitGame(int startingPlayer)
        {

            GC.IsMultiplay = true;
            GC.InitGame(startingPlayer);
        }

        
        public void AddPlayer(NetworkPrint nw_print)
        {
            if(nw_print.IsLocal)
            {
                localPlayerNWPrint = nw_print;
            }
            AddPlayers = nw_print;
            nw_print.transform.parent = MultiplayerReferences;
        }
        public NetworkPrint GetPlayers(int photonId)
        {
            for (int i=0;i<Players.Count; i++)
            {
                if(Players[i].photonId == photonId)
                {
                    return Players[i];
                }
            }
            return null;
        }


        #endregion

        #region End Turn
        public void PlayerEndsTurn(int photonId)//photon Id is player who  ends the turn -> we need next one.
        {
            photonView.RPC("RPC_PlayerEndsTurn", PhotonTargets.MasterClient,photonId);
        }

        [PunRPC]
        public void RPC_PlayerEndsTurn(int photonId)
        {
            if(photonId == GC.CurrentPlayer.PhotonId)
            {
                if (NetworkManager.singleton.IsMaster)
                {
                    int targetId = GC.GetAnotherPlayerID();
                    photonView.RPC("RPC_PlayerStartsTurn", PhotonTargets.All, targetId);
                }
            }

        }

        [PunRPC]
        public void RPC_PlayerStartsTurn(int photonId)
        {
            GC.ChangeCurrentTurn(photonId);
        }
        #endregion

        #region Card Checks


        public void PlayerTryToUseCard(int cardInst, int photonId)
        {
            photonView.RPC("RPC_PlayerTryToUseCard", PhotonTargets.MasterClient, cardInst, photonId);
        }
        [PunRPC]
        public void RPC_PlayerTryToUseCard(int cardInst, int photonId)
        { 
            if(!NetworkManager.singleton.IsMaster)
                return;
        }

        //private bool PlayerHasCard(int cardInst, int photonId)
        //{
        //    bool r = false;
        //    NetworkPrint player = GetPlayer(photonId);

        //    return r;
        //}
        #endregion
    }

}
 
