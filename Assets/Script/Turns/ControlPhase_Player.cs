using UnityEngine;
using System.Collections;


namespace GH.GameTurn
{
    [CreateAssetMenu(menuName ="Turns/Control Phase_Player")]
    public class ControlPhase_Player : Phase
    {
        public PlayerAction OnStartAction;
        public GameStates.State playerControlState;
        public override bool IsComplete()
        {
            if (_PhaseForceExit )
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
                Setting.gameController.SetState(playerControlState);
                Setting.gameController.OnPhaseChanged.Raise();
                _IsInit = true;
            }

            if(OnStartAction!= null)
            {
                OnStartAction.Execute(Setting.gameController.currentPlayer);
            }
        }
    }

}
