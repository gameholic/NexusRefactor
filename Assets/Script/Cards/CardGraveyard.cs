using UnityEngine;
using UnityEditor;
using GH.GameElements;
namespace GH.GameCard{
    [CreateAssetMenu(menuName ="Card_Grave")]
    public class CardGraveyard : ScriptableObject
    {


        public void PutCardToGrave(CardInstance c)
        {
            PlayerHolder cardOwner = null;
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
            cardOwner.deadCards.Add(c);

            //Should check owner to move card to graveyard

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
            c.GetOriginFieldLocation().GetComponentInParent<Area>().IsPlaced = false;
            //c.SetOriginFieldLocation(null);
            c.dead = true;            
            c.gameObject.SetActive(false);
            c.gameObject.GetComponentInChildren<CardInstance>().enabled = false;
            c.currentLogic = null;

            Debug.LogWarningFormat("Check Error for {0} as cardOwner", cardOwner.player);
            CheckError(cardOwner);
            Debug.LogWarningFormat("Check Error for {0} as Setting.gameController.GetPlayer(i)", Setting.gameController.GetPlayer(j).player);
            CheckError(Setting.gameController.GetPlayer(j));
            //Debug.LogWarningFormat("PutCardToGrave: {0} is deleted from all lists",c.viz.card.name);
        }

        public void CheckError(PlayerHolder p)
        {
            foreach (CardInstance c in p.handCards)
            {
                Debug.Log("Hand Card: " + c.viz.card.name);
            }
            foreach (CardInstance c in p.fieldCard)
            {
                Debug.Log("Field Card: "+ c.viz.card.name);
            }
            foreach (CardInstance c in p.attackingCards)
            {
                Debug.Log("Attacking Card: " + c.viz.card.name);
            }
            Debug.LogWarning("//////END//////");
        }

        public void MoveCardToGrave(CardInstance c, Transform graveTransform)
        {
            Setting.SetParentForCard(c.transform, graveTransform);
            //Debug.LogFormat("MoveCardToGrave: {0} is move to grave({1}) successfully", c.viz.card.name, graveTransform.name);
        }
    }
}