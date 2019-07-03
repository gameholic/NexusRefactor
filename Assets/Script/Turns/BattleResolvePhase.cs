using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GH.Multiplay;
using GH.GameCard;

namespace GH.GameTurn
{
    [CreateAssetMenu(menuName ="Turns/Battle Resolve")]
    public class BattleResolvePhase : Phase
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
            IsInit = false;
        }
        public override void OnStartPhase()
        {
            if(!IsInit)
            {
                IsInit = true;
                MultiplayManager.singleton.SetBattleResolvePhase();
            }
        }
    }
}
