#pragma warning disable 0649
using GH.GameStates;
using System.Collections.Generic;
using UnityEngine;
using GH.GameCard;
using GH.GameTurn;
using GH.Setup;
using GH.Multiplay;

namespace GH
{
    public class GameController : MonoBehaviour
    {
        public static GameController singleton;

        [SerializeField]
        private PlayerHolder _CurrentPlayer;
        [SerializeField]
        private CardHolders _TopCardHolder;
        [SerializeField]
        private CardHolders _BottomCardHolder;
        [SerializeField]
        private State _CurrentState;
        [SerializeField]
        private Turn[] _Turns;
        [SerializeField]
        private PlayerStatsUI[] _PlayerStatsUI;
        [SerializeField]
        private CardGraveyard _CardGrave;
        [SerializeField]
        private ResourceManager _ResourceManager;
        private bool _IsMultiplayer;
        public GameEvent OnTurnChanged;
        public GameEvent OnPhaseChanged;
        public StringVariable turnText;
        public StringVariable turnCountTextVariable;//Count the turn. When both player plays, it increases by 1
        //public TransformVariable[] graveyard_transform;
        public GameObject cardPrefab;
        public GameObject[] manaObj;

        public int turnIndex = 0;
        private bool isComplete;
        private bool switchPlayer;
        private int _TurnLength = 0;
        private BlockInstanceManager _BlockManager = new BlockInstanceManager();
        private CheckPlayerCanUse _CheckOwner = new CheckPlayerCanUse();
        private LoadPlayerUI _LoadPlayerUI = new LoadPlayerUI();
        public PlayerHolder[] _Players;

        private bool startTurn = true; //Check the start of the turn
        private int turnCounter; //Count the turn. When both player plays, it increases by 1
        private bool isInit;
        /// <summary>
        /// Variables used for multiplay
        /// </summary>
        [SerializeField]
        private PlayerHolder localPlayer;
        [SerializeField]
        private PlayerHolder clientPlayer;

        /// <summary>
        /// Get/Set Methods/Properties
        /// </summary>
        public PlayerHolder CurrentPlayer
        {
            set { _CurrentPlayer = value; }
            get { return _CurrentPlayer; }
        }
        public CardHolders TopCardHolder
        {
            set { _TopCardHolder = value; }
            get { return _TopCardHolder; }
        }
        public CardHolders BottomCardHolder
        {
            set { _BottomCardHolder = value; }
            get { return _BottomCardHolder; }
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
        public LoadPlayerUI LoadPlayerUI
        {
            get { return _LoadPlayerUI; }
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
            set { localPlayer = value; }
            get { return localPlayer; }
        }
        public PlayerHolder ClientPlayer
        {
            set { clientPlayer = value; }
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
        /// <summary>
        /// Get/Set Properties
        /// </summary>
        private void Awake()
        {
            Setting.gameController = this;
            singleton = this;
            isComplete = false;
            switchPlayer = false;
            _TurnLength = _Turns.Length;
            _BlockManager.BlockInstDict = new Dictionary<CardInstance, BlockInstance>();
            _Players = new PlayerHolder[_TurnLength];
            for (int i = 0; i < _TurnLength; i++)
            {
                SetPlayer(i, GetTurns(i).ThisTurnPlayer);
                if (GetPlayer(i).player == "Player1")
                    GetPlayer(i).isBottomPos = true;
                else
                    GetPlayer(i).isBottomPos = false;
                Setting.RegisterLog(GetPlayer(i).name + " joined the game successfully", GetPlayer(i).playerColor);
            }
            _CurrentPlayer = GetTurns(0).ThisTurnPlayer;
            //_BottomCardHolder = GetTurns(0).ThisTurnPlayer.currentCardHolder;
            //_TopCardHolder = GetTurns(1).ThisTurnPlayer.currentCardHolder;
        }
        private void Start()
        {



        }
        public void InitGame(int startingPlayer)
        {
            Turn[] _tmpTurn = new Turn[2];

            for (int i = 0; i < _Players.Length; i++)
            {
                GetPlayer(i).statsUI = GetPlayerUIInfo(i);
                GetPlayer(i).manaResourceManager.InitManaZero();
                GetPlayerUIInfo(i).UpdateManaUI();
                /*
                 *Initialise mana as 0
                 *This is for the speical mode that might exist in future
                 *      Ex) Initialise mana as 3 
                 */

                if (GetPlayer(i).PhotonId == startingPlayer)
                {
                    _tmpTurn[0] = GetTurns(i);
                }
                else
                {
                    _tmpTurn[1] = GetTurns(i);
                }
            }
            _Turns = _tmpTurn;
            SetupPlayers();
            turnCounter = 1;
            turnText.value = GetTurns(turnIndex).ThisTurnPlayer.ToString(); // Visualize whose turn is now            
            turnCountTextVariable.value = turnCounter.ToString();
            OnTurnChanged.Raise();
            isInit = true;

        }


        private void SetupPlayers()
        {
            for (int i = 0; i < _Players.Length; i++)
            {
                GetPlayer(i).Init();
                if (i == 0)
                    GetPlayer(i).currentCardHolder = BottomCardHolder; // Player 1 is bottom card holder
                else
                    GetPlayer(i).currentCardHolder = TopCardHolder;  // Player 2 is top card holder

                if (i < 2)
                {
                    GetPlayer(i).statsUI = GetPlayerUIInfo(i);
                    GetPlayerUIInfo(i).player.LoadPlayerOnStatsUI();
                }
            }
        }
        //public void LoadPlayerOnActive(PlayerHolder loadedPlayer)
        //{
        //    //At first run, bottomcardholder is player1, topCardHolder is player2
        //    PlayerHolder prevPlayer = TopCardHolder.thisPlayer;
        //    if (loadedPlayer == TopCardHolder.thisPlayer)
        //    {
        //        prevPlayer = BottomCardHolder.thisPlayer;
        //        LoadPlayerOnHolder(prevPlayer, GetPlayer(1).currentCardHolder, _PlayerStatsUI[0]); // move bottom player's UI, cards to top                
        //        LoadPlayerOnHolder(loadedPlayer, GetPlayer(0).currentCardHolder, _PlayerStatsUI[1]);
        //        if (GetTurns(turnIndex).PhaseIndex != 2) // 3rd Phase is blockphase___ On block phase, infinite loop exists.
        //        {
        //            TopCardHolder = prevPlayer.currentCardHolder;
        //            BottomCardHolder = loadedPlayer.currentCardHolder;
        //        }
        //    }
        //    else if (loadedPlayer == BottomCardHolder.thisPlayer && loadedPlayer.player == "Player2")
        //    {
        //        //This is only run at battle phase;
        //        prevPlayer = TopCardHolder.thisPlayer;
        //        LoadPlayerOnHolder(prevPlayer, BottomCardHolder, _PlayerStatsUI[1]); // move bottom player's UI, cards to top
        //        LoadPlayerOnHolder(loadedPlayer, TopCardHolder, _PlayerStatsUI[0]);
        //    }
        //    else if (loadedPlayer != TopCardHolder.thisPlayer && loadedPlayer != BottomCardHolder.thisPlayer)
        //    {
        //        Debug.LogError("loaded player isn't at bottom nor top");
        //    }
        //}
        //public void LoadPlayerOnHolder(PlayerHolder targetPlayer, CardHolders destCardHolder, PlayerStatsUI targetUI)
        //{
        //    destCardHolder.LoadPlayer(targetPlayer, targetUI);
        //}
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
            //turnIndex is great but not in multiplay
            if (!isInit)
                return;
            UpdateMana();
            if (startTurn)
            {
                GetTurns(turnIndex).TurnBegin = startTurn;
                startTurn = false;
            }
            if (switchPlayer)
            {
                Setting.RegisterLog("Position Changed!! ", Color.green);
                ///Switch UI's position.
                Vector3 t = _PlayerStatsUI[0].gameObject.transform.position;
                _PlayerStatsUI[0].gameObject.transform.position = _PlayerStatsUI[1].gameObject.transform.position;
                _PlayerStatsUI[1].gameObject.transform.position = t;

                Vector3 m = manaObj[0].gameObject.transform.position;
                manaObj[0].gameObject.transform.position = manaObj[1].gameObject.transform.position;
                manaObj[1].gameObject.transform.position = m;

                switchPlayer = false;
            }

            turnCountTextVariable.value = turnCounter.ToString();
            isComplete = GetTurns(turnIndex).Execute();
            if (!IsMultiplay)
            {
                _CurrentPlayer = GetTurns(turnIndex).ThisTurnPlayer;
                if (isComplete)
                {
                    turnIndex++;
                    if (turnIndex > _TurnLength - 1)
                    {
                        turnIndex = 0;
                        GetTurns(turnIndex).TurnBegin = startTurn;
                        turnCounter++;
                    }

                    startTurn = true;
                    turnText.value = GetTurns(turnIndex).ThisTurnPlayer.ToString();
                    switchPlayer = true;
                    OnTurnChanged.Raise();
                }
            }
            else
            {
                if (isComplete)
                {
                    MultiplayManager.singleton.PlayerEndsTurn(CurrentPlayer.PhotonId);
                }
            }

            if (_CurrentState != null)
                _CurrentState.Tick(Time.deltaTime);
        }

        public int GetAnotherPlayerID()
        {
            int r = turnIndex;
            r++;
            if (r > _TurnLength - 1)
            {
                r = 0;
            }
            return GetTurns(r).ThisTurnPlayer.PhotonId;
        }
        public int GetPlayerTurnIndex(int photonId)
        {
            for (int i = 0; i < _TurnLength; i++)
            {
                if (GetTurns(i).ThisTurnPlayer.PhotonId == photonId)
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
        public void SetState(State state)
        {
            _CurrentState = state;
        }
        public void EndPhase()//Run by EndTurn button
        {
            if (CurrentPlayer.isHumanPlayer)
            {
                Setting.RegisterLog(_CurrentPlayer.name + " finished phase: " + GetTurns(turnIndex).CurrentPhase.value.name, _CurrentPlayer.playerColor);
                GetTurns(turnIndex).EndCurrentPhase();

            }
            //_CurrentPlayer = GetTurns(turnIndex).ThisTurnPlayer;
        }
        public void PutCardToGrave(CardInstance c)
        {
            _CardGrave.PutCardToGrave(c);
        }
    }
}