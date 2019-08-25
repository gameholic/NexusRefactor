using GH.GameCard;
using GH.Player;
using UnityEngine;
namespace GH.GameTurn
{
    [CreateAssetMenu(menuName ="Turns/BlockPhase")]
    public class BlockPhase : Phase
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
            if (IsInit)
            {
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
                if (enemy.CardManager.attackingCards.Count == 0)
                {
                    Debug.LogFormat("{0}, BlockPhase_OnStart: Can't find enemy ({1}) attacking cards",
                        gc.CurrentPlayer.PlayerProfile.UniqueId, enemy.PlayerProfile.UniqueId);
                    PhaseForceExit = true;
                    return;
                }
                foreach (int instId in gc.CurrentPlayer.CardManager.fieldCards)
                {
                    Card c = gc.CurrentPlayer.CardManager.SearchCard(instId);
                    if (c.CardCondition.CanUse)
                    {
                        usableCards++;
                    }
                }

                //Exit logic 2: If there is no cards to block enemy card, this phase don't need to be initiated. End turn
                if (usableCards < 1)
                {
                    Debug.LogFormat("{0}, BlockPhase_OnStart: There is no blockable cards", gc.CurrentPlayer.PlayerProfile.UniqueId);
                    PhaseForceExit = true;
                    return;
                }
                gc.OnPhaseChanged.Raise();

            }
        }
    }
}