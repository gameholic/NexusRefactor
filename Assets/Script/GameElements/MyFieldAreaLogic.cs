using UnityEngine;
using System.Collections;
using UnityEditor;
using GH.GameCard;


/// <summary>
/// It runs when card is trying to be placed on area.
/// 
/// It checks what sort of the card is and can this card be placed in area. If not, return error message and stop running.
/// </summary>
namespace GH.GameElements
{
    [CreateAssetMenu(menuName ="Areas/MyCardsHolding")]
    public class MyFieldAreaLogic : AreaLogic
    {
        [SerializeField]
        private CardVariables _CardVar;
        [SerializeField]
        private CardType _CreatureType;
        [SerializeField]
        private CardType _SpellType;
        [SerializeField]
        private Instance_logic _CardOnFieldLogic;

        public override void Execute(GameElements.Area a)
        {
            PlayerHolder p = Setting.gameController.CurrentPlayer;
            //bool isPlaced = false;
            //if (a.transform.childCount != 0)
            //    isPlaced = true;
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
            if (_CardVar.value == null /*|| isPlaced==true*/)
                return;

            Card currentCard = _CardVar.value.viz.card;
            CardInstance c = _CardVar.value;
            if (currentCard.cardType == _CreatureType)
            {
                bool canUse = p.PayMana(currentCard); // Check current mana cost.


                if (canUse)
                {
                    //Send card transform, area transform, current card viz and variables to Setting.
                    Setting.DropCreatureCard(_CardVar.value.transform
                        ,a.transform
                        ,currentCard
                        ,_CardVar.value);

                    _CardVar.value.currentLogic = _CardOnFieldLogic;
                    //currentCard.value.gameObject.layer = 9;

                    //isPlaced = true; // Set isPlace to true so another card can't be placed in this area. 
                    BoxCollider box =a.GetComponent<BoxCollider>();
                    box.enabled = false;
                    p.manaResourceManager.UpdateCurrentMana(-(currentCard.cardCost));


                    c.SetOriginFieldLocation(a.transform);
                    c.SetCanAttack(false);
                }
                c.gameObject.SetActive(true);
            }

            else if(currentCard.cardType == _SpellType)
            {
                Debug.Log("This is spell Card");
            }
        }
    }

}