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
                int usableCards = 0;
                PlayerHolder enemy = gc.GetOpponentOf(gc.CurrentPlayer);
                if (enemy.attackingCards.Count == 0)
                {
                    PhaseForceExit = true;
                    return;
                }
                foreach (CardInstance c in gc.CurrentPlayer.fieldCard)
                {
                    if (!c.GetCanAttack())
                    {
                        usableCards++;
                    }
                }
                if (usableCards < 1)
                {
                    Debug.Log("No UsableCards");
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