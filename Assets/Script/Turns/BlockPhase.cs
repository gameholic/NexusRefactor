using UnityEngine;
using System.Collections;
using GH.GameAction;
using GH.GameCard;
namespace GH.GameTurn
{
    [CreateAssetMenu(menuName ="Turns/BlockPhase")]
    public class BlockPhase : Phase
    {
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
                int availableCards = 0;
                PlayerHolder enemy = gc.GetOpponentOf(gc.CurrentPlayer);


                if (enemy.attackingCards.Count == 0)
                {
                    PhaseForceExit = true;
                    Debug.Log("ForceExit: " + PhaseForceExit);
                    return;
                }
                foreach (CardInstance c in gc.CurrentPlayer.fieldCard)
                {
                    if(!c.GetCanAttack())
                    {
                        availableCards++;
                        Debug.Log(gc.CurrentPlayer + "  " + availableCards);
                    }
                }
                if (availableCards <= 0)
                {
                    Debug.Log("There are no available cards");
                    PhaseForceExit = true;
                    return;
                }


                //As attacking cards are on field without attack, forceExit is false (Which means can't get away from loop)
                //if (gc.CurrentPlayer.attackingCards.Count == 0)
                //if (gc.GetOpponentOf(gc.CurrentPlayer).attackingCards.Count == 0)
                //{
                //    PhaseForceExit = true;
                //    return;
                //}
            }
        }
    }
}