using UnityEngine;
using UnityEditor;
using GH.GameElements;
namespace GH.GameCard{
    [CreateAssetMenu(menuName ="Card_Grave")]
    public class CardGraveyard : ScriptableObject
    {


        public void PutCardToGrave(CardInstance c)
        {
            PlayerHolder cardOwner = c.owner;
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
            c.dead = true;            
            c.gameObject.SetActive(false);
            c.gameObject.GetComponentInChildren<CardInstance>().enabled = false;
            c.currentLogic = null;
        }

        public void MoveCardToGrave(CardInstance c, Transform graveTransform)
        {
            Setting.SetParentForCard(c.transform, graveTransform);
        }
    }
}