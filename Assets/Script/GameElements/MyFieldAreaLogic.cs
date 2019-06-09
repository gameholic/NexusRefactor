using UnityEngine;
using GH.GameCard;
using GH.Multiplay;

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
        //[SerializeField]
        //private Instance_logic _CardOnFieldLogic;

        public override void Execute(GameElements.Area a)
        {
            PlayerHolder p = Setting.gameController.CurrentPlayer;
            bool checkOwner = Setting.gameController.CheckOwner.CheckPlayer(a.gameObject);

            if (!checkOwner)
            {
                Debug.Log("You cant control other player's obj");
                return;
            }
            if (a.IsPlaced)
                Debug.Log("There is something in area");

            Card thisCard = _CardVar.value.viz.card;
            if (thisCard.cardType == _CreatureType)
            {
                bool canUse = p.PayMana(thisCard);
                if (canUse)
                {
                    thisCard.Instance.SetOriginFieldLocation(a.transform);
                    a.IsPlaced = true;
                    MultiplayManager.singleton.PlayerTryToUseCard
                        (thisCard.InstId, GameController.singleton.LocalPlayer.PhotonId,
                        MultiplayManager.CardOperation.dropCreatureCard);
                }
                else
                {
                    Setting.RegisterLog("Can't use card", Color.red);
                }
            }

            else if(thisCard.cardType == _SpellType)
            {
                Debug.Log("This is spell Card");
            }
        }
    }

}