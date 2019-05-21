using GH.GameStates;
using System.Collections.Generic;
using UnityEngine;

namespace GH
{
    public class GameController : MonoBehaviour
    {
        public PlayerHolder currentPlayer;
        public CardHolders bottomCardHolder;
        public CardHolders topCardHolder;
        public State currentState;
        public Phase currentPhase;
        public Turn[] turns;

        //public AssignPlayer[] gameobjPlayerAssigned;
        public PlayerStatsUI[] playerStats;


        public GH.GameEvent OnTurnChanged;
        public GH.GameEvent OnPhaseChanged;
        public GH.StringVariable turnText;
        public GH.StringVariable turnCountTextVariable;//Count the turn. When both player plays, it increases by 1


        public GameObject cardPrefab;
        public GameObject[] manaObj;

        public bool switchPlayer;
        public int turnIndex;


        public static GameController singleton;


        public  PlayerHolder[] allPlayers; // Don't modify ever
        [System.NonSerialized]
        public bool isComplete = false;
        [System.NonSerialized]
        public CheckPlayerCanUse checkObjOwner = new CheckPlayerCanUse();


        private readonly ManaHolder tempManaHolder;

        private bool startTurn = true; //Check the start of the turn
        private int turnCounter; //Count the turn. When both player plays, it increases by 1

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

            if(!b.defenders.Contains(def))
            {
                Debug.Log("ADD blocker");
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
            allPlayers = new PlayerHolder[turns.Length];
            for(int i =0; i< turns.Length; i++)
            {
                allPlayers[i] = turns[i].player;
                if (allPlayers[i].player == "Player1")
                    allPlayers[i].isBottomPos = true;
                else
                    allPlayers[i].isBottomPos = false;

                Setting.RegisterLog(allPlayers[i].name+" joined the game successfully", allPlayers[i].playerColor);
            }
            currentPlayer = turns[0].player;
        }

        private void Start()
        {
            Setting.gameController = this;

            SetupPlayers();

            CreateStartingCards(); //Each player gets their starting cards

            turnText.value = turns[turnIndex].player.ToString(); // Visualize whose turn is now

            turnCounter = 1; 
            turnCountTextVariable.value = turnCounter.ToString();

            OnTurnChanged.Raise();
            for(int i=0;i<allPlayers.Length;i++)
            {
                allPlayers[i].statsUI = playerStats[i];
                allPlayers[i].manaResourceManager.InitManaZero();
                playerStats[i].UpdateMana();
                /*
                 *Initialise mana as 0
                 *This is for the speical mode that might exist in future
                 *      Ex) Initialise mana as 3 
                 */

            }

        }              

        private void SetupPlayers()
        {
            for(int i=0;i<allPlayers.Length;i++)
            {
                allPlayers[i].Init();
                if (i == 0)
                    allPlayers[i].currentCardHolder = bottomCardHolder; // Player 1 is bottom card holder
                else
                    allPlayers[i].currentCardHolder = topCardHolder;  // Player 2 is top card holder

                if (i<2)
                {
                    allPlayers[i].statsUI = playerStats[i];
                    playerStats[i].player.LoadPlayerOnStatsUI();                   
                }
            }
        }

        public void LoadPlayerOnActive(PlayerHolder loadedPlayer)
        {
            //At first run, bottomcardholder is player1, topCardHolder is player2
            PlayerHolder prevPlayer = topCardHolder.thisPlayer;

            
            if(loadedPlayer == topCardHolder.thisPlayer)
            //It is run when player turn(or position) is changed
            {
                //playerStats[0] = UIs at bottom / playerStats[1] = UIs at top
                prevPlayer = bottomCardHolder.thisPlayer;

                LoadPlayerOnHolder(prevPlayer, allPlayers[1].currentCardHolder, playerStats[0]); // move bottom player's UI, cards to top                
                LoadPlayerOnHolder(loadedPlayer, allPlayers[0].currentCardHolder, playerStats[1]);
                ///////////////////////////////////
                //LoadPlayerOnHolder(prevPlayer, topCardHolder, playerStats[0]);           
                //LoadPlayerOnHolder(loadedPlayer, bottomCardHolder, playerStats[1]);
                //Above code doesn't work, because topCardHolder and bottomCardHolder aren't get changed yet when loadedPlayer is changed
                //Example)
                //loaded Player is "player1" *** TopPlayer = "player1" *** BottomPlayer = "player2"
                //in this case, line 171,172 don't have meaning except moving cards in its origninal place.
                ///////////////////////////////////////
                
                if(turns[turnIndex].index != 2) // 3rd Phase is blockphase___ On block phase, infinite loop exists.
                {
                    topCardHolder = prevPlayer.currentCardHolder;
                    bottomCardHolder = loadedPlayer.currentCardHolder;
                }
               /* else
                    Debug.Log("CHECK");*/
            }
            else if (loadedPlayer == bottomCardHolder.thisPlayer && loadedPlayer.player == "Player2")
            //It is only run at battle phase
            {
                Debug.Log("Loop2 + loadedPlayer is "+loadedPlayer);
                prevPlayer = topCardHolder.thisPlayer;
                LoadPlayerOnHolder(prevPlayer, bottomCardHolder, playerStats[1]); // move bottom player's UI, cards to top
                LoadPlayerOnHolder(loadedPlayer, topCardHolder, playerStats[0]);

            }
            else if(loadedPlayer != topCardHolder.thisPlayer && loadedPlayer != bottomCardHolder.thisPlayer)
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

        public void PickNewCardFromDeck(PlayerHolder p )
        {
            ResourceManager rm = Setting.GetResourceManager();

            if (p.allCards.Count==0)
            {
                Setting.RegisterLog(p + " don't have card in deck", Color.black);
                return;
            }
            string cardId = p.allCards[0];
            p.allCards.RemoveAt(0);
            GameObject go = Instantiate(cardPrefab) as GameObject;
            CardViz v = go.GetComponent<CardViz>();
            v.LoadCard(rm.GetCardInstance(cardId));

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
                Setting.RegisterLog("Can't add card. Next card is deleted",Color.black);
            //LoadPlayerOnHolder(p, p.currentCardHolder, p.statsUI);
        }
       

        // Move targetPlayer, targetUI's position to destCardHolder's position
        public void LoadPlayerOnHolder(PlayerHolder targetPlayer, CardHolders destCardHolder,PlayerStatsUI targetUI)
        {
            destCardHolder.LoadPlayer(targetPlayer, targetUI);
        }
        private void CreateStartingCards()
        {
            //ResourceManager rm = Setting.GetResourceManager();

            //for(int p =0;p<allPlayers.Length; p++)
            //{
            //    Setting.RegisterLog(allPlayers[p].name + " loading . . . ", allPlayers[p].playerColor);
            //    for (int i = 0; i < allPlayers[p].startingCards.Length; i++)
            //    {
            //        GameObject go = Instantiate(cardPrefab) as GameObject;
            //        CardViz v = go.GetComponent<CardViz>();

            //        v.LoadCard(rm.GetCardInstance(allPlayers[p].startingCards[i]));


            //        CardInstance inst = go.GetComponent<CardInstance>();
            //        inst.currentLogic = allPlayers[p].handLogic;
            //        Setting.SetParentForCard(go.transform, allPlayers[p].currentCardHolder.handGrid.value);
            //        allPlayers[p].handCards.Add(inst);
            //    }

            //    //allPlayers[p].currentCardHolder.LoadPlayer(allPlayers[p],allPlayers[p].statsUI);
            //    Setting.RegisterLog("Card Created for " + allPlayers[p].name, allPlayers[p].playerColor);
            //}
        }
             
        private void UpdateMana()
        {
            //int playerIndex = -1;

            //for ( int i =0; i<manaObj.Length;i++)
            //{
            //    tempManaHolder = manaObj[i].GetComponent<ManaHolder>();
            //    PlayerHolder thisPlayer = manaObj[i].GetComponentInParent<AssignPlayer>().GetPlayer();
                
            //    if (currentPlayer == thisPlayer)
            //    {
            //        playerIndex = i;
            //        break;
            //    }
            //}
            //if (playerIndex == -1)
            //    return;

            //tempManaHolder.currentMana.text = currentPlayer.manaResourceManager.getCurrentMana().ToString();
            //tempManaHolder.maxMana.text = currentPlayer.manaResourceManager.getMaxMana().ToString();       


            for(int i =0;i<playerStats.Length;i++)
            {
                if(currentPlayer == playerStats[i].player)
                {
                    playerStats[i].UpdateMana();
                }
            }
        }

        private void Update()
        {
            //if (switchPlayer)
            //{
            //    playerOnHolder.LoadPlayer(allPlayers[1], playersStats[1]);
            //    otherPlayerOnHolder.LoadPlayer(allPlayers[0], playersStats[0]);
            //    switchPlayer = false;
            //}        


            UpdateMana();
            if (startTurn)
            {
                turns[turnIndex].firstTime = startTurn;
                startTurn = false;
            }


            if (switchPlayer)
            {
                Setting.RegisterLog("Position Changed!! ", Color.green);
                ///Switch UI's position.
                Vector3 t = playerStats[0].gameObject.transform.position;
                playerStats[0].gameObject.transform.position = playerStats[1].gameObject.transform.position;
                playerStats[1].gameObject.transform.position = t;

                Vector3 m = manaObj[0].gameObject.transform.position;
                manaObj[0].gameObject.transform.position = manaObj[1].gameObject.transform.position;
                manaObj[1].gameObject.transform.position = m;
                               
                switchPlayer = false;
            }
        
            turnCountTextVariable.value = turnCounter.ToString();

            
            isComplete = turns[turnIndex].Execute();

            if (isComplete)
            {
                turnIndex++;
                if (turnIndex > turns.Length -1)
                {
                    turnIndex = 0;
                    turns[turnIndex].firstTime = startTurn;
                    turnCounter++;
                }

                startTurn = true;
                currentPlayer = turns[turnIndex].player;
                turnText.value = turns[turnIndex].player.ToString();

                switchPlayer = true;
                OnTurnChanged.Raise();

            }

            if (currentState !=null)
                currentState.Tick(Time.deltaTime);
            
           
        }

        public void SetState(State state)
        {
            currentState = state;
        }
        
        public void EndPhase()
        {
            Setting.RegisterLog(currentPlayer.name + " finished phase: " + turns[turnIndex].currentPhase.value, currentPlayer.playerColor);
            turns[turnIndex].EndCurrentPhase();
            currentPlayer = turns[turnIndex].player;
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

    }
}
