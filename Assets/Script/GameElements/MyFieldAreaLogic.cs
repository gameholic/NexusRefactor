using UnityEngine;
using System.Collections;
using UnityEditor;


/// <summary>
/// It runs when card is trying to be placed on area.
/// 
/// It checks what sort of the card is and can this card be placed in area. If not, return error message and stop running.
/// </summary>
namespace GH
{
    [CreateAssetMenu(menuName ="Areas/MyCardsHolding")]
    public class MyFieldAreaLogic : AreaLogic
    {
        public CardVariables cardVar;
        public CardType creature;
        public CardType spell;
        public GameElements.Instance_logic cardOnFieldLogic;

        public override void Execute(GameElements.Area a)
        {
            PlayerHolder p = Setting.gameController.currentPlayer;
            bool isPlaced = false;
            if (a.transform.childCount != 0)
                isPlaced = true;
            bool checkOwner = Setting.gameController.checkObjOwner.CheckPlayer(a.gameObject);
            if (!checkOwner)
            {
                Debug.Log("You cant control other player's obj");
                return;
            }

            ////////////////
            ///this need to be changed.
            ///isPlaced isn't initialised which means, eventhough creature card that is placed on field get removed, still it thinks area isn't empty.
            ////////////////
            if (cardVar.value == null || isPlaced==true)
                return;

            Card currentCard = cardVar.value.viz.card;
            if (currentCard.cardType == creature)
            {
                bool canUse = p.PayMana(currentCard); // Check current mana cost.


                if (canUse)
                {
                    //Send card transform, area transform, current card viz and variables to Setting.
                    Setting.DropCreatureCard(cardVar.value.transform
                        ,a.transform
                        ,currentCard
                        ,cardVar.value);

                    cardVar.value.currentLogic = cardOnFieldLogic;
                    //currentCard.value.gameObject.layer = 9;

                    isPlaced = true; // Set isPlace to true so another card can't be placed in this area. 
                    BoxCollider box =a.GetComponent<BoxCollider>();
                    box.enabled = false;
                    p.manaResourceManager.UpdateCurrentMana(-(currentCard.cardCost));


                    cardVar.value.SetOriginFieldLocation(a.transform);
                }
                cardVar.value.gameObject.SetActive(true);
            }

            else if(currentCard.cardType == spell)
            {
                Debug.Log("This is spell Card");
            }
        }
    }

}