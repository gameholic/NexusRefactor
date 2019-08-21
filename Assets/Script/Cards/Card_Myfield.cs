//using GH.GameStates;
//using UnityEngine;

//namespace GH.GameCard.CardState
//{
//    [CreateAssetMenu(menuName = "GameElements/My Field")]
//    public class Card_Myfield : CardStateLogic
//    {
//#pragma warning disable 0649
//        [SerializeField]
//        private State _BattleState;
//        [SerializeField]
//        private GameEvent _OnCurrentCardSelected;
//        [SerializeField]
//        private BattlePhaseStartCheck _StartBattle;
//        [SerializeField]
//        private CardVariables _CurrentSelectedCard;
//#pragma warning restore 0649
//        public override void LOnClick(CardInstance inst)
//        {
//            if (_StartBattle.IsValid())
//            {
//                _CurrentSelectedCard.Set(inst);
//                Setting.gameController.SetState(_BattleState);
//                _OnCurrentCardSelected.Raise();
//            }
//            else
//            {
//                Setting.RegisterLog(this.name + " startbattle is invalid", Color.red);
//                return;
//            }
//        }
//        public override void LOnHighlight(CardInstance inst)
//        {
            
//        }
//    }
//} 
