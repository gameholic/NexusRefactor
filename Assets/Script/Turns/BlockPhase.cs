using UnityEngine;
using System.Collections;

namespace GH.GameTurn
{
    [CreateAssetMenu(menuName ="Turns/BlockPhase")]
    public class BlockPhase : Phase
    {
        public PlayerAction changeActivePlayer;
        public GameStates.State playerControlState;
        public override bool IsComplete()
        {
            if (_PhaseForceExit)
            {
                _PhaseForceExit = false;
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
            GameController gc = Setting.gameController;
            if (!isInit)
            {
                gc.SetState(playerControlState);
                gc.OnPhaseChanged.Raise();
                isInit = true;
                //As attacking cards are on field without attack, forceExit is false (Which means can't getaway from loop)
            }
            
            if(gc.currentPlayer.attackingCards.Count == 0)
            {
                _PhaseForceExit = true;
                return; 
            }
            if(gc.topCardHolder.thisPlayer.isHumanPlayer /*&& forceExit == true*/)
            {
                gc.LoadPlayerOnActive(gc.topCardHolder.thisPlayer);
                //forceExit = true; // add code that other player blocks
            }
            else
            {

            }
        }

    }


}