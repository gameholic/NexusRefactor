using GH.MouseLogics;
using GH.GameCard;
using GH.GameCard.CardInfo;
using GH.GameCard.CardLogics;
using GH.GameElements;
using GH.Player;
using UnityEngine;
using System.Collections.Generic;
namespace GH.Multiplay
{

    public class CardPlayManager : Photon.MonoBehaviour
    {

        public static CardPlayManager singleton;
        private static MultiplayManager multiplayManager = MultiplayManager.singleton;
        private void Awake()
        {
            singleton = this;
            DontDestroyOnLoad(this.gameObject);
        }

        #region GraveLogics
        public void SetCardToGrave(Card c)
        {
            photonView.RPC("RPC_SetCardDead", PhotonTargets.All, c.Data.UniqueId, c.User.InGameData.PhotonId);
        }
        [PunRPC]
        private void RPC_SetCardDead(int cardInstId, int playerPhotonId)
        {
            NetworkPrint p = multiplayManager.GetPlayer(playerPhotonId);
            PlayerHolder cardOwner = p.ThisPlayer;
            Card card = p.ThisPlayer.CardManager.SearchCard(cardInstId);
            if (card == null)
            {
                Debug.LogError("COULDN'T FIND CARDINSTANCE");
                return;
            }
            cardOwner.CardManager.deadCards.Add(card.Data.UniqueId);          //dead card should be added here.
            for (int i = 0; i < 2; i++)
            {
                if (card.User == Setting.gameController.GetPlayer(i))
                {
                    cardOwner = Setting.gameController.GetPlayer(i);
                    Debug.LogFormat("This dead card's owner is " + cardOwner);
                }
            }
            if (cardOwner == null)
            {
                Debug.LogError("CARD OWNER IS NULL");
                return;
            }
            //Should check owner to move card to graveyard
            photonView.RPC("RPC_CleanCardData", PhotonTargets.All, card.Data.UniqueId, cardOwner.InGameData.PhotonId);
        }

        /// <summary>
        /// Clean Card Instance data from all lists, and add to dead card.
        /// </summary>
        /// <param name="playerPhotonId"></param>
        /// <param name="cardInstId"></param>
        [PunRPC]
        public void RPC_CleanCardData(int playerPhotonId, int cardInstId)
        {
            NetworkPrint p = multiplayManager.GetPlayer(playerPhotonId);
            PlayerHolder thisPlayer = p.ThisPlayer;
            Card card = p.ThisPlayer.CardManager.SearchCard(cardInstId);
            string cardOwner = thisPlayer.PlayerProfile.UniqueId;
            //After Refactoring, Try to reduce below codes
            if (thisPlayer.CardManager.CheckCardContainer(Player.CardContainer.Field, card))
            {
                thisPlayer.CardManager.fieldCards.Remove(card.Data.UniqueId);
                Debug.LogFormat("CardGraveyard, {0}'s {1} is removed from fieldCard", cardOwner, card.Data.Name);
            }
            if (thisPlayer.CardManager.CheckCardContainer(Player.CardContainer.Hand, card))
            {
                thisPlayer.CardManager.handCards.Remove(card.Data.UniqueId);
                Debug.LogFormat("CardGraveyard, {0}'s {1} is removed from handCards", cardOwner, card.Data.Name);
            }
            if (thisPlayer.CardManager.CheckCardContainer(Player.CardContainer.Attack, card))
            {
                thisPlayer.CardManager.attackingCards.Remove(card.Data.UniqueId);
                Debug.LogFormat("CardGraveyard, {0}'s {1} is removed from attackingCards", cardOwner, card.Data.Name);
            }
            card.PhysicalCondition.GetOriginFieldLocation().GetComponentInParent<Area>().SetIsPlaced(false);
            card.CardCondition.IsDead = true;
            card.PhysicalCondition.gameObject.SetActive(false);
            card.PhysicalCondition.gameObject.GetComponentInChildren<PhysicalAttribute>().enabled = false;
            //Debug.LogWarningFormat("PutCardToGrave: {0} is deleted from all lists",c.Data.Name);
        }

        /// <summary>
        /// Move Card Instance To Grave
        /// Due to Synchronization, It can't be placed on 'MoveCardInstance'
        /// </summary>
        /// <param name="c"></param>
        /// <param name="graveTransform"></param>
        public static void MoveCardToGrave(Card c, Transform graveTransform)
        {
            MoveCardInstance.SetParentForCard(c.PhysicalCondition.transform, graveTransform);
            //Debug.LogFormat("MoveCardToGrave: {0} is move to grave({1}) successfully", c.Data.Name, graveTransform.name);
        }
        #endregion

        /// <summary>
        /// Card From Paramter of Function in 'CreatureMoveLogics' is current user's card.
        /// User Check is done.
        /// </summary>
        #region CreatureMoveLogics

        private Dictionary<int, Area> tmp = new Dictionary<int, Area>();
        public void CardPlayDrop(CreatureCard c,Area a)
        {
            Debug.Log("CardPlayDrop");
            tmp.Add(1,a);
            photonView.RPC("RPC_DropCard", PhotonTargets.All,
                    c.Data.UniqueId, 1);
        }
        [PunRPC]
        public void RPC_DropCard(int cardId, int areaID)
        {
            CreatureCard c = GetCard(cardId);
            Area a = null;
            tmp.TryGetValue(areaID, out a);
            c.PhysicalCondition.SetOriginFieldLocation(a.transform);
            MoveCardInstance.DropCreatureCard(c);
            c.User.InGameData.ManaManager.UpdateCurrentMana(-(c.Data.ManaCost));
            Debug.Log("CardDroped");

        }


        public void CardPlayAttack(CreatureCard c)
        {
            photonView.RPC("RPC_Attack", PhotonTargets.All,
                   c.Data.UniqueId);

        }
        [PunRPC]
        private void RPC_Attack(int cardId)
        {
            CreatureCard c = GetCard(cardId);

            PlayerHolder currentPlayer = c.User;
            if (c.User.CardManager.CheckCardContainer(CardContainer.Field, c))
            {
                currentPlayer.CardManager.attackingCards.Add(c.Data.UniqueId);
                currentPlayer.CardTransform.SetCardOnBattleLine(c);
            }
            else if (c.User.CardManager.CheckCardContainer(CardContainer.Attack, c))    //Return to Original Place = Cancel Attack
            {
                currentPlayer.CardManager.attackingCards.Remove(c.Data.UniqueId);
                MoveCardInstance.SetParentForCard(c.PhysicalCondition.transform, c.PhysicalCondition.GetOriginFieldLocation());
            }
        }
        public void CardPlayBlock(CreatureCard blockingCard, CreatureCard attackingCard)
        {
            int defendInstId = blockingCard.Data.UniqueId;
            int attackInstId = attackingCard.Data.UniqueId;

            int defendUser = blockingCard.User.PlayerProfile.PhotonId;
            int attackUser = attackingCard.User.PlayerProfile.PhotonId;

            photonView.RPC("RPC_BlockMaster", PhotonTargets.All,
                defendInstId, defendUser, attackInstId, attackUser);


        }

        [PunRPC]
        private void RPC_BlockMaster(int defendId, int attackId, int defendUser, int attackUser)
        {
            CreatureCard defendCard = GetCard(defendId, defendUser);
            CreatureCard attackCard = GetCard(attackId, attackUser);

            int count = 0;
            Setting.gameController.BlockManager.AddBlockInstance(attackCard, defendCard, ref count);

            Debug.Log("PlayerBlocksTargetCard_Master: Blocking cards successfully added");

            photonView.RPC("RPC_BlockClient", PhotonTargets.All,
                defendId, defendUser, attackId, attackUser, count);

        }

        [PunRPC]
        private void RPC_BlockClient(int defendId, int attackId, int defendUser, int attackUser, int count)
        {
            CreatureCard defendCard = GetCard(defendId, defendUser);
            CreatureCard attackCard = GetCard(attackId, attackUser);

            MoveCardInstance.SetCardsForBlock(defendCard, attackCard, count);
        }

        private CreatureCard GetCard(int cardId)
        {
            CreatureCard c = (CreatureCard)Setting.gameController.CurrentPlayer.CardManager.SearchCard(cardId);
            return c;
        }

        private CreatureCard GetCard(int cardId, int playerId)
        {
            CreatureCard c = null;


            for(int i=0;i< Setting.gameController.Players.Length;i++)
            {
                PlayerHolder p = Setting.gameController.GetPlayer(i);
                if(p.PlayerProfile.PhotonId  == playerId)
                {
                    c =(CreatureCard) p.CardManager.SearchCard(cardId);
                }
            }                
            return c;
        }
        #endregion
    }
}