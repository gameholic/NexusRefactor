using UnityEngine;
using System.Collections;
using GH.GameStates;
namespace GH.GameTurn
{
    [CreateAssetMenu(menuName = "Turns/Battle Phase_Player")]
    public class BattlePhase : Phase
    {
        [SerializeField]
        private State _BattleStateControl;
        [SerializeField]
        private Condition _IsBattleValid;
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
                PhaseForceExit = !_IsBattleValid.IsValid();
                Setting.gameController.SetState((!PhaseForceExit)? _BattleStateControl : null);
                Setting.gameController.OnPhaseChanged.Raise();
                IsInit = true;
            }
        }

    }


}
