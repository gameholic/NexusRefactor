using UnityEngine;
using System.Collections;
using UnityEditor;
using GH.GameCard;


/// <summary>
/// It runs when card is getting placed on area.
/// Checks what sort of the card is and can this card be placed in area. If not, return error message and stop running.
/// It works with field area game object
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
            bool checkOwner = Setting.gameController.CheckOwner.CheckPlayer(a.gameObject);
            if (!checkOwner)
            {
                Debug.Log("You cant control other player's obj");
                return;
            }

            if (_CardVar.value == null)
                Debug.Log("CARD VAR ERR");
            if (a.IsPlaced)
                Debug.Log("There is something in area");

            Card currentCard = _CardVar.value.viz.card;
            CardInstance c = _CardVar.value;
            if (currentCard.cardType == _CreatureType)
            {
                bool canUse = p.PayMana(currentCard);
                if (canUse)
                {
                    Setting.DropCreatureCard(_CardVar.value.transform
                        ,a.transform
                        ,currentCard
                        ,_CardVar.value);
                    _CardVar.value.currentLogic = _CardOnFieldLogic;
                    p.manaResourceManager.UpdateCurrentMana(-(currentCard.cardCost));
                    a.IsPlaced = true;
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