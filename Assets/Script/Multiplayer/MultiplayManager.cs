
using GH.GameCard;
using GH.GameCard.CardInfo;
using GH.GameCard.CardLogics;
using GH.Player;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using GH.GameCard.CardElement;
namespace GH.Multiplay

{
    /// <summary>
    /// Responsible for sending all events
    /// </summary>
    public class MultiplayManager : Photon.MonoBehaviour
    {
        #region Variables
        [SerializeField]
        private MainDataHolder _MainDataHolder;
        public static MultiplayManager singleton;
        private List<NetworkPrint> _Players = new List<NetworkPrint>();
        private NetworkPrint localPlayerNWPrint;
        private Transform _MultiplayerReferences;
        //Now the game is on between two players so List might not be best way.

        //[SerializeField]
        //private PlayerHolder localPlayerHolder;
        //[SerializeField]
        //private PlayerHolder clientPlayerHolder;
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
        public void AddPlayer(NetworkPrint nw_print)
        {
            if (nw_print.IsLocal)
            {
                Debug.Log("AddNetworkPrint: Set local network print");
                localPlayerNWPrint = nw_print;
            }
            AddPlayerNetworkPrint = nw_print;
            nw_print.transform.parent = MultiplayerReferences;
        }
        public NetworkPrint AddPlayerNetworkPrint
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
        public MainDataHolder MainData
        {
            get { return _MainDataHolder; }
        }
        public NetworkPrint GetPlayer(int pId)
        {
            for (int i = 0; i < Players.Count; i++)
            {
                if (Players[i].photonId == pId)
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
            PhotonNetwork.Instantiate("NetworkPrint", Vector3.zero, Quaternion.identity, 0/*, data*/);
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
            ResourceManager rm = GC.ResourceManager;

            if (NetworkManager.IsMaster)
            {
                Debug.Log("NOTIFICATION: This client is room manager client");
                List<int> playerId = new List<int>();
                List<int> cardInstId = new List<int>();
                List<string> cardName = new List<string>();
                foreach (NetworkPrint p in Players)
                {
                    foreach (string id in p.GetStartingCardids)
                    {
                        Card c = rm.GetCardInstFromDeck(id);
                        playerId.Add(p.photonId);                        
                        cardInstId.Add(c.Data.UniqueId);
                        cardName.Add(id);

                        if (p.IsLocal)
                        {
                            p.ThisPlayer = GC.LocalPlayer;
                            p.ThisPlayer.InGameData.SetPhotonId = p.photonId;
                            GC.LocalPlayer.SetPlayerProfile(p.PlayerProfile);
                            GC.LocalPlayer.PlayerProfile.Name = "Room Manager";
                        }
                        else
                        {
                            p.ThisPlayer = GC.ClientPlayer;
                            p.ThisPlayer.InGameData.SetPhotonId = p.photonId;
                            GC.ClientPlayer.SetPlayerProfile(MainData.GetClientProfile);
                            GC.ClientPlayer.PlayerProfile.Name = "Room Member_Client";
                        }
                    }
                }
                photonView.RPC("RPC_InitGame", PhotonTargets.All, 1);
                for (int i = 0; i < playerId.Count; i++)
                {
                    photonView.RPC("RPC_PlayerCreatesCard", PhotonTargets.All, playerId[i], cardInstId[i], cardName[i]);
                }
            }

            else
            {
                Debug.Log("NOTIFICATION: This client is room guest client");                
                foreach (NetworkPrint p in Players)
                {
                    if (p.IsLocal)
                    {
                        p.ThisPlayer = GC.LocalPlayer;
                        p.ThisPlayer.InGameData.SetPhotonId = p.photonId;
                        GC.LocalPlayer.SetPlayerProfile(p.PlayerProfile);
                        GC.LocalPlayer.PlayerProfile.Name = "Room Member";
                    }
                    else
                    {
                        p.ThisPlayer = GC.ClientPlayer;
                        p.ThisPlayer.InGameData.SetPhotonId = p.photonId;
                        GC.ClientPlayer.SetPlayerProfile(MainData.GetClientProfile);
                        GC.ClientPlayer.PlayerProfile.Name = "Room Manager_Client";
                    }
                }
            }
        }

        /// <summary>
        /// Add Player cards 
        /// </summary>
        /// <param name="photonId"></param>
        /// <param name="cardId"></param>
        /// <param name="cardName"></param>
        [PunRPC]
        public void RPC_PlayerCreatesCard(int photonId, int cardId, string cardName)
        {
            Card c = GC.ResourceManager.GetCardInstFromDeck(cardName);
            if(c==null)
            {
                Debug.LogError("CardNameNotFound: " + cardName);
            }
            c.Data.SetUniqueId = cardId;
            NetworkPrint p = GetPlayer(photonId);
            p.ThisPlayer.CardManager.AddCardInDeck(c);
        }

        [PunRPC]
        public void RPC_InitGame(int startingPlayer)
        {
            GC.IsMultiplay = true;
            GC.InitGame(startingPlayer);
        }


        #endregion

        #region End Turn
        public void PlayerEndsTurn(int photonId)//photon Id is player who  ends the turn -> we need next one.
        {
            photonView.RPC("RPC_PlayerEndsTurn", PhotonTargets.All, photonId);
        }

        [PunRPC]
        public void RPC_PlayerEndsTurn(int photonId)
        {
            Debug.LogFormat("RPC_PlayerEndsTurn: {0} ends turn.", GC.CurrentPlayer.PlayerProfile.UniqueId);

            if (photonId == GC.CurrentPlayer.InGameData.PhotonId)
            {
                int targetId = GC.GetAnotherPlayerID();
                photonView.RPC("RPC_PlayerStartsTurn", PhotonTargets.All, targetId);    
                //if (NetworkManager.IsMaster)
                //{
                //    int targetId = GC.GetAnotherPlayerID();
                //    photonView.RPC("RPC_PlayerStartsTurn", PhotonTargets.All, targetId);
                //}
                //Uncommenting above codes will allow only Network Master run both start turn actions. 
            }
            else
            {
                Debug.LogError("RPC_Turn: Current player can't finish turn");
            }

        }

        [PunRPC]
        public void RPC_PlayerStartsTurn(int photonId)
        {
            GC.ChangeCurrentTurn(photonId);
        }
        #endregion


        #region Card Operations

        public void PlayerPicksCardFromDeck(PlayerHolder playerHolder)
        {
            Debug.Log("PlayerPicksCard");
            NetworkPrint p = GetPlayer(playerHolder.InGameData.PhotonId);
            for (int i = 0; i < p.PlayerProfile._DeckToPlay.Cards.Length; i++)
            {
                p.AddCardToDeck(p.PlayerProfile._DeckToPlay.Cards[i]);
            }
            Card c = null;
            if (p.CardDeck.Count != 0)
            {
                c = p.CardDeck[0];
                p.CardDeck.RemoveAt(0);
                Debug.Log("Draw card");
                photonView.RPC("RPC_PlayerPickCard", PhotonTargets.All, c.Data.UniqueId, p.photonId);
            }
            else
                Debug.Log("There is no card in deck");
        }
        [PunRPC]
        private void RPC_PlayerPickCard(int cardUniqueId, int photonId)
        {
            PlayerHolder currentPlayer = GetPlayer(photonId).ThisPlayer;
            Card card = currentPlayer.CardManager.SearchCard(cardUniqueId);
            if (card == null)
            {
                Debug.LogError("CardISNULL");
                return;
            }
            Debug.Log("AddCard");
            GameObject go = Instantiate(MainData.CardPrefab) as GameObject;
            CardAppearance visual = go.GetComponent<CardAppearance>();
            visual.LoadCard(card, go);
            card.Init(go);
            card.User = currentPlayer;
            MoveCardInstance.SetParentForCard(go.transform, currentPlayer.CardTransform.HandGrid.value);
            if (currentPlayer.CardManager.handCards.Count <= 7)
                currentPlayer.CardManager.handCards.Add(card.Data.UniqueId);
            else
                Setting.RegisterLog("Can't add card. Next card is deleted", Color.black);
        }

        #endregion

        #region Battle Resolve

        public void SetBattleResolvePhase()
        {
            photonView.RPC("RPC_BattleResolve", PhotonTargets.MasterClient);
        }
        [PunRPC]
        public void RPC_BattleResolve()
        {
            //Debug.LogFormat("RPC_BattleResolve_CurrentPlayer: {0}. Network Master Check: {1}",GC.CurrentPlayer.player,NetworkManager.IsMaster);
            if (!NetworkManager.IsMaster)
            {
                Debug.LogError("This Player is not network master. Battle Resolve End");
                //Setting.gameController.EndPhaseByBattleResolve();
                return;
            }
            else
            {
                //Debug.Log("This Player is Network Master. Battle Resolve Starts");
                BattleResolveForPlayers();
            }
        }
        private void BattleResolveForPlayers()
        {
            CardBattle.BattleLogics battleLogic = new CardBattle.BattleLogics();
            PlayerHolder currentPlayer = Setting.gameController.CurrentPlayer;
            PlayerHolder enemyPlayer = Setting.gameController.GetOpponentOf(currentPlayer);

            Element elementAttack = MainData.AttackElement;
            Element elementHealth = MainData.HealthElement;
            int battleResult = 0;

            //Debug.LogFormat ("CurrentPlayer is {0}. Is he owns photonview?: {1}", currentPlayer.player ,photonView.isMine);
            //When there is no attacking card, Battle resolve don't need to be run. End the phase
            if (enemyPlayer.CardManager.attackingCards.Count == 0)
            {
                photonView.RPC("RPC_BattleResolvesCallBack", PhotonTargets.All, currentPlayer.InGameData.PhotonId);
                return;
            }


            //Get defending cards from the manager
            //Dictionary<CardInstance, BlockInstance> defDic = gc.BlockManager.BlockInstDict;
            Dictionary<Card, BlockInstance> blockInstDict = GC.BlockManager.BlockInstDict;
            if (blockInstDict == null)
            {
                Debug.LogWarning("BattleResolve_Error: Defending card instance dictionary is null");
                return;
            }


            //Every current player's attacking card, 
            for (int atkCardIndex = 0; atkCardIndex < enemyPlayer.CardManager.attackingCards.Count; atkCardIndex++)
            {
                int atkId = enemyPlayer.CardManager.attackingCards[atkCardIndex];
                CreatureCard atkInst = (CreatureCard)enemyPlayer.CardManager.SearchCard(atkId);
                if(atkInst==null)
                {
                    Debug.LogError("FailCastingToCreature: Attacking card failed change to 'CreatureCard");
                }
                BlockInstance blockInstance = GC.BlockManager.GetBlockInstanceByAttacker(atkInst, blockInstDict);
                
                battleResult = battleLogic.CardBattle(atkInst, blockInstance, MainData);
         
                if(battleResult == -1)
                {
                    continue;
                }
                //BattleResult is remaining attack damage after destroying all block cards.
                //After remaining damage is dealt to user, card goes back to its original place.
                if(battleResult>0)
                {
                    battleLogic.AttackerWinFight(atkInst, battleResult);
                    photonView.RPC("RPC_SyncPlayerHealth", PhotonTargets.All, currentPlayer.InGameData.PhotonId, currentPlayer.InGameData.Health);
                    Setting.RegisterLog("Attack damage is " + battleResult, Color.red);
                }
            }

            //Return all alive blocking cards to its original field location
            foreach (Card c in blockInstDict.Keys)
            {
                if (c.CardCondition.IsDead)
                {
                    Debug.LogWarningFormat("BattleResolve_BlockDictionary: {0} is dead", c.Data.Name);
                    continue;
                }
                else
                {
                    Debug.LogFormat("BattleResolveForPlayer_BattleFinished: Card ( {0} ) Reset. Move Card to its original field location", c.Data.Name);
                    c.CardCondition.CanUse  = true;
                    MoveCardInstance.SetParentForCard(c.PhysicalCondition.transform, c.PhysicalCondition.GetOriginFieldLocation());
                }
            }
            if(enemyPlayer.CardManager.fieldCards.Count!=0)
            {
                foreach (int id in enemyPlayer.CardManager.fieldCards)
                {
                    Card c = enemyPlayer.CardManager.SearchCard(id);
                    if(c.CardCondition.IsDead)
                    {
                        Debug.LogErrorFormat("BattleResolve_EnemyFieldCardError: {0} is dead__owner is {1}", c.Data.Name
                            ,c.User.PlayerProfile.UniqueId);
                        continue;
                    }
                    else
                    {
                        Debug.LogFormat("BattleResolveForPlayer_ResetEnemyCard__{0}", c.Data.Name);
                        MoveCardInstance.SetParentForCard(c.PhysicalCondition.transform, c.PhysicalCondition.GetOriginFieldLocation());
                    }
                }
            }

            Debug.Log("BattleResolveForPlayer: EndOfCode_CallBack_Run");
            //After all battle logics are finished, run callback to clean up remaining variables
            photonView.RPC("RPC_BattleResolvesCallBack", PhotonTargets.All, currentPlayer.InGameData.PhotonId);
            return;
        }

        public void SendCardToGrave(int photonId)
        {
            photonView.RPC("RPC_SendCardToGrave", PhotonTargets.All, photonId);
        }

        [PunRPC]
        public void RPC_SendCardToGrave(int photonId)
        {
            PlayerHolder p = GetPlayer(photonId).ThisPlayer;

            foreach (int id in p.CardManager.deadCards)
            {
                Card c = p.CardManager.SearchCard(id);
                Transform graveyardTransform = c.User.CardTransform.Graveyard.value;
                Debug.LogFormat("RPC_SendCardToGrave: Player({0})'s {1} is dead. It's going to graveyard: {2}", 
                    p.PlayerProfile.UniqueId, c.Data.Name,graveyardTransform);
                CardPlayManager.MoveCardToGrave(c, graveyardTransform);
            }
        }

        [PunRPC]
        public void RPC_SyncPlayerHealth(int photonId, int health)            
        {
            NetworkPrint nw_player = GetPlayer(photonId);
            nw_player.ThisPlayer.InGameData.Health = health;
        }


        [PunRPC]
        public void RPC_BattleResolvesCallBack(int photonId)
        {
            foreach(NetworkPrint p in Players)
            {
                //Enemy player, who tryed to attack on his/her last turn needs their attacking cards reset                
                //Move all enemy attacking cards to its original position and set visual of card to 'cant use'                
                foreach (int instId in p.ThisPlayer.CardManager.attackingCards)
                {
                    Card c = p.ThisPlayer.CardManager.SearchCard(instId);
                    Debug.LogFormat("{0} was attacking. Call back", c.Data.Name);
                    p.ThisPlayer.CardTransform.SetCardBackToOrigin(c);

                    c.CardCondition.CanUse = false;
                }

                //Find Current Player NetworkPrint
                if (p.photonId == photonId)
                {
                    if (p == localPlayerNWPrint)
                    {
                        Debug.Log("RPC_BattleResolvesCallBack: End");
                        //As Current Player, who is defender don't have attacking card, can finish phase here
                        Setting.gameController.EndPhaseByBattleResolve();

                    }
                    else
                    {
                        Debug.Log("RPC_BattleResolveCallBack: FailedToEnd_ThisPlayerIsNotLocalPlayer");
                    }
                }
                //Debug.LogWarningFormat("RPC_BattleResolveCallback: {0}'s attacking cards are going to be cleared.", p.ThisPlayer);
                //Clear all attacking cards.
                p.ThisPlayer.CardManager.attackingCards.Clear();       
                
            }
            //Debug.Log("RPC_BattleResolveCallBack: Block Inst Dic size: "+ GC.BlockManager.BlockInstDict.Count);
            foreach(BlockInstance bi in GC.BlockManager.BlockInstDict.Values)
            {
                foreach(Card c in bi.defenders)
                {
                    if(!c.CardCondition.IsDead)
                    {
                        Debug.LogFormat("RPC_BattleResolveCallBack: {0} go back to origin field location",c.Data.Name);
                        c.User.CardTransform.SetCardBackToOrigin(c);
                    }
                    else
                    {
                        Debug.LogWarningFormat("{0} is dead. Cant go back to origin", c.Data.Name);
                    }
                }

            }


            //Clear block instances. All attacking and defending cards are deleted from dictionary
            Setting.gameController.BlockManager.ClearBlockInst();
        }

        #endregion

       // #region Blocking

        //public void PlayerBlocksTargetCard(int blockingInstId, int photonId_blocker, int attackInstId, int photonId_attacker)
        //{
        //    photonView.RPC("RPC_PlayerBlocksTargetCard_Master", PhotonTargets.All, 
        //        blockingInstId, photonId_blocker, attackInstId, photonId_attacker);
        //}

        //[PunRPC]
        //public void RPC_PlayerBlocksTargetCard_Master(int defendCardInstId, int defenderPhotonId, int attackCardInstId, int attackerPhotonId)
        //{
        //    NetworkPrint print_Defend = GetPlayer(defenderPhotonId);
        //    Card card_Defend = print_Defend.ThisPlayer.CardManager.SearchCard(defendCardInstId);

        //    NetworkPrint print_Invade = GetPlayer(attackerPhotonId);
        //    Card card_Invade = print_Invade.ThisPlayer.CardManager.SearchCard(attackCardInstId);
            
        //    int count = 0;

        //    //Make new BlockInstance wth 'card_Invade' and 'card_Defend'
        //    //If there is BlockInstance with 'card_Invade', it will automatically add 'card_Defend' in existing BlockInstance with increasing 'count'
        //    //Setting.gameController.BlockManager.AddBlockInstance(card_Invade,card_Defend ,ref count);

        //    Debug.Log("PlayerBlocksTargetCard_Master: Blocking cards successfully added");

        //    photonView.RPC("RPC_PlayerBlocksTargetCard_Client", PhotonTargets.All,
        //        defendCardInstId, defenderPhotonId, attackCardInstId, attackerPhotonId, count);

        //    //Sending CardInstance and NetworkPrint through RPC can't be done
        //}


        //[PunRPC]
        //public void RPC_PlayerBlocksTargetCard_Client(int defendCardInstId, int defenderPhotonId, int attackCardInstId, int attackerPhotonId, int count)
        //{
        //    NetworkPrint print_Defend = GetPlayer(defenderPhotonId);
        //    PlayerHolder def = print_Defend.ThisPlayer;
        //    Card card_Defend = def.CardManager.SearchCard(defendCardInstId); ;

        //    NetworkPrint print_Invade = GetPlayer(attackerPhotonId);
        //    PlayerHolder atk = print_Invade.ThisPlayer;
        //    Card card_Invade = atk.CardManager.SearchCard(attackCardInstId);

        //    //MoveCardInstance.SetCardsForBlock(card_Defend, card_Invade, count);
        //}
       // #endregion
               
        #region Multiple Cards Operations        
        #region FlatFooted Cards
        public void PlayerResetFlatFootedCard(int photonId)
        {
            photonView.RPC("RPC_PlayerCanUseCard_Master", PhotonTargets.MasterClient, photonId);
        }

        [PunRPC]
        public void RPC_PlayerCanUseCard_Master(int photonId)
        {
            NetworkPrint nw_Player = GetPlayer(photonId);
            
            if (GC.CurrentPlayer == nw_Player.ThisPlayer)
            {
                photonView.RPC("RPC_PlayerCanUseCard", PhotonTargets.All, photonId);
            }
            else
            {
                Debug.Log("RPC_PlayerCanUseCard_Error: This turn player is different with current player");
                return;
            }
            
        }

        [PunRPC]
        public void RPC_PlayerCanUseCard(int photonId)
        {
            NetworkPrint nw_Player = GetPlayer(photonId);
            foreach (int cardId in nw_Player.ThisPlayer.CardManager.fieldCards)
            {
                Card c = nw_Player.ThisPlayer.CardManager.SearchCard(cardId);
                if (c.CardCondition.CanUse==false)
                {
                    c.CardCondition.CanUse = true;
                    Debug.LogFormat("PlayerCanUseCard: {0} can use {1} to Attack ",
                        nw_Player.ThisPlayer.PlayerProfile.UniqueId,c.Data.Name);
                }
            }

}
        #endregion


        #region Management
        public void SendPhase(string holder, string phase)
        {
            //photonView.RPC("RPC_MessagePhase", PhotonTargets.All, phase, holder);
        }
        [PunRPC]
        public void RPC_MessagePhase(string phase, string holder)
        {
            //Debug.Log("current phase: " + phase + "and holder: "+ holder);
        }




        #endregion

        #endregion

    }

}
 
