#pragma warning disable 0649
using GH.GameStates;
using System.Collections.Generic;
using UnityEngine;
using GH.GameCard;
using GH.GameTurn;

namespace GH
{
    public class GameController : MonoBehaviour
    {
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
        private int _TurnLength = 0;
        [SerializeField]
        private PlayerStatsUI[] _PlayerStatsUI;


        public GH.GameEvent OnTurnChanged;
        public GH.GameEvent OnPhaseChanged;
        public GH.StringVariable turnText;
        public GH.StringVariable turnCountTextVariable;//Count the turn. When both player plays, it increases by 1
        public GH.TransformVariable[] graveyard_transform;

        public GameObject cardPrefab;
        public GameObject[] manaObj;

        private bool switchPlayer;
        public int turnIndex=0;


        public static GameController singleton;


        public PlayerHolder[] allPlayers; // Don't modify ever
        [System.NonSerialized]
        public bool isComplete = false;
        [System.NonSerialized]
        public CheckPlayerCanUse checkObjOwner = new CheckPlayerCanUse();


        private bool startTurn = true; //Check the start of the turn
        private int turnCounter; //Count the turn. When both player plays, it increases by 1


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
        public PlayerStatsUI GetPlayerStatsUI(int i)
        {
            return _PlayerStatsUI[i];
        }

        Dictionary<CardInstance, BlockInstance> blockInstDic = new Dictionary<CardInstance, BlockInstance>();

        public Dictionary<CardInstance, BlockInstance> GetBlockInstances()
        {
            return blockInstDic;
        }

        public void ClearBlockInstances()
        {
            blockInstDic.Clear();
        }

        public void AddBlockInstance(CardInstance attk, CardInstance def, ref int count)
        {

            BlockInstance b = null;
            b = GetBlockInstanceOfAttacker(attk);
            if (b == null)
            {
                b = new BlockInstance();
                b.attacker = attk;

                blockInstDic.Add(attk, b);
            }

            if (!b.defenders.Contains(def))
            {
                b.defenders.Add(def);
            }
            count = b.defenders.Count;
        }

        BlockInstance GetBlockInstanceOfAttacker(CardInstance attck)
        {
            BlockInstance r = null;
            blockInstDic.TryGetValue(attck, out r);
            return r;

        }


        private void Awake()
        {
            switchPlayer = false;
            singleton = this;
            _TurnLength = _Turns.Length;
            allPlayers = new PlayerHolder[_TurnLength];
            for (int i = 0; i < _TurnLength; i++)
            {
                allPlayers[i] = GetTurns(i).ThisTurnPlayer;
                if (allPlayers[i].player == "Player1")
                    allPlayers[i].isBottomPos = true;
                else
                    allPlayers[i].isBottomPos = false;

                Setting.RegisterLog(allPlayers[i].name + " joined the game successfully", allPlayers[i].playerColor);
            }
            Debug.Log("First TurnIndex is " + turnIndex);
            _CurrentPlayer = GetTurns(0).ThisTurnPlayer;
            //_BottomCardHolder = GetTurns(0).ThisTurnPlayer.currentCardHolder;
            //_TopCardHolder = GetTurns(1).ThisTurnPlayer.currentCardHolder;
        }

        private void Start()
        {
            Setting.gameController = this;

            SetupPlayers();

            //CreateStartingCards(); //Each player gets their starting cards

            turnText.value = GetTurns(turnIndex).ThisTurnPlayer.ToString(); // Visualize whose turn is now

            turnCounter = 1;
            turnCountTextVariable.value = turnCounter.ToString();

            OnTurnChanged.Raise();
            for (int i = 0; i < allPlayers.Length; i++)
            {
                allPlayers[i].statsUI = GetPlayerStatsUI(i);
                allPlayers[i].manaResourceManager.InitManaZero();
                GetPlayerStatsUI(i).UpdateManaUI();
                /*
                 *Initialise mana as 0
                 *This is for the speical mode that might exist in future
                 *      Ex) Initialise mana as 3 
                 */

            }

        }

        private void SetupPlayers()
        {
            for (int i = 0; i < allPlayers.Length; i++)
            {
                allPlayers[i].Init();
                if (i == 0)
                    allPlayers[i].currentCardHolder = _BottomCardHolder; // Player 1 is bottom card holder
                else
                    allPlayers[i].currentCardHolder = _TopCardHolder;  // Player 2 is top card holder

                if (i < 2)
                {
                    allPlayers[i].statsUI = GetPlayerStatsUI(i);
                    GetPlayerStatsUI(i).player.LoadPlayerOnStatsUI();
                }
            }
        }

        public void LoadPlayerOnActive(PlayerHolder loadedPlayer)
        {
            //At first run, bottomcardholder is player1, topCardHolder is player2
            PlayerHolder prevPlayer = _TopCardHolder.thisPlayer;


            if (loadedPlayer == _TopCardHolder.thisPlayer)
            //It is run when player turn(or position) is changed
            {
                //playerStats[0] = UIs at bottom / playerStats[1] = UIs at top
                prevPlayer = _BottomCardHolder.thisPlayer;

                LoadPlayerOnHolder(prevPlayer, allPlayers[1].currentCardHolder, _PlayerStatsUI[0]); // move bottom player's UI, cards to top                
                LoadPlayerOnHolder(loadedPlayer, allPlayers[0].currentCardHolder, _PlayerStatsUI[1]);
                ///////////////////////////////////
                //LoadPlayerOnHolder(prevPlayer, topCardHolder, playerStats[0]);           
                //LoadPlayerOnHolder(loadedPlayer, bottomCardHolder, playerStats[1]);
                //Above code doesn't work, because topCardHolder and bottomCardHolder aren't get changed yet when loadedPlayer is changed
                //Example)
                //loaded Player is "player1" *** TopPlayer = "player1" *** BottomPlayer = "player2"
                //in this case, line 171,172 don't have meaning except moving cards in its origninal place.
                ///////////////////////////////////////

                if (GetTurns(turnIndex).PhaseIndex != 2) // 3rd Phase is blockphase___ On block phase, infinite loop exists.
                {
                    _TopCardHolder = prevPlayer.currentCardHolder;
                    _BottomCardHolder = loadedPlayer.currentCardHolder;
                }
                /* else
                     Debug.Log("CHECK");*/
            }
            else if (loadedPlayer == _BottomCardHolder.thisPlayer && loadedPlayer.player == "Player2")
            //It is only run at battle phase
            {
                Debug.Log("Loop2 + loadedPlayer is " + loadedPlayer);
                prevPlayer = _TopCardHolder.thisPlayer;
                LoadPlayerOnHolder(prevPlayer, _BottomCardHolder, _PlayerStatsUI[1]); // move bottom player's UI, cards to top
                LoadPlayerOnHolder(loadedPlayer, _TopCardHolder, _PlayerStatsUI[0]);

            }
            else if (loadedPlayer != _TopCardHolder.thisPlayer && loadedPlayer != _BottomCardHolder.thisPlayer)
            {
                Debug.LogError("loaded player isn't at bottom nor top");
            }
            ////What if current player is Player2 and bottomCardHolder is Player2 also
            //if (bottomCardHolder.thisPlayer.player == "Player2" || loadedPlayer.player == "Player2")
            //{
            //    //if(prevPlayer == loadedPlayer)
            //    prevPlayer = bottomCardHolder.thisPlayer;

            //    LoadPlayerOnHolder(prevPlayer, topCardHolder, playersStats[0]);  //Move top player's cards to bottom

            //    // Move top Player's cards to bottom
            //    if (loadedPlayer.player == "Player2")
            //        LoadPlayerOnHolder(loadedPlayer, bottomCardHolder, playersStats[1]);
            //    else
            //        LoadPlayerOnHolder(loadedPlayer, loadedPlayer.currentCardHolder, playersStats[1]);

            //    bottomCardHolder = loadedPlayer.currentCardHolder;
            //}
        }
        /// <summary>
        /// Pick top card from deck
        /// Issue: Does this function should be at Gamecontroller?
        /// </summary>
        /// <param name="p"></param>
        public void PickNewCardFromDeck(PlayerHolder p)
        {
            ResourceManager rm = Setting.GetResourceManager();

            if (p.allCards.Count == 0)
            {
                Setting.RegisterLog(p + " don't have card in deck", Color.black);
                return;
            }
            string cardId = p.allCards[0];
            p.allCards.RemoveAt(0);
            GameObject go = Instantiate(cardPrefab) as GameObject;
            CardViz v = go.GetComponent<CardViz>();
            v.LoadCard(rm.GetCardFromDict(cardId));

            CardInstance inst = go.GetComponent<CardInstance>();
            inst.owner = p;
            inst.currentLogic = p.handLogic;

            //In single play, player who control card is always at bottom position. 
            //So when player draw card, place the card at bottom and add list on current player.
            Setting.SetParentForCard(go.transform, allPlayers[0].currentCardHolder.handGrid.value);
            //Debug.Log("Card added");
            if (p.handCards.Count <= 7)
                p.handCards.Add(inst);
            else
                Setting.RegisterLog("Can't add card. Next card is deleted", Color.black);
            //LoadPlayerOnHolder(p, p.currentCardHolder, p.statsUI);
        }
        // Move targetPlayer, targetUI's position to destCardHolder's position
        public void LoadPlayerOnHolder(PlayerHolder targetPlayer, CardHolders destCardHolder, PlayerStatsUI targetUI)
        {
            destCardHolder.LoadPlayer(targetPlayer, targetUI);
        }       
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
            _CurrentPlayer = GetTurns(turnIndex).ThisTurnPlayer;
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
            
            if (isComplete)
            {
                Debug.Log("ISCOMPLET");
                turnIndex++;
                if (turnIndex > _TurnLength-1 )
                {
                    turnIndex = 0;
                    GetTurns(turnIndex).TurnBegin = startTurn;
                    turnCounter++;
                }

                startTurn = true;
                //currentPlayer = turns[turnIndex].ThisTurnPlayer;
                turnText.value = GetTurns(turnIndex).ThisTurnPlayer.ToString();
                switchPlayer = true;
                OnTurnChanged.Raise();
            }

            if (_CurrentState != null)
                _CurrentState.Tick(Time.deltaTime);
        }
        public void SetState(State state)
        {
            _CurrentState = state;
        }
        public void EndPhase()
        {
            Setting.RegisterLog(_CurrentPlayer.name + " finished phase: " + GetTurns(turnIndex).CurrentPhase.value.name, _CurrentPlayer.playerColor);

            GetTurns(turnIndex).EndCurrentPhase();
            //_CurrentPlayer = GetTurns(turnIndex).ThisTurnPlayer;
        }
        public PlayerHolder GetOpponentOf(PlayerHolder p)
        {
            for (int i = 0; i < allPlayers.Length; i++)
            {
                if (allPlayers[i] != p)
                    return allPlayers[i];
            }
            return null;

        }
        public void PutCardToGrave(CardInstance c)
        {
            PlayerHolder cardOwner = c.owner;
            GameObject graveyardObj = null;
            cardOwner.graveyard.Add(c);

            for (int i = 0; i < allPlayers.Length; i++)
            {
                if (allPlayers[i] == cardOwner)
                {
                    Debug.Log("check  " + cardOwner);
                    allPlayers[i] = cardOwner;
                }
            }
            if (c.owner.player == "Player1")
                graveyardObj = graveyard_transform[0].value.gameObject;
            else if (c.owner.player == "Player2")
                graveyardObj = graveyard_transform[1].value.gameObject;


            if (graveyardObj == null)
            {
                Debug.Log("Failed to check obj");
            }
            else
            {
                Debug.Log("Found graveyardOBj : " + graveyardObj.transform);
                Setting.SetParentForCard(c.transform, graveyardObj.transform);
            }

            if (cardOwner.fieldCard.Contains(c))
            {
                cardOwner.fieldCard.Remove(c);
            }

            if (cardOwner.handCards.Contains(c))
            {
                cardOwner.handCards.Remove(c);
            }

            if (cardOwner.attackingCards.Contains(c))
            {
                cardOwner.attackingCards.Remove(c);
            }
            c.dead = true;
            c.gameObject.SetActive(false);
            c.gameObject.GetComponentInChildren<CardInstance>().enabled = false;
            c.currentLogic = null;
        }
    }
}
