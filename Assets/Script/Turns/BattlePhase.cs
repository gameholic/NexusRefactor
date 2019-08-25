using UnityEngine;
using System.Collections;
using GH.GameStates;
namespace GH.GameTurn
{
    [CreateAssetMenu(menuName = "Turns/Battle Phase_Player")]
    public class BattlePhase : Phase
    {
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
                IsInit = false;
            }            
        }
        public override void OnStartPhase()
        {
            if (!IsInit)
            {
                //PhaseForceExit = !_IsBattleValid.IsValid();
                //Setting.gameController.SetState((!PhaseForceExit) ? _BattleStateControl : null);
                Setting.gameController.OnPhaseChanged.Raise();
                IsInit = true;
            }
            else
            {
                Debug.Log("BattlePhaseError: OnStartPhase_IsInit is true");
            }
        }

    }


}
