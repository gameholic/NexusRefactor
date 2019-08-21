using UnityEngine;
using GH.GameCard;
using GH.Multiplay;
using GH.GameCard.CardInfo;
using GH.Player;
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

#pragma warning disable 0649
        [SerializeField]
        private CardVariables _CardVar;
#pragma warning restore 0649
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

            Card thisCard = _CardVar.value;
            if (thisCard.Data.CardType == CardType.Creature)
            {
                bool canUse = p.InGameData.ManaManager.HaveEnoughMana(thisCard.Data.ManaCost);
                if (canUse)
                {
                    int fieldCode = 0;
                    //thisCard.Instance.SetOriginFieldLocation(fieldArea.transform); //Maybe this should go to setting
                    for (int i =0; i<p.CardTransform.GetFieldGrid().Length; i++)
                    {
                        if (fieldArea.transform.name == p.CardTransform.GetFieldGrid(i).value.name)
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

                    fieldArea.IsPlaced = true;
                    MultiplayManager.singleton.PlayerTryToUseCard
                        (thisCard.Data.UniqueId, GameController.singleton.LocalPlayer.InGameData.PhotonId,
                        MultiplayManager.CardOperation.dropCreatureCard, fieldCode);

                }
                else
                {
                    Setting.RegisterLog("Can't use card", Color.red);
                }
            }

            else if(thisCard.Data.CardType == CardType.Spell)
            {
                Debug.Log("This is spell Card");
            }
        }
    }

}