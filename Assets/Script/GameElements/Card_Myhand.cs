using UnityEngine;
using System.Collections;
using GH.GameCard;


namespace GH.GameElements
{
    [CreateAssetMenu(menuName ="GameElements/My Hand")]
    public class Card_Myhand : Instance_logic
    {
        [SerializeField]
        private GH.GameEvent onCurrentCardSelected;
        [SerializeField]
        private CardVariables currentCard;
        [SerializeField]
        private GH.GameStates.State holdingCard;

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