using UnityEngine;
using System.Collections;


namespace GH.GameTurn
{  
    [CreateAssetMenu(menuName ="Turns/ClientEmptyPhase")]
    public class ClientEmptyPhase : Phase
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

        public override void OnEndPhase()
        {

        }

        public override void OnStartPhase()
        {
            PhaseForceExit = false;
        }
    }

}