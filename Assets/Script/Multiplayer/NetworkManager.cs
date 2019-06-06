using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;
using GH.GameCard;

namespace GH.Multiplay
{
    public class NetworkManager : Photon.PunBehaviour
    {
        [SerializeField]
        private StringVariable logger;
        private static bool _IsMaster;
        private int _CardInstIds;
        ResourceManager rm;
        List<MultiplayerHolder> multiplayerHolders = new List<MultiplayerHolder>();
        public static NetworkManager singleton;

        public GameEvent loggerUpdated;
        public GameEvent onConnected;
        public GameEvent failedToConnect;
        public GameEvent waitingForPlayer;

        /// <summary>
        /// Property & Set/Get field
        /// </summary>
        public MultiplayerHolder GetHolder(int photonId)
        {
            for (int i = 0; i < multiplayerHolders.Count; i++)
            {
                if (multiplayerHolders[i].OwnerId == photonId)
                    return multiplayerHolders[i];
            }
            return null;
        }
        public Card GetCard(int instId, int ownerId)
        {
            MultiplayerHolder h = GetHolder(ownerId);
            return h.GetCard(instId);
        }
        public StringVariable Logger
        {
            set { logger = value; }
            get {return logger;}
        }
        public void SetLoggerAsString(string str)
        {
            logger.value = str;
        }
        public int CardInstId
        {
            set { _CardInstIds = value; }
            get { return _CardInstIds; }
        }
        public bool IsMaster
        {
            set { _IsMaster = value; }
            get { return _IsMaster; }
        }
        /// <summary>
        /// Property & Set/Get field
        /// </summary>

        private void Awake()
        {
            if (singleton == null)
            {
                rm = GameController.singleton.ResourceManager;
                singleton = this;
                DontDestroyOnLoad(this.gameObject);
            }
            else
            {
                Destroy(this.gameObject);
            }
        }
        /// <summary>
        /// Connect to Photon networks
        /// </summary>
        private void Start()
        {
            PhotonNetwork.autoCleanUpPlayerObjects = false;
            PhotonNetwork.autoJoinLobby = false;
            PhotonNetwork.automaticallySyncScene = false;
            Init();

        }
      
        public void Init()
        {
            PhotonNetwork.ConnectUsingSettings("1");
            SetLoggerAsString("Connecting");
            loggerUpdated.Raise();
            //Use this to add something in future
        }
       
        #region MyCalls


        public void OnPlayGame()
        {
            JoinRandomRoom();        
        }
        private void JoinRandomRoom()
        {
            PhotonNetwork.JoinRandomRoom();
        }
        private void CreateRoom()
        {
            RoomOptions room = new RoomOptions();
            room.MaxPlayers = 2;
            PhotonNetwork.CreateRoom(RandomString(256), room, TypedLobby.Default);
        }
        private System.Random random = new System.Random();
        private string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789abcdefghijklmnopqrstuvwxyz";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        public void PlayerJoined(int photonId,string[] cards)
        {
            MultiplayerHolder m = new MultiplayerHolder();
            m.OwnerId = photonId;

            for(int i =0;i<cards.Length;i++)
            {
                Card c = CreateCardMaster(cards[i]);
                if (c == null)
                    continue;

                m.RegisterCard(c);
                //Rpc 
            }
        }


        public Card CreateCardMaster(string cardId)
        {
            Card card = rm.GetCardInstFromDeck(cardId);
            card.InstId = CardInstId;
            CardInstId = CardInstId + 1;
            return card;
        }
        //public void CreateCardClient_Call(string cardId, int instId,int photonId)
        //{
        //    Card c = CreateCardClient(cardId, instId);
        //    if(c!=null)
        //    {
        //        MultiplayerHolder h = GetHolder(photonId);
        //        h.RegisterCard(c);
        //    }

        //}
        //public Card CreateCardClient(string cardId, int instId)
        //{
        //    Card card = rm.GetCardFromDict(cardId);
        //    card.InstId = CardInstId;
        //    return card;
        //}
        #endregion

        #region Phton Callbacks
        public override void OnConnectedToMaster()
        {
            base.OnConnectedToMaster(); 
            SetLoggerAsString("Connected");
            //logger.value = "Connected"
            loggerUpdated.Raise();
            onConnected.Raise();
        }
        public override void OnFailedToConnectToPhoton(DisconnectCause cause)
        {
            base.OnFailedToConnectToPhoton(cause);
            SetLoggerAsString("Failed to Connect");
            loggerUpdated.Raise();
            failedToConnect.Raise();
        }
        public override void OnPhotonRandomJoinFailed(object[] codeAndMsg)
        {
            base.OnPhotonRandomJoinFailed(codeAndMsg);
            CreateRoom();
        }

        public override void OnCreatedRoom()
        {
            base.OnCreatedRoom();
            IsMaster = true;
        }

        public override void OnJoinedRoom()
        {
            base.OnJoinedRoom();
            SetLoggerAsString("Waiting other player");
            loggerUpdated.Raise();
            waitingForPlayer.Raise();
        }

        public override void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
        {
            if(IsMaster)
            {
                if(PhotonNetwork.playerList.Length > 1)
                {
                    SetLoggerAsString("Ready for game");
                    loggerUpdated.Raise();
                    PhotonNetwork.room.IsOpen = false;
                    PhotonNetwork.Instantiate("Multiplay Manager", Vector3.zero, Quaternion.identity, 0);
                }
            }
        }
        public void LoadGameScene()
        {
            SessionManager.singleton.LoadGameLevel(OnGameSceneLoaded);
        }

        /// <summary>
        /// Responsible for getting data from n/w manager about BattleScene
        /// </summary>
        void OnGameSceneLoaded()
        {
            MultiplayManager.singleton.CountPlayer = true;
        }

        public override void OnDisconnectedFromPhoton()
        {
            base.OnDisconnectedFromPhoton();
        }

        public override void OnLeftRoom()
        {
            base.OnLeftRoom();
        }
        #endregion

        #region RPCs

        #endregion

    }

    public class MultiplayerHolder
    {
        private int _OwnerId;
        private Dictionary<int, Card> cards = new Dictionary<int, Card>();

        public int OwnerId
        {
            set { _OwnerId = value; }
            get { return _OwnerId; }
        }
        public void RegisterCard(Card c)
        {
            cards.Add(c.InstId, c);
        }

        public Card GetCard(int instId)
        {
            Card r = null;
            cards.TryGetValue(instId, out r);
            return r;
        }
    }

}