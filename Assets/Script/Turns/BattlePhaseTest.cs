﻿using UnityEngine;
using System.Collections;

namespace GH
{
    [CreateAssetMenu(menuName = "Turns/Battle PhaseTest_Player")]
    public class BattlePhaseTest : Phase
    {
        public GameStates.State battlePhaseControl;
        public Condition isBattleValid;
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
                forceExit = !isBattleValid.IsValid();
                Setting.gameController.SetState((!forceExit)? battlePhaseControl : null);
                Setting.gameController.OnPhaseChanged.Raise();
                isInit = true;
            }
        }
    }


}
