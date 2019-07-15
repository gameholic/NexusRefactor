using UnityEngine;
using System.Collections;
using GH.GameAction;
using GH.GameCard;
namespace GH.GameTurn
{
    [CreateAssetMenu(menuName ="Turns/BlockPhase")]
    public class BlockPhase : Phase
    {
        public GameStates.State playerBlockState;
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
                IsInit = true;
                int usableCards = 0;
                PlayerHolder enemy = gc.GetOpponentOf(gc.CurrentPlayer);

                //Exit logic 1: If there is no attacking cards to block. this phase don't need to be initiated. End turn
                if (enemy.attackingCards.Count == 0)
                {
                    Debug.LogFormat("{0}, BlockPhase_OnStart: Can't find enemy ({1}) attacking cards", gc.CurrentPlayer.player, enemy.player);
                    PhaseForceExit = true;
                    return;
                }
                foreach (CardInstance c in gc.CurrentPlayer.fieldCard)
                {
                    if (c.GetAttackable())
                    {
                        usableCards++;
                    }
                }

                //Exit logic 2: If there is no cards to block enemy card, this phase don't need to be initiated. End turn
                if (usableCards < 1)
                {
                    Debug.LogFormat("{0}, BlockPhase_OnStart: There is no blockable cards", gc.CurrentPlayer.player);
                    PhaseForceExit = true;
                    return;
                }
                gc.SetState(playerBlockState);
                gc.OnPhaseChanged.Raise();

            }
        }
    }
}