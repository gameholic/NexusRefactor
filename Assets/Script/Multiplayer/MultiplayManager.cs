using System.Collections.Generic;
using UnityEngine;

namespace GH.Multiplay

{
    /// <summary>
    /// Responsible for sending all events
    /// </summary>
    public class MultiplayManager : Photon.MonoBehaviour
    {
        #region Variables
        public static MultiplayManager singleton;

        private List<NetworkPrint> players = new List<NetworkPrint>();
        private NetworkPrint localPlayerNWPrint;
        Transform multiplayerReferences;
        //Now the game is on between two players so List might not be best way.

        [SerializeField]
        private PlayerHolder localPlayerHolder;
        [SerializeField]
        private PlayerHolder clientPlayerHolder;
        private bool gameStarted;
        private bool countPlayers;
        #endregion  

        public bool CountPlayer
        {
            set { countPlayers = value; }
            get { return countPlayers; }
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
            PlayerProfile profile = Resources.Load("Player Profile")as PlayerProfile;
            object[] data = new object[1];
            data[0] = profile.GetCardIds();
            
            PhotonNetwork.Instantiate("NetworkPrint",Vector3.zero,Quaternion.identity,0,data);                        
        }
        #endregion
        #region My Calls
        public NetworkPrint Players
        {
            set { players.Add(value); }
        }
        public void AddPlayer(NetworkPrint nw_print)
        {
            if(nw_print.IsLocal)
            {
                localPlayerNWPrint = nw_print;
            }
            players.Add(nw_print);
            nw_print.transform.parent = multiplayerReferences;
        }
        public NetworkPrint GetPlayers(int photonId)
        {
            for (int i=0;i<players.Count; i++)
            {
                if(players[i].photonId == photonId)
                {
                    return players[i];
                }
            }
            return null;
        }

        public void StartMatch()
        {
            GameController gc = GameController.singleton;

            foreach(NetworkPrint p in players)
            {
                if(p.IsLocal)
                {
                    localPlayerHolder.PhotonId = p.photonId;
                    localPlayerHolder.allCards.Clear();
                    localPlayerHolder.allCards.AddRange(p.GetStartingCardids());
                }
                else
                {
                    clientPlayerHolder.PhotonId = p.photonId;
                    clientPlayerHolder.allCards.Clear();
                    clientPlayerHolder.allCards.AddRange(p.GetStartingCardids());
                }
            
            }
            
            gc.InitGame(1);
        }

        #endregion
        #region Tick
        private void Update()
        {
            if(!gameStarted && countPlayers)
            {
                if(players.Count > 1)
                {
                    gameStarted = true;
                    StartMatch();
                }
            }
        }
        #endregion
    }

}
 
