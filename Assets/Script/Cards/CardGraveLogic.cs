using UnityEngine;
using UnityEditor;
using GH.GameElements;
using GH.GameCard;

namespace GH
{

    public class CardGraveLogic : Photon.MonoBehaviour
    {
        private void Awake()
        {
            Setting.gameController.CardGraveLogic = this;
            DontDestroyOnLoad(this.gameObject);
        }
        public void PutCardToGrave(CardInstance c)
        {
            photonView.RPC("RPC_PutCardToGrave", PhotonTargets.All, c.viz.card.InstId, c.owner.PhotonId);
        }
        [PunRPC]
        private void RPC_PutCardToGrave(int instId, int photonId)
        {
            PlayerHolder cardOwner = Multiplay.MultiplayManager.singleton.GetPlayer(photonId).ThisPlayer;
            Card card = Multiplay.MultiplayManager.singleton.GetPlayer(photonId).GetCard(instId);
            CardInstance c = card.Instance;
            cardOwner.deadCards.Add(c);
            //dead card should be added here.
            if (c == null)
            {
                Debug.LogError("COULDN'T FIND CARDINSTANCE");
            }
            int j = 0;
            for (int i = 0; i < 2; i++)
            {
                if(c.owner == Setting.gameController.GetPlayer(i))
                {
                    cardOwner = Setting.gameController.GetPlayer(i);
                    j = i;
                    Debug.LogFormat("This dead card's owner is " + cardOwner);
                }
            }
            if(cardOwner ==null)
            {
                Debug.LogError("CARD OWNER IS NULL");
                return;
            }
            //Should check owner to move card to graveyard

            if (cardOwner.fieldCard.Contains(c))
            {
                //cardOwner.fieldCard.Remove(c);
                Setting.gameController.GetPlayer(j).fieldCard.Remove(c);
                Debug.LogFormat("CardGraveyard, {0}'s {1} is removed from fieldCard", cardOwner.player, c.viz.card.name);
            }
            if (cardOwner.handCards.Contains(c))
            {
                cardOwner.handCards.Remove(c);
                Debug.LogFormat("CardGraveyard, {0}'s {1} is removed from handCards", cardOwner.player, c.viz.card.name);
            }
            if (cardOwner.attackingCards.Contains(c))
            {
                cardOwner.attackingCards.Remove(c);
                Debug.LogFormat("CardGraveyard, {0}'s {1} is removed from attackingCards", cardOwner.player, c.viz.card.name);
            }
            c.GetOriginFieldLocation().GetComponentInParent<Area>().IsPlaced = false;
            //c.SetOriginFieldLocation(null);
            c.dead = true;            
            c.gameObject.SetActive(false);
            c.gameObject.GetComponentInChildren<CardInstance>().enabled = false;
            c.currentLogic = null;

            //Debug.LogWarningFormat("PutCardToGrave: {0} is deleted from all lists",c.viz.card.name);
        }

        public void MoveCardToGrave(CardInstance c, Transform graveTransform)
        {
            Setting.SetParentForCard(c.transform, graveTransform);
            //Debug.LogFormat("MoveCardToGrave: {0} is move to grave({1}) successfully", c.viz.card.name, graveTransform.name);
        }
    }
}