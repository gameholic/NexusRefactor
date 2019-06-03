using UnityEngine;
using System.Collections;
using GH.GameAction;
namespace GH.GameTurn
{
    [CreateAssetMenu(menuName ="Turns/BlockPhase")]
    public class BlockPhase : Phase
    {
        public PlayerAction changeActivePlayer;
        public GameStates.State playerControlState;
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
            GameController gc = Setting.gameController;
            if (!IsInit)
            {
                gc.SetState(playerControlState);
                gc.OnPhaseChanged.Raise();
                IsInit = true;
                //As attacking cards are on field without attack, forceExit is false (Which means can't getaway from loop)
            }
            
            if(gc.CurrentPlayer.attackingCards.Count == 0)
            {
                PhaseForceExit = true;
                return; 
            }
            if(gc.TopCardHolder.thisPlayer.isHumanPlayer /*&& forceExit == true*/)
            {
                gc.LoadPlayerUI.LoadPlayerOnActive(gc.TopCardHolder.thisPlayer);
                //forceExit = true; // add code that other player blocks
            }
            else
            {

            }
        }

    }


}