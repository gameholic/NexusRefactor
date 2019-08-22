using UnityEngine;
using UnityEditor;
using GH.GameElements;
using GH.Multiplay;
using GH.Player;
using GH.GameCard.CardInfo;

namespace GH.GameCard.CardLogics
{

    public class GraveLogic : Photon.MonoBehaviour
    {

        private static MultiplayManager multiplayManager = MultiplayManager.singleton;
        private void Awake()
        {
            DontDestroyOnLoad(this.gameObject);
        }
        public void SetCardDeadLogic(Card c)
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
            cardOwner.CardManager.deadCards.Add(card.Data.UniqueId);
            //dead card should be added here.
           
            for (int i = 0; i < 2; i++)
            {
                if(card.User == Setting.gameController.GetPlayer(i))
                {
                    cardOwner = Setting.gameController.GetPlayer(i);
                    Debug.LogFormat("This dead card's owner is " + cardOwner);
                }
            }
            if(cardOwner ==null)
            {
                Debug.LogError("CARD OWNER IS NULL");
                return;
            }
            //Should check owner to move card to graveyard
            photonView.RPC("RPC_YouAreAlreadyDead", PhotonTargets.All, card.Data.UniqueId, cardOwner.InGameData.PhotonId);

        }

        [PunRPC]
        public void RPC_YouAreAlreadyDead(int playerPhotonId,int cardInstId)
        {

            NetworkPrint p = multiplayManager.GetPlayer(playerPhotonId);

            PlayerHolder you = p.ThisPlayer;
            Card card = p.ThisPlayer.CardManager.SearchCard(cardInstId);
            string cardOwner = you.PlayerProfile.UniqueId;


            if (you.CardManager.CheckCardContainer(Player.CardContainer.Field, card))
            {
                //cardOwner.fieldCard.Remove(c);
                you.CardManager.fieldCards.Remove(card.Data.UniqueId);
                Debug.LogFormat("CardGraveyard, {0}'s {1} is removed from fieldCard", cardOwner, card.Data.Name);
            }
            if (you.CardManager.CheckCardContainer(Player.CardContainer.Hand, card))
            {
                you.CardManager.handCards.Remove(card.Data.UniqueId);
                Debug.LogFormat("CardGraveyard, {0}'s {1} is removed from handCards", cardOwner, card.Data.Name);
            }
            if (you.CardManager.CheckCardContainer(Player.CardContainer.Attack, card))
            {
                you.CardManager.attackingCards.Remove(card.Data.UniqueId);
                Debug.LogFormat("CardGraveyard, {0}'s {1} is removed from attackingCards", cardOwner, card.Data.Name);
            }
            card.PhysicalCondition.GetOriginFieldLocation().GetComponentInParent<Area>().IsPlaced = false;
            card.CardCondition.IsDead = true;
            card.PhysicalCondition.gameObject.SetActive(false);
            card.PhysicalCondition.gameObject.GetComponentInChildren<PhysicalAttribute>().enabled = false;

            //Debug.LogWarningFormat("PutCardToGrave: {0} is deleted from all lists",c.Data.Name);
        }

        public static void MoveCardToGrave(Card c, Transform graveTransform)
        {
            MoveCardInstance.SetParentForCard(c.PhysicalCondition.transform, graveTransform);
            //Debug.LogFormat("MoveCardToGrave: {0} is move to grave({1}) successfully", c.Data.Name, graveTransform.name);
        }
    }
}