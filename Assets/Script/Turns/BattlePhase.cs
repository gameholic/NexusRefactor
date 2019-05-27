using UnityEngine;
using System.Collections;

namespace GH.GameTurn
{
    [CreateAssetMenu(menuName = "Turns/Battle Phase_Player")]
    public class BattlePhase : Phase
    {
        public GameStates.State battlePhaseControl;
        public Condition isBattleValid;
        public override bool IsComplete()
        {
            if (_PhaseForceExit)
            {
                _PhaseForceExit = false;
                return true;
            }
            return false;
        }

        public override void OnEndPhase()
        {
            if (_IsInit)
            {
                Setting.gameController.SetState(null);
                _IsInit = false;
            }            
        }

        public override void OnStartPhase()
        {
            if (!_IsInit)
            {
                _PhaseForceExit = !isBattleValid.IsValid();
                Setting.gameController.SetState((!_PhaseForceExit)? battlePhaseControl : null);
                Setting.gameController.OnPhaseChanged.Raise();
                _IsInit = true;
            }
        }

    }


}
