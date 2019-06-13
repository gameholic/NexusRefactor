using UnityEngine;
using UnityEditor;
using GH.GameElements;
namespace GH.GameCard{
    [CreateAssetMenu(menuName ="Card_Grave")]
    public class CardGraveyard : ScriptableObject
    {
        [SerializeField]
        private TransformVariable[] _GraveYard;


        public TransformVariable GetGraveYard(int playerCode)
        {
             return _GraveYard[playerCode];
        }
        public void PutCardToGrave(CardInstance c)
        {
            PlayerHolder cardOwner = c.owner;
            GameObject graveyardObj = null;
            cardOwner.graveyard.Add(c);
            

            //Should check owner to move card to graveyard
            //if (c.owner.player == "Player1")
            //    graveyardObj = GetGraveYard(0).value.gameObject;
            //else if (c.owner.player == "Player2")
            //    graveyardObj = GetGraveYard(1).value.gameObject;


            if (graveyardObj == null)
            {
                Debug.LogError("Failed to check obj");
            }
            else
            {
                Setting.SetParentForCard(c.transform, graveyardObj.transform);
            }

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
            Area a = c.GetComponentInParent<Area>();
            a.IsPlaced = false;
            c.dead = true;
            c.gameObject.SetActive(false);
            c.gameObject.GetComponentInChildren<CardInstance>().enabled = false;
            c.currentLogic = null;
        }

    }
}