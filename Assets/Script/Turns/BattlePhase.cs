using UnityEngine;
using System.Collections;

namespace GH
{
    [CreateAssetMenu(menuName = "Turns/Battle Phase_Player")]
    public class BattlePhase : Phase
    {
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
                Setting.gameController.SetState(null);
                Setting.gameController.OnPhaseChanged.Raise();
                isInit = true;
            }
        }
    }


}
