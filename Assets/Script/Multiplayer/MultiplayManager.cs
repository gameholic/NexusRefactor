﻿using GH.GameCard;
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
            PlayerProfile profile = Resources.Load("Player Profile") as PlayerProfile;
            object[] data = new object[1];
            data[0] = profile.GetCardIds();

            PhotonNetwork.Instantiate("NetworkPrint", Vector3.zero, Quaternion.identity, 0, data);
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
                Debug.Log("This client is room manager client");
                List<int> playerId = new List<int>();
                List<int> cardInstId = new List<int>();
                List<string> cardName = new List<string>();
                foreach (NetworkPrint p in Players)
                {
                    foreach (string id in p.GetStartingCardids())
                    {
                        Card c = rm.GetCardInstFromDeck(id);
                        playerId.Add(p.photonId);
                        
                        cardInstId.Add(c.InstId);
                        cardName.Add(id);

                        if (p.IsLocal)
                        {
                            p.ThisPlayer = GC.LocalPlayer;
                            p.ThisPlayer.PhotonId = p.photonId;
                            GC.LocalPlayer.player = "Room Manager";
                        }
                        else
                        {
                            p.ThisPlayer = GC.ClientPlayer;
                            p.ThisPlayer.PhotonId = p.photonId;
                            GC.ClientPlayer.player = "Room Member_Client";
                        }
                    }
                }
                for (int i = 0; i < playerId.Count; i++)
                {
                    photonView.RPC("RPC_PlayerCreatesCard", PhotonTargets.All, playerId[i], cardInstId[i], cardName[i]);
                }
                photonView.RPC("RPC_InitGame", PhotonTargets.All, 1);
            }

            else
            {
                Debug.Log("This client is room guest client");                
                foreach (NetworkPrint p in Players)
                {
                    if (p.IsLocal)
                    {
                        p.ThisPlayer = GC.LocalPlayer;
                        p.ThisPlayer.PhotonId = p.photonId;
                        GC.LocalPlayer.player = "Room Member";
                    }
                    else
                    {
                        p.ThisPlayer = GC.ClientPlayer;
                        p.ThisPlayer.PhotonId = p.photonId;
                        GC.ClientPlayer.player = "Room Manager_Client";
                    }
                }
            }
        }
        [PunRPC]
        public void RPC_PlayerCreatesCard(int photonId, int cardId, string cardName)
        {
            Card c = GC.ResourceManager.GetCardInstFromDeck(cardName);
            if(c==null)
            {
                Debug.LogError("CardNameNotFound: " + cardName);
            }
            c.InstId = cardId;
            NetworkPrint p = GetPlayer(photonId);
            p.AddCard(c);
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
            Debug.LogFormat("RPC_PlayerEndsTurn: {0} ends turn.", GC.CurrentPlayer.player);

            if (photonId == GC.CurrentPlayer.PhotonId)
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

        #region Card Checks

        /// <summary>
        /// Use RPC to call proper functions player wants
        /// Parameters are all clear and should be checked before this statement
        /// 
        /// </summary>
        /// <param name="cardInst"> Card instance id</param>
        /// <param name="photonId"> Player Photon id</param>
        /// <param name="operation"> Action that player wants to perform </param>
        /// <param name="cardArea"> Card area which is 0 when it's not passed to use number of 'field' obj</param>
        public void PlayerTryToUseCard(int cardInst, int photonId, CardOperation operation, int cardArea = 0)
        {
            photonView.RPC("RPC_PlayerTryToUseCard", PhotonTargets.MasterClient, cardInst, photonId, operation, cardArea);
        }

        [PunRPC]
        public void RPC_PlayerTryToUseCard(int cardInst, int photonId, CardOperation operation, int cardArea = 0)
        {
            //if (!NetworkManager.IsMaster)
            //    return;

            // 
            bool hasCard = PlayerHasCard(cardInst, photonId);
            if (hasCard)
            {
                photonView.RPC("RPC_PlayerUsesCard", PhotonTargets.All, cardInst, photonId, operation, cardArea);
            }
            else
            {
                Debug.LogErrorFormat("PlayerDontOwnCard: {0} don't have selected card", GetPlayer(photonId));
            }
        }

        /// <summary>
        /// Check if player has the card
        /// </summary>
        /// <param name="cardInst"> Card instance id to be checked</param>
        /// <param name="photonId"> Player's photon id</param>
        /// <returns></returns>
        private bool PlayerHasCard(int cardInst, int photonId)
        {
            NetworkPrint p = GetPlayer(photonId);
            Card c = p.GetCard(cardInst);
            return (c != null);
        }

        #endregion

        #region Card Operations

        public enum CardOperation
        {
            dropCreatureCard, useSpellCard, pickCardFromDeck, setCardToAttack,cardToGraveyard
        }

        public void PlayerPicksCardFromDeck(PlayerHolder playerHolder)
        {
            Setting.RegisterLog(playerHolder + " Draws card", playerHolder.playerColor);
            NetworkPrint p = GetPlayer(playerHolder.PhotonId);

            Card c =null;
            if (p.CardDeck.Count != 0)
            {
                c = p.CardDeck[0];
                p.CardDeck.RemoveAt(0);
                PlayerTryToUseCard(c.InstId, p.photonId, CardOperation.pickCardFromDeck);
            }
            else
                Debug.Log("There is no card in deck");
        }


        /// <summary>
        /// Play cards base on parameter 'CardOperation'
        /// All actions with card is performed in this function.
        /// </summary>
        /// <param name="instId"> Card instance id </param>
        /// <param name="photonId"> Player photon id </param>
        /// <param name="operation"> Action to perform </param>
        /// <param name="cardArea"> Area number that is used only at dropCreatureCard. If it's null, it is passed as 0 </param>
        /// <param name="info"></param>
        [PunRPC]
        public void RPC_PlayerUsesCard(int instId, int photonId, CardOperation operation, int cardArea, PhotonMessageInfo info)
        {
            NetworkPrint nwPrint = GetPlayer(photonId);
            PlayerHolder currentPlayer = nwPrint.ThisPlayer;
            Card card = nwPrint.GetCard(instId);

            switch (operation)
            {
                case CardOperation.dropCreatureCard:

                    Setting.DropCreatureCard(card.Instance.transform,
                        currentPlayer._CardHolder.GetFieldGrid(cardArea).value,
                        card);
                    card.Instance.currentLogic = MainData.FieldCardLogic;
                    currentPlayer.manaResourceManager.UpdateCurrentMana(-(card.cardCost));
                    //card.Instance.SetCanAttack(false);
                    //card.Instance.gameObject.SetActive(true);
                    break;

                case CardOperation.useSpellCard:
                    //Logic for spell card
                    break;

                case CardOperation.pickCardFromDeck:
                    GameObject go = Instantiate(MainData.CardPrefab) as GameObject;
                    CardViz v = go.GetComponent<CardViz>();
                    v.LoadCard(card);
                    card.Instance = go.GetComponent<CardInstance>();
                    card.Instance.owner = currentPlayer;
                    card.Instance.currentLogic = MainData.HandCardLogic;
                    Setting.SetParentForCard(go.transform, currentPlayer._CardHolder.handGrid.value);
                    //Register log occurs error here. Don't know why
                    //Setting.RegisterLog("Opponent field card position: " + GameController.singleton.GetOpponentOf(nwPrint.ThisPlayer).fieldCard[0].gameObject, Color.green);
                    if (currentPlayer.handCards.Count <= 7)
                        currentPlayer.handCards.Add(card.Instance);
                    else
                        Setting.RegisterLog("Can't add card. Next card is deleted", Color.black);
                    break;

                case CardOperation.setCardToAttack:
                    ///Below Codes must be changed.
                    ///Reason 1: SetCardDown don't work as expected because 'OriginFieldLocation' isn't saved in card instance id
                    ///when we call card instance through instance id.
                    ///Reason 2: Somehow, eventhough this card isn't on attackingCard list it is recognized as so.
                    ///This error occurs only to client.
                    ///Expecting Error : What if there is same card attacking together?
                    ///If selected is already on attack, remove that card from 'attackingCards' list and place back to original field location
                    ///if (currentPlayer.attackingCards.Contains(card.Instance))
                    ///{
                    ///    currentPlayer.attackingCards.Remove(card.Instance);
                    ///    currentPlayer._CardHolder.SetCardDown(card.Instance);                           
                    ///}
                    ///

                    // Deleting same position of line: alt + shif + arrow
                    //If card isn't on attack, move card to 'BattleLine'obj and add at 'attackingCards' list
                    //else
                    //{
                    currentPlayer.attackingCards.Add(card.Instance);
                        currentPlayer._CardHolder.SetCardOnBattleLine(card.Instance);
                    //}
                    break;

                case CardOperation.cardToGraveyard:
                    card.Instance.CardInstanceToGrave();

                    break;

                default:
                    break;
            }
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
            Debug.LogFormat("RPC_BattleResolve_CurrentPlayer: {0}. Network Master Check: {1}",GC.CurrentPlayer.player,NetworkManager.IsMaster);
            if (!NetworkManager.IsMaster)
            {
                Debug.LogWarning("This Player is not network master. Battle Resolve End");
                Setting.gameController.EndPhaseByBattleResolve();
                return;
            }
            else
            {
                Debug.Log("This Player is Network Master. Battle Resolve Starts");
                BattleResolveForPlayers();
            }
        }
        private void BattleResolveForPlayers()
        {
            GameController gc = Setting.gameController;
            PlayerHolder p = Setting.gameController.CurrentPlayer;
            PlayerHolder e = Setting.gameController.GetOpponentOf(p);

            Element elementAttack = MainData.AttackElement;
            Element elementHealth = MainData.HealthElement;


            if (p == e)
                Debug.LogError("BattleResolve_Error: This Player and Opponent Player is same");
            if (p.attackingCards.Count == 0)
            {
                photonView.RPC("RPC_BattleResolvesCallBack", PhotonTargets.All, p.PhotonId);
                Debug.LogWarning("BattleResolveCallBackFailed_ForceExit Won't Work");
            }


            Dictionary<CardInstance, BlockInstance> defDic = gc.BlockManager.BlockInstDict;
            if(defDic == null)
            {
                Debug.LogWarning("BattleResolve_Error: Defending card instance dictionary is null");
            }

            for (int i = 0; i < p.attackingCards.Count; i++)
            {
                CardInstance inst = p.attackingCards[i];
                Card c = inst.viz.card;
                CardProperties attack = c.GetProperties(elementAttack);
                if (attack == null)
                {
                    Debug.LogFormat("BattleResolveError_CardCantAttack: {0} 's attack ability is null.",c.Viz.card.name);
                    continue;
                }

                int attackValue = attack.intValue;
                BlockInstance bi = gc.BlockManager.GetBlockInstanceOfAttacker(inst, defDic);
                if (bi != null)
                {
                    for (int defenders = 0; defenders < bi.defenders.Count; defenders++)
                    {
                        CardProperties def = c.GetProperties(elementHealth);
                        if (def == null)
                        {
                            Debug.LogWarning("You are trying to block with a card with no def element");
                            continue;
                        }

                        attackValue -= def.intValue;
                        if (def.intValue <= attackValue)
                        {
                            bi.defenders[i].CardInstanceToGrave();
                        }
                    }
                }
                else
                {
                    Debug.LogError("BattleResolve_Error: Attacking card instance dictionary is null");
                }

                if (attackValue <= 0)
                {
                    attackValue = 0;
                    PlayerTryToUseCard(inst.viz.card.InstId, p.PhotonId, CardOperation.cardToGraveyard);
                }
                else
                {
                    p.DropCardOnField(inst, false);
                    p._CardHolder.SetCardDown(inst);
                    inst.CanUseByViz(false);
                    photonView.RPC("RPC_SyncPlayerHealth", PhotonTargets.All ,e.PhotonId, e.Health);
                }
                ////////
                Setting.RegisterLog("Attack damage is " + attackValue, Color.red);
                e.DoDamage(attackValue);
            }

            Dictionary<CardInstance, BlockInstance> blockInstances = gc.BlockManager.BlockInstDict;


            //Return all alive cards to its original location
            foreach (CardInstance c in blockInstances.Keys)
            {
                if (c.dead)
                {
                    break;
                }
                Debug.Log("BattleResolveForPlayer_ResetCard");
                c.CanUseByViz(true);
                //This Makes Card Rotation at first turn
                Setting.SetParentForCard(c.transform, c.GetOriginFieldLocation());
            }
            if(e.attackingCards.Count!=0)
            {
                foreach (CardInstance c in e.fieldCard)
                {
                    Debug.Log("BattleResolveForPlayer_ResetEnemyCards");
                    Setting.SetParentForCard(c.transform, c.GetOriginFieldLocation());
                }
            }

            Debug.Log("BattleResolveForPlayer: EndOfCode_CallBackRun");
            photonView.RPC("RPC_BattleResolvesCallBack", PhotonTargets.All, p.PhotonId);
            
        }


        [PunRPC]
        public void RPC_SyncPlayerHealth(int photonId, int health)            
        {
            NetworkPrint nw_player = GetPlayer(photonId);
            nw_player.ThisPlayer.Health = health;


        }
        [PunRPC]
        public void RPC_BattleResolvesCallBack(int photonId)
        {
            Setting.gameController.BlockManager.ClearBlockInst();


            foreach(NetworkPrint p in Players)
            {
                bool isAttacker = false;
                if(p.photonId == photonId)
                {
                    Debug.LogFormat("RPC_BattleResolveCallBack: LocalPlayerNWCheck__Current Player: {0} // The local player: {1}",
                       p.ThisPlayer, localPlayerNWPrint.ThisPlayer);
                    if (p == localPlayerNWPrint)
                    {
                        isAttacker = true;
                        Debug.Log("RPC_BattleResolvesCallBack: End");
                        Setting.gameController.EndPhaseByBattleResolve();
                    }
                    else
                    {
                        Debug.Log("RPC_BattleResolveCallBack: FailedToEnd_ThisPlayerIsNotLocalPlayer");
                        //Setting.gameController.ForceEndPhase();
                    }
                }

                foreach (CardInstance c in p.ThisPlayer.attackingCards)
                {
                    Debug.LogFormat("{0} was attacking. Call back",c.viz.card.name);
                    p.ThisPlayer._CardHolder.SetCardDown(c);
                    c.CanUseByViz(false);
                }

                p.ThisPlayer.attackingCards.Clear();
            }
        }

        #endregion
        #region Blocking

        public void PlayerBlocksTargetCard(int blockingInstId, int photonId_blocker, int attackInstId, int photonId_attacker)
        {
            photonView.RPC("RPC_PlayerBlocksTargetCard_Master", PhotonTargets.MasterClient, 
                blockingInstId, photonId_blocker, attackInstId, photonId_attacker);
        }

        [PunRPC]
        public void RPC_PlayerBlocksTargetCard_Master(int defendCardInstId, int defenderPhotonId, int attackCardInstId, int attackerPhotonId)
        {

            NetworkPrint print_Defend = GetPlayer(defenderPhotonId);
            CardInstance card_Defend = print_Defend.GetCard(defendCardInstId).Instance;

            NetworkPrint print_Invade = GetPlayer(attackerPhotonId);
            CardInstance card_Invade = print_Invade.GetCard(attackCardInstId).Instance;
            
            int count = 0;
            Setting.gameController.BlockManager.AddBlockInstance(card_Invade,card_Defend ,ref count);

            Debug.Log("PlayerBlocksTargetCard_Master: Blocking cards successfully added");
            
            //Sending CardInstance and NetworkPrint through RPC can't be done
            //Those types must be serialized
            //photonView.RPC("RPC_PlayerBlocksTargetCard_Client", PhotonTargets.All,
            //    card_Defend, print_Defend, card_Invade, print_Invade, count);
            photonView.RPC("RPC_PlayerBlocksTargetCard_Client", PhotonTargets.All,
                defendCardInstId, defenderPhotonId, attackCardInstId, attackerPhotonId, count);
        }


        [PunRPC]
        public void RPC_PlayerBlocksTargetCard_Client(int defendCardInstId, int defenderPhotonId, int attackCardInstId, int attackerPhotonId, int count)
        {
            NetworkPrint print_Defend = GetPlayer(defenderPhotonId);
            CardInstance card_Defend = print_Defend.GetCard(defendCardInstId).Instance;

            NetworkPrint print_Invade = GetPlayer(attackerPhotonId);
            CardInstance card_Invade = print_Invade.GetCard(attackCardInstId).Instance;

            Setting.SetCardsForBlock(card_Defend.transform, card_Invade.transform, count);
            Debug.Log("PlayerBlocksTargetCard: Move Cards to Defend Location Sucessful");
            
        }
        #endregion
               
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
            foreach (CardInstance c in nw_Player.ThisPlayer.fieldCard)
            {
                if (!c.GetCanAttack())
                {
                    c.SetCanAttack(true);
                    c.CanUseByViz(true);
                    Debug.LogFormat("PlayerCanUseCard: {0} can use {1} to Attack ",nw_Player.ThisPlayer.userID,c.viz.card.name);
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
 
