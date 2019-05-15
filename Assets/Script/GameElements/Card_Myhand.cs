using UnityEngine;
using System.Collections;

namespace GH.GameElements
{
    [CreateAssetMenu(menuName ="GameElements/My Hand")]
    public class Card_Myhand : Instance_logic
    {
        public GH.GameEvent onCurrentCardSelected;
        public CardVariables currentCard;
        public GH.GameStates.State holdingCard;

        public override void OnClick(CardInstance inst)
        {
            currentCard.Set(inst);
            Setting.gameController.SetState(holdingCard);
            onCurrentCardSelected.Raise();
        }
        public override void OnHighlight(CardInstance inst)
        {

        }
    }
}