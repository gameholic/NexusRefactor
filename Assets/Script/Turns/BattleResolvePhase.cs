using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GH.Multiplay;

namespace GH.GameTurn
{
    [CreateAssetMenu(menuName ="Turns/Battle Resolve")]
    public class BattleResolvePhase : Phase
    {
        public override bool IsComplete()
        {
            if(PhaseForceExit)
            {
                PhaseForceExit = false;
                return true;
            }
            
            return false;
        }
        /*
        BlockInstance GetBlockInstanceOfAttacker(CardInstance attck, Dictionary<CardInstance, BlockInstance> blockInst)
        {
            BlockInstance r = null;
            blockInst.TryGetValue(attck, out r);
            return r;

        }*/

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
