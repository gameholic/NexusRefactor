using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;
using GH.GameCard;
using UnityEngine.UI;
using GH.Player;
using GH.AssetEditor;
using System.IO;

namespace GH.Multiplay
{
    public class NetworkManager : Photon.PunBehaviour
    {
        public static NetworkManager singleton;


        [SerializeField]
        private StringVariable logger;
        private static bool _IsMaster; //true = is room manager else it's client
        private int _CardInstIds;
        [SerializeField]
        private ResourceManager rm;
        List<MultiplayerHolder> multiplayerHolders = new List<MultiplayerHolder>();

        public Dropdown dropDown;
        public GameEvent loggerUpdated;
        public GameEvent onConnected;
        public GameEvent failedToConnect;
        public GameEvent waitingForPlayer;

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
        public static  bool IsMaster
        {
            set { _IsMaster = value; }
            get { return _IsMaster; }
        }



        private void Awake()
        {
            if (singleton == null)
            {
                rm = Resources.Load("ResourceManager") as ResourceManager;
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
            rm.Init();
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
                Debug.Log("PlayerJoined");
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
            card.GetCardData.SetUniqueId = CardInstId;
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
            Debug.Log("Connected");
            base.OnConnectedToMaster(); 
            SetLoggerAsString("Connected");
            //logger.value = "Connected"
            loggerUpdated.Raise();
            onConnected.Raise();
        }
        public override void OnFailedToConnectToPhoton(DisconnectCause cause)
        {

            Debug.Log("Failed to Connected");
            base.OnFailedToConnectToPhoton(cause);
            SetLoggerAsString("Failed to Connect");
            loggerUpdated.Raise();
            failedToConnect.Raise();
        }
        public override void OnPhotonRandomJoinFailed(object[] codeAndMsg)
        {
            Debug.Log("RandomRoomNotFound: Create  new Room");
            base.OnPhotonRandomJoinFailed(codeAndMsg);
            CreateRoom();
        }
        public override void OnCreatedRoom()
        {
            base.OnCreatedRoom();
            IsMaster = true;
            Debug.Log("Room Created: I'm Master");
        }
        public override void OnJoinedRoom()
        {
            base.OnJoinedRoom();
            SetLoggerAsString("Waiting other player");
            loggerUpdated.Raise();
            waitingForPlayer.Raise();
        }

        /// <summary>
        /// Set player's playing deck and Enters the game.
        /// Player setting(Deck, ID, Avatar etc) is loaded and used in battle.
        /// </summary>
        /// <param name="newPlayer"></param>
        public override void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
        {
            if (IsMaster)
            {
                if (PhotonNetwork.playerList.Length > 1)
                {
                    SetLoggerAsString("Ready for game");
                    string deckName = dropDown.options[dropDown.value].text;
                    PlayerProfile p = FileBridge.LoadProfile();
                    if (p != null)
                    {
                        Debug.Log("Set Deck To Play");
                        p.SetDeckToPlay(deckName);
                        FileBridge.SaveProfile(p);
                        FileBridge.UpdateAsset(p);
                    }
                    else
                        Debug.LogError("OnPhtonPlayerConnected: Can'tFindPlayer");
                    loggerUpdated.Raise();
                    PhotonNetwork.room.IsOpen = false;
                    PhotonNetwork.Instantiate("Multiplay Manager", Vector3.zero, Quaternion.identity, 0);
                }
                else
                    Debug.Log("OnPhotonPlayerConnectedLog: PlayerListIsLessThan2");
            }
            else
                Debug.Log("OnPhotonPlayerConnectedLog:This Client is not master");
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
            cards.Add(c.GetCardData.UniqueId, c);
        }
        public Card GetCard(int instId)
        {
            Card r = null;
            cards.TryGetValue(instId, out r);
            return r;
        }
    }

}