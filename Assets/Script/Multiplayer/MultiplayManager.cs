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
        public MainDataHolder MainData
        {
            get { return _MainDataHolder; }
        }
        public NetworkPrint GetPlayer(int photonID)
        {
            for (int i = 0; i < Players.Count; i++)
            {
                if (Players[i].photonId == photonID)
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

            if (NetworkManager.singleton.IsMaster)
            {
                Debug.Log("ISMASTER");
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
                        }
                        else
                        {
                            p.ThisPlayer = GC.ClientPlayer;
                            p.ThisPlayer.PhotonId = p.photonId;
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
                Debug.Log("IsNotMaster");
                
                foreach (NetworkPrint p in Players)
                {
                    if (p.IsLocal)
                    {
                        p.ThisPlayer = GC.LocalPlayer;
                        p.ThisPlayer.PhotonId = p.photonId;
                    }
                    else
                    {
                        p.ThisPlayer = GC.ClientPlayer;
                        p.ThisPlayer.PhotonId = p.photonId;
                    }
                }


            }
        }
        [PunRPC]
        public void RPC_PlayerCreatesCard(int photonId, int cardId, string cardName)
        {
            Card c = GC.ResourceManager.GetCardInstFromDeck(cardName);
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


        public void AddPlayer(NetworkPrint nw_print)
        {
            if (nw_print.IsLocal)
            {
                localPlayerNWPrint = nw_print;
            }
            AddPlayers = nw_print;
            nw_print.transform.parent = MultiplayerReferences;
        }
        public NetworkPrint GetPlayers(int photonId)
        {
            for (int i = 0; i < Players.Count; i++)
            {
                if (Players[i].photonId == photonId)
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
            photonView.RPC("RPC_PlayerEndsTurn", PhotonTargets.MasterClient, photonId);
        }

        [PunRPC]
        public void RPC_PlayerEndsTurn(int photonId)
        {
            if (photonId == GC.CurrentPlayer.PhotonId)
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

        public void PlayerTryToUseCard(int cardInst, int photonId, CardOperation operation, int cardArea = 0)
        {
            photonView.RPC("RPC_PlayerTryToUseCard", PhotonTargets.MasterClient, cardInst, photonId, operation, cardArea);
        }
        [PunRPC]
        public void RPC_PlayerTryToUseCard(int cardInst, int photonId, CardOperation operation, int cardArea = 0)
        {
            if (!NetworkManager.singleton.IsMaster)
                return;

            bool hasCard = PlayerHasCard(cardInst, photonId);
            if (hasCard)
            {
                photonView.RPC("RPC_PlayerUsesCard", PhotonTargets.All, cardInst, photonId, operation, cardArea);
            }
        }

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
            NetworkPrint p = GetPlayer(playerHolder.PhotonId);
            Card c=null;
            if (p.CardDeck.Count != 0)
            {
                c = p.CardDeck[0];
                p.CardDeck.RemoveAt(0);
                PlayerTryToUseCard(c.InstId, p.photonId, CardOperation.pickCardFromDeck);
            }
            else
                Debug.Log("There is no card in deck");
        }



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
                    card.Instance.SetCanAttack(false);
                    card.Instance.gameObject.SetActive(true);
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
                    if (currentPlayer.attackingCards.Contains(card.Instance))
                    {
                        currentPlayer.attackingCards.Remove(card.Instance);
                        currentPlayer._CardHolder.SetCardDown(card.Instance);
                           
                    }
                    else
                    {
                        currentPlayer.attackingCards.Add(card.Instance);
                        currentPlayer._CardHolder.SetCardOnBattleLine(card.Instance);
                    }

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
            if (!NetworkManager.singleton.IsMaster)
                return;
            BattleResolveForPlayers();
        }
        private void BattleResolveForPlayers()
        {
            GameController gc = Setting.gameController;
            PlayerHolder p = Setting.gameController.CurrentPlayer;
            PlayerHolder e = Setting.gameController.GetOpponentOf(p);

            Element elementAttack = MainData.AttackElement;
            Element elementHealth = MainData.HealthElement;


            if (p == e)
                Debug.LogError("p == e ");
            if (p.attackingCards.Count == 0)
            {
                photonView.RPC("RPC_BattleResolvesCallBack", PhotonTargets.All, p.PhotonId);
            }

            Dictionary<CardInstance, BlockInstance> defDic = gc.BlockManager.BlockInstDict;


            for (int i = 0; i < p.attackingCards.Count; i++)
            {
                CardInstance inst = p.attackingCards[i];
                Card c = inst.viz.card;
                CardProperties attack = c.GetProperties(elementAttack);
                if (attack == null)
                {
                    Setting.RegisterLog("Attack of the " + c.name + "is null. This card can't attack", Color.red);
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
                            //Debug.Log("defendcard dead ");
                            bi.defenders[i].CardInstanceToGrave();
                        }
                    }
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
                    inst.IsAvailable(true);       
                    photonView.RPC("RPC_SyncPlayerHealth", PhotonTargets.All ,e.PhotonId, e.Health);
                }
                ////////
                Setting.RegisterLog("Attack damage is " + attackValue, Color.red);
                e.DoDamage(attackValue);
            }

            Dictionary<CardInstance, BlockInstance> blockInstances = gc.BlockManager.BlockInstDict;


            foreach (CardInstance c in blockInstances.Keys)
            {
                if (c.dead)
                {
                    Debug.Log("this card is dead");
                    break;
                }
                c.IsAvailable(false);
                Setting.SetParentForCard(c.transform, c.GetOriginFieldLocation());
            }

            foreach (CardInstance c in e.fieldCard)
            {
                Setting.SetParentForCard(c.transform, c.GetOriginFieldLocation());
            }

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
                    if (p == localPlayerNWPrint)
                    {
                        isAttacker = true;
                        Setting.gameController.EndPhase();

                    }
                }
                foreach (CardInstance c in p.ThisPlayer.attackingCards)
                {
                    p.ThisPlayer._CardHolder.SetCardDown(c);
                    c.IsAvailable(true);
                }

                p.ThisPlayer.attackingCards.Clear();
            }
            

        }

        #endregion
        #region Blocking

        public void PlayerBlocksTargetCard(int blockingInstId, int photonId_blocker, int attackInstId, int photonId_attacker)
        {
            photonView.RPC("RPC_PlayerBlocksTargetCard", PhotonTargets.MasterClient, 
                blockingInstId, photonId_blocker, attackInstId, photonId_attacker);
        }

        [PunRPC]
        public void RPC_PlayerBlocksTargetCard_Master(int defendCard, int photonId_Defend, int invadeInstId, int photonId_invade)
        {
            NetworkPrint print_Defend = GetPlayer(photonId_Defend);
            //PlayerHolder blocker = blockerPrint.ThisPlayer;
            CardInstance card_Defend = print_Defend.GetCard(defendCard).Instance;

            NetworkPrint print_Invade = GetPlayer(photonId_invade);
            //PlayerHolder atker = atkPrint.ThisPlayer;
            CardInstance card_Invade = print_Invade.GetCard(invadeInstId).Instance;


            int count = 0;
            Setting.gameController.BlockManager.AddBlockInstance(card_Invade,card_Defend ,ref count);

            photonView.RPC("RPC_PlayerBlocksTargetCard_Client", PhotonTargets.All,
                card_Defend, print_Defend, card_Invade, print_Invade, count);
        }


        [PunRPC]
        public void RPC_PlayerBlocksTargetCard_Client(CardInstance card_Defend, NetworkPrint print_Defend, 
            CardInstance card_Invade, NetworkPrint print_Invade, int count)
        {
            Setting.SetCardForblock(card_Defend.transform, card_Invade.transform, count);
            
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
            if(GC.GetTurns(GC.turnIndex).ThisTurnPlayer == nw_Player.ThisPlayer)
            {
                photonView.RPC("RPC_PlayerCanUseCard", PhotonTargets.All, photonId);
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
                    c.IsAvailable(false);
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
 
