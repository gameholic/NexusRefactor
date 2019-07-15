using GH.GameCard;
using GH.Multiplay;
using System.Collections.Generic;
using UnityEngine;


namespace GH.CardBattle
{

    public class BattleLogics
    {

        private GameController gc
        {
            get { return GameController.singleton; }
        }

        /// <summary>
        /// Handles All Card Battles
        /// </summary>
        /// <param name="atkInst">Attacking card</param>
        /// <param name="blockInstance">Blocking Card Instances</param>
        /// <param name="mainData"></param>
        public int CardBattle(CardInstance atkInst, BlockInstance blockInstance ,MainDataHolder mainData)
        {
            int result = -1;
            Element atkElement = mainData.AttackElement;
            Element defElement = mainData.HealthElement;
            //Element abilityElement = maindData.AbilityElement;

            CardProperties defProperty = atkInst.viz.card.GetProperties(defElement);
            CardProperties atkProperty = atkInst.viz.card.GetProperties(atkElement);
            //CardProperties atkAbility = atkInst.viz.card.GetProperties(abilityElement);


            if (atkProperty == null)
            {
                Debug.LogError("Attacking card don't have attack element");
                return result;
            }
            if (defProperty == null)
            {
                Debug.LogError("Attacking card don't have attack element");
                return result;
            }

            Debug.Log("BattleLogics: Card Battle Begins");
            int atkLife = defProperty.intValue;
            int atkAttack = atkProperty.intValue;

            for (int index = 0; index < blockInstance.defenders.Count; index++)
            {
                CardInstance defInst = blockInstance.defenders[index];
                CardProperties defenderLife = defInst.viz.card.GetProperties(defElement);
                CardProperties defenderStr = defInst.viz.card.GetProperties(atkElement);
                if (defenderLife == null)
                {
                    Debug.LogWarning("You are trying to block with a card with no health element");
                    continue;
                }
                int defLife = defenderLife.intValue;
                int defAttack = defenderStr.intValue;




                atkAttack -= defLife;
                if (defLife <= atkAttack)
                {
                    Debug.LogFormat("CardBattle: {0}'s {1} Killed {2}",atkInst.owner.player, atkInst.viz.card.name, defInst.viz.card.name);
                    SetCardToGrave(blockInstance.defenders[index]);
                }
                atkLife -= defAttack;
                if(atkLife<=0)
                {
                    Debug.LogFormat("CardBattle: {0}'s {1} Killed by {2} during attack ", atkInst.owner.player, atkInst.viz.card.name, defInst.viz.card.name);
                    atkLife = 0;
                    SetCardToGrave(atkInst);
                    break;
                }
            }
            result = atkAttack;
            return result;
        }
        private void SetCardToGrave(CardInstance c)
        {
            c.CardInstanceToGrave();
            MultiplayManager.singleton.SendCardToGrave(c.owner.PhotonId);
        }
        public void AttackerWinFight(CardInstance atkInst, int damage )
        {
            PlayerHolder currentPlayer = gc.CurrentPlayer;
            PlayerHolder enemy = gc.GetOpponentOf(currentPlayer);

            enemy.DropCardOnField(atkInst, false);
            currentPlayer.DoDamage(damage);
            Debug.LogFormat("BattleResult_AttackerWin: {0} took damage of {1}", currentPlayer.player, damage);
        }
    }
}