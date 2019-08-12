using UnityEngine;
using System.Collections;
using GH;
using GH.GameCard;
using GH.GameStates;

namespace GH.GameElements
{
    [CreateAssetMenu(menuName = "GameElements/My Field")]
    public class Card_Myfield : Instance_logic
    {
        [SerializeField]
        private State _BattleState;
        [SerializeField]
        private GameEvent _OnCurrentCardSelected;
        [SerializeField]
        private BattlePhaseStartCheck _StartBattle;

        [SerializeField]
        private CardVariables _CurrentSelectedCard;
        public override void LOnClick(CardInstance inst)
        {
            if (_StartBattle.IsValid())
            {
                _CurrentSelectedCard.Set(inst);
                Setting.gameController.SetState(_BattleState);
                _OnCurrentCardSelected.Raise();
            }
            else
            {
                Setting.RegisterLog(this.name + " startbattle is invalid", Color.red);
                return;
            }
        }
        public override void LOnHighlight(CardInstance inst)
        {
            
        }
    }
} 
