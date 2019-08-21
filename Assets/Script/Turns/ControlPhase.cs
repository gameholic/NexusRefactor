using UnityEngine;
using System.Collections;
using GH.GameAction;


namespace GH.GameTurn
{
    [CreateAssetMenu(menuName ="Turns/Control Phase_Player")]
    public class ControlPhase : Phase
    {
        public PlayerAction OnStartAction;
        public GameStates.State playerControlState;

        public override bool IsComplete()
        {
            if (PhaseForceExit)
            {
                PhaseForceExit = false;
                return true;
            }
            return false;
        }

        public override void OnEndPhase()
        {
            if (IsInit)
            {
                Setting.gameController.SetState(null);
                IsInit = false;
            }

        }

        public override void OnStartPhase()
        {
            if (!IsInit)
            {
                Setting.gameController.SetState(playerControlState);
                Setting.gameController.OnPhaseChanged.Raise();
                IsInit = true;
            }
            if(OnStartAction!= null)
            {
                OnStartAction.Execute(Setting.gameController.CurrentPlayer);
            }
        }
    }

}
