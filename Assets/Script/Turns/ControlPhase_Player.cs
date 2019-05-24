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
            if (forceExit)
            {
                forceExit = false;
                return true;
            }
            return false;
        }

        public override void OnEndPhase()
        {
            if (isInit)
            {
                Setting.gameController.SetState(null);
                isInit = false;
            }

        }

        public override void OnStartPhase()
        {
            if (!isInit)
            {
                Setting.gameController.SetState(playerControlState);
                Setting.gameController.OnPhaseChanged.Raise();
                isInit = true;
            }

            if(OnStartAction!= null)
            {
                OnStartAction.Execute(Setting.gameController.currentPlayer);
            }
        }
    }

}
