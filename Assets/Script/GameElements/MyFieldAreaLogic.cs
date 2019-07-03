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

        public override void Execute(GameElements.Area fieldArea)
        {
            PlayerHolder p = Setting.gameController.CurrentPlayer;
            bool checkOwner = Setting.gameController.CheckOwner.CheckPlayer(fieldArea.gameObject);

            if (!checkOwner)
            {
                Debug.Log("You cant control other player's obj");
                return;
            }
            if (fieldArea.IsPlaced)
            {
                Debug.Log("There is something in area");
                return;

            }

            Card thisCard = _CardVar.value.viz.card;
            if (thisCard.cardType == _CreatureType)
            {
                bool canUse = p.PayMana(thisCard);
                if (canUse)
                {
                    int fieldCode = 0;
                    thisCard.Instance.SetOriginFieldLocation(fieldArea.transform);
                    for(int i =0; i<p._CardHolder.GetFieldGrid().Length; i++)
                    {
                        if (fieldArea.transform.name == p._CardHolder.GetFieldGrid(i).value.name)
                        {
                            fieldCode = i;
                            break;
                        }

                    }
                    
                    if(fieldCode ==0)
                    { 
                        //If there is no field area for this card, send it to trashArea.
                        //This is for checking error. So when game is on release, delete this code.
                        Debug.LogError("Cant find fieldCode ");
                    }
                    else
                    {
                        Debug.Log("Feildcode is " + fieldCode);
                    }
                    fieldArea.IsPlaced = true;
                    MultiplayManager.singleton.PlayerTryToUseCard
                        (thisCard.InstId, GameController.singleton.LocalPlayer.PhotonId,
                        MultiplayManager.CardOperation.dropCreatureCard, fieldCode);

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