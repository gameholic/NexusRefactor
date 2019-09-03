#pragma warning disable 0649
using GH.GameCard;
using GH.GameStates;
using GH.GameTurn;
using GH.Multiplay;
using GH.Player;
using GH.Player.Assists;
using GH.Setup;
using GH.UI;
using GH.GameCard.CardLogics;
using System.Collections.Generic;
using UnityEngine;
namespace GH
{
    public class GameController : Photon.MonoBehaviour
    {
        public static GameController singleton;

        [SerializeField]
        private PlayerHolder _CurrentPlayer;
        [SerializeField]
        private PlayerCardTransform _TopCardTransform;
        [SerializeField]
        private PlayerCardTransform _BottomCardTransform;
        [SerializeField]
        private Turn[] _Turns;
        [SerializeField]
        private PlayerStatsUI[] _PlayerStatsUI;
        [SerializeField]
        private ResourceManager _ResourceManager;


        public Phase CurrentPhase;
        public GameEvent OnTurnChanged;
        public GameEvent OnPhaseChanged;
        public StringVariable turnText;
        public StringVariable turnCountTextVariable;//Count the turn. When both player plays, it increases by 1
        public UpdatePlayer updatePlayer;
        //public TransformVariable[] graveyard_transform;
        public GameObject cardPrefab;
        public GameObject[] manaObj;
        public int turnIndex = 0;
        /// <summary>
        /// Variables used for multiplay
        /// </summary>
        /// 
        [SerializeField]
        private PlayerHolder localPlayer;
        [SerializeField]
        private PlayerHolder clientPlayer;
        private bool isComplete;
        //private bool switchPlayer;
        private int _TurnLength = 0;
        private BlockInstanceManager _BlockManager = new BlockInstanceManager();
        private CheckPlayerCanUse _CheckOwner = new CheckPlayerCanUse();
        
        private PlayerHolder[] _Players;
        private bool startTurn = true; //Check the start of the turn
        private int turnCounter; //Count the turn. When both player plays, it increases by 1
        private bool isInit;
        private bool _IsMultiplayer;
        #region GetSetProperties
        public PlayerHolder[] Players { get { return _Players; } }
        public PlayerHolder CurrentPlayer
        {
            set { _CurrentPlayer = value; }
            get { return _CurrentPlayer; }
        }
        public PlayerCardTransform TopCardTransform
        {
            set { _TopCardTransform = value; }
            get { return _TopCardTransform; }
        }
        public PlayerCardTransform BottomCardTransform
        {
            set { _BottomCardTransform = value; }
            get { return _BottomCardTransform; }
        }
        public Turn GetTurns(int i)
        {
            return _Turns[i];
        }
        public BlockInstanceManager BlockManager
        {
            set { _BlockManager = value; }
            get { return _BlockManager; }
        }
        public PlayerStatsUI GetPlayerUIInfo(int i)
        {
            return _PlayerStatsUI[i];
        }
        public CheckPlayerCanUse CheckOwner
        {
            get { return _CheckOwner; }
        }
        public void SetPlayer(int i, PlayerHolder p)
        {
            _Players[i] = p;
        }
        public PlayerHolder GetPlayer(int i)
        {
            return _Players[i];
        }

        public PlayerHolder GetOpponentOf(PlayerHolder p)
        {
            for (int i = 0; i < _Players.Length; i++)
            {
                if (GetPlayer(i) != p)
                    return GetPlayer(i);
            }
            return null;

        }
        public PlayerHolder LocalPlayer
        {
            get { return localPlayer; }
        }
        public PlayerHolder ClientPlayer
        {
            get { return clientPlayer; }
        }
        public ResourceManager ResourceManager
        {
            get { return _ResourceManager; }
        }
       
        public bool IsMultiplay
        {
            set
            {
                _IsMultiplayer = value;
            }
            get
            {
                return _IsMultiplayer;
            }
        }
        #endregion
        #region SetupForGame
        private void Awake()
        {
            Setting.gameController = this;
            singleton = this;
            isComplete = false;
            //switchPlayer = false;
            _TurnLength = _Turns.Length;
            _BlockManager.BlockInstDict = new Dictionary<Card, BlockInstance>();
            _CurrentPlayer = GetTurns(0).ThisTurnPlayer;
            _Players = new PlayerHolder [2];
            SetPlayer(0, localPlayer);
            SetPlayer(1, ClientPlayer);
            
            
        }
        public void InitGame(int startingPlayer)
        {
            Debug.Log("INITIALISING GAME...");

            NetworkPrint nwp = MultiplayManager.singleton.GetPlayer(LocalPlayer.InGameData.PhotonId);
            localPlayer = nwp.ThisPlayer;        //This is where local player's profile should be changed
            //Debug.Log(localPlayer.player);                            /

            //Turn[] _tmpTurn = new Turn[2];
            for (int i = 0; i < _Players.Length; i++)
            {
                GetPlayer(i).Init();
                GetPlayer(i).InGameData.StatsUI.Init();
                GetPlayer(i).InGameData.StatsUI = GetPlayerUIInfo(i);
                GetPlayer(i).InGameData.ManaManager.InitManaZero();
                GetPlayerUIInfo(i).UpdateManaUI();
            }
            BottomCardTransform = LocalPlayer.CardTransform;
            TopCardTransform = ClientPlayer.CardTransform;

            for (int i = 0; i < _Players.Length; i++)
            {
                if (GetPlayer(0) == GetPlayerUIInfo(i).player)
                    GetPlayer(0).InGameData.StatsUI = GetPlayerUIInfo(i);
                else
                    GetPlayer(1).InGameData.StatsUI = GetPlayerUIInfo(i);
            }
            turnCounter = 1;
            turnText.value = GetTurns(turnIndex).ThisTurnPlayer.PlayerProfile.Name; // Visualize whose turn is now            
            turnCountTextVariable.value = turnCounter.ToString();

            OnTurnChanged.Raise();
            SetUpMultiplay();
            isInit = true;
            Debug.Log("---Initialising Done---");
        }
        private void SetUpMultiplay()
        {
            SetUpGraveLogic();
        }
        private void SetUpGraveLogic()
        {
            PhotonNetwork.Instantiate("CardPlayManager", Vector3.zero, Quaternion.identity,0);

        }
        #endregion
        /// <summary>
        /// Pick top card from deck
        /// Issue: Does this function should be at Gamecontroller?
        /// </summary>
        /// <param name="p"></param>
        public void PickNewCardFromDeck(PlayerHolder p)
        {
            MultiplayManager.singleton.PlayerPicksCardFromDeck(p);
        }
        // Move targetPlayer, targetUI's position to destCardHolder's position            
        private void UpdateMana()
        {
            for (int i = 0; i < _PlayerStatsUI.Length; i++)
            {
                if (_CurrentPlayer == _PlayerStatsUI[i].player)
                {
                    _PlayerStatsUI[i].UpdateManaUI();
                }
            }
        }
        private void Update()
        { 
            if (!isInit)
            {
                Debug.LogError("IsInitIsFalse");
                return;
            }
            CurrentPlayer = GetTurns(turnIndex).ThisTurnPlayer;
            CurrentPhase = GetTurns(turnIndex).CurrentPhase.value;
            UpdateMana();

            if (startTurn)
            {
                Debug.Log("StartTurn");
                GetTurns(turnIndex).TurnBegin = startTurn;
                startTurn = false;
            }     
            turnCountTextVariable.value = turnCounter.ToString();
            updatePlayer.UpdatePlayerText(CurrentPlayer);
            isComplete = GetTurns(turnIndex).Execute();
            if (!IsMultiplay)
            {
                Debug.LogError("Multiplay is False");
            }
            else
            {
                if (isComplete)
                {
                    Debug.LogFormat("{0} Ends Turn", CurrentPlayer);
                    MultiplayManager.singleton.PlayerEndsTurn(CurrentPlayer.InGameData.PhotonId);
                }
            }            
        }
        public int GetAnotherPlayerID()
        {
            int r = turnIndex;
            r++;
            if (r > _TurnLength-1)
            {
                r = 0;
            }
            return GetTurns(r).ThisTurnPlayer.InGameData.PhotonId;
        }
        public int GetPlayerTurnIndex(int photonId)
        {
            for (int i = 0; i < _TurnLength; i++)
            {
                if (GetTurns(i).ThisTurnPlayer.InGameData.PhotonId == photonId)
                {
                    return i;
                }
            }
            return -1;
        }
        public void ChangeCurrentTurn(int photonId)
        {
            startTurn = true;
            turnIndex = GetPlayerTurnIndex(photonId);
            turnText.value = GetTurns(turnIndex).ThisTurnPlayer.ToString();
            OnTurnChanged.Raise();
        }
        public void ForceEndPhase()
        {
            GetTurns(turnIndex).EndCurrentPhase();
        }
        public void EndPhaseByBattleResolve()
        {
            EndPhase();
        }
        public void EndPhaseByButton()
        {
            EndPhase();
        }
        private void EndPhase()
        {
            if (CurrentPlayer == localPlayer)
            {
                GetTurns(turnIndex).EndCurrentPhase();
            }
            else
            {
                Debug.LogErrorFormat("EndPhaseException: Current Player_ {0}", CurrentPlayer);
            }
        }
    }
}