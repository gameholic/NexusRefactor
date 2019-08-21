//using UnityEngine;


//namespace GH.GameCard.CardState
//{
//    [CreateAssetMenu(menuName ="GameElements/My Hand")]
//    public class Card_Myhand : CardStateLogic
//    {
//        [SerializeField]
//        private GH.GameEvent onCurrentCardSelected;
//        [SerializeField]
//        private CardVariables currentCard;
//        [SerializeField]
//        private GH.GameStates.State leaguholdingCard;

//        public override void LOnClick(CardInstance inst)
//        {
//            currentCard.Set(inst);
//            Setting.gameController.SetState(holdingCard);
//            onCurrentCardSelected.Raise();
//        }
//        public override void LOnHighlight(CardInstance inst)
//        {

//        }
//    }
//}