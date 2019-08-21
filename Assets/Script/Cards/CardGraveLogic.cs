//using GH.GameCard;
//using GH.GameCard.CardLogics;
//using GH.GameElements;
//using GH.Player;
//using UnityEngine;
//namespace GH
//{

//    public class CardGraveLogic : Photon.MonoBehaviour
//    {
//        private void Awake()
//        {
//            Setting.gameController.CardGraveLogic = this;
//            DontDestroyOnLoad(this.gameObject);
//        }
//        public void PutCardToGrave(Card c)
//        {
//            photonView.RPC("RPC_PutCardToGrave", PhotonTargets.All, c.Data.UniqueId, c.User.InGameData.PhotonId);
//        }
//        [PunRPC]
//        private void RPC_PutCardToGrave(int cardId, int photonId)
//        {
//            PlayerHolder cardOwner = Multiplay.MultiplayManager.singleton.GetPlayer(photonId).ThisPlayer;
//            Card c = cardOwner.CardManager.SearchCard(cardId);

//            cardOwner.CardManager.deadCards.Add(cardId);
//            //dead card should be added here.
//            if (c == null)
//            {
//                Debug.LogError("COULDN'T FIND CARDINSTANCE");
//            }
//            int j = 0;
//            for (int i = 0; i < 2; i++)
//            {
//                if(c.User == Setting.gameController.GetPlayer(i))
//                {
//                    cardOwner = Setting.gameController.GetPlayer(i);
//                    j = i;
//                    Debug.LogFormat("This dead card's owner is " + cardOwner);
//                }
//            }
//            if(cardOwner ==null)
//            {
//                Debug.LogError("CARD OWNER IS NULL");
//                return;
//            }
//            //Should check owner to move card to graveyard

//            if (cardOwner.CardManager.fieldCards.Contains(cardId))
//            {
//                //cardOwner.fieldCard.Remove(c);
//                Setting.gameController.GetPlayer(j).CardManager.fieldCards.Remove(cardId);
//                Debug.LogFormat("CardGraveyard, {0}'s {1} is removed from fieldCard", 
//                    cardOwner.PlayerProfile.UniqueId, c.Data.Name);
//            }
//            if (cardOwner.CardManager.handCards.Contains(cardId))
//            {
//                cardOwner.CardManager.handCards.Remove(cardId);
//                Debug.LogFormat("CardGraveyard, {0}'s {1} is removed from handCards", 
//                    cardOwner.PlayerProfile.UniqueId, c.Data.Name);
//            }
//            if (cardOwner.CardManager.attackingCards.Contains(cardId))
//            {
//                cardOwner.CardManager.attackingCards.Remove(cardId);
//                Debug.LogFormat("CardGraveyard, {0}'s {1} is removed from attackingCards",
//                    cardOwner.PlayerProfile.UniqueId, c.Data.Name);
//            }
//            c.PhysicalCondition.GetOriginFieldLocation().GetComponentInParent<Area>().IsPlaced = false;
//            //c.SetOriginFieldLocation(null);
//            c.CardCondition.IsDead = true;            
//            cardId.gameObject.SetActive(false);
//            cardId.gameObject.GetComponentInChildren<CardInstance>().enabled = false;
//            cardId.currentLogic = null;

//            //Debug.LogWarningFormat("PutCardToGrave: {0} is deleted from all lists",c.Data.Name);
//        }

//        public void MoveCardToGrave(Card c, Transform graveTransform)
//        {
//            MoveCardInstance.SetParentForCard(c.PhysicalCondition.transform, graveTransform);
//            //Debug.LogFormat("MoveCardToGrave: {0} is move to grave({1}) successfully", c.Data.Name, graveTransform.name);
//        }
//    }
//}