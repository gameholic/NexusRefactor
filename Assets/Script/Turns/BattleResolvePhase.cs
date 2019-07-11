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
            Debug.Log("Check How Many Time Battle Resolve Ends");
            IsInit = false;
        }
        public override void OnStartPhase()
        {
            Debug.Log("Check How Many Time Battle Resolve Starts");
            if(!IsInit)
            {
                IsInit = true;
                MultiplayManager.singleton.SetBattleResolvePhase();
            }
            else
            {
                Debug.LogError("BattleResolveOnStartPhase: IsInit is true. BattleResolveOut");
            }
        }
    }
}
