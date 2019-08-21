using GH.GameCard;

using GH.GameCard.CardLogics;
using GH.Multiplay;
using GH.Player;
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
        /// Returns damage to player
        /// </summary>
        /// <param name="atkInst">Attacking card</param>
        /// <param name="blockInstance">Blocking Card Instances</param>
        /// <param name="mainData"></param>
        public int CardBattle(CreatureCard atkInst, BlockInstance blockInstance ,MainDataHolder mainData)
        {
            int result = -1;
            Element atkElement = mainData.AttackElement;
            Element defElement = mainData.HealthElement;
            //Element abilityElement = maindData.AbilityElement;

            int atkLife = atkInst.Data.Defend;
            int atkAttack = atkInst.Data.Attack;
            //CardProperties atkAbility = atkInst.viz.card.GetProperties(abilityElement);


            if (atkLife == 0)
            {
                Debug.LogError("Attacking card don't have Life element");
                return result;
            }
            if (atkAttack == 0)
            {
                Debug.LogError("Attacking card don't have attack element");
                return result;
            }


            if (blockInstance != null)
            {
                for (int index = 0; index < blockInstance.defenders.Count; index++)
                {
                    Card defInst = blockInstance.defenders[index];
                    int defLife = defInst.Data.Defend;
                    int defAttack = defInst.Data.Attack;
                    if (defLife == 0)
                    {
                        Debug.LogWarning("You are trying to block with a card with no health element");
                        continue;
                    }


                    Debug.LogFormat("CARD BATTLE STARTS: Attcker({0}) Health: {1} Attack: {2} VS Defender({3}) Health: {4} Attack: {5}",
                        atkInst.User.PlayerProfile.UniqueId, atkLife, atkAttack,
                        defInst.User.PlayerProfile.UniqueId, defLife, defAttack);



                    int tmpAtk = atkAttack;
                    atkAttack -= defLife;

                    defLife -= tmpAtk;
                    atkLife -= defAttack;

                    Debug.LogFormat("CARD BATTLE RESULT: Attcker({0}) Health: {1}. Defender({2}) Health: {3}",
                        atkInst.User.PlayerProfile.UniqueId, atkLife,
                        defInst.User.PlayerProfile.UniqueId, defLife);
                    if (defLife <= atkAttack)
                    {
                        Debug.LogFormat("CardBattle: Defender( {0} )'s Card {1} Killed by {2}",
                            defInst.User.PlayerProfile.UniqueId, defInst.Data.Name, atkInst.Data.Name);
                        SetCardToGrave(defInst);
                    }
                    if (atkLife <= 0)
                    {
                        Debug.LogFormat("CardBattle: Attacker( {0} )'s Card {1} is Killed by {2} during attack ",
                            atkInst.User.PlayerProfile.UniqueId, atkInst.Data.Name, defInst.Data.Name);
                        atkLife = 0;
                        SetCardToGrave(atkInst);
                        break;
                    }
                }
            }
            else
            {
                Debug.Log("BattleLogicCardBattle: Block Instance of attacking card is null");
            }

            result = atkAttack;
            return result;
        }
        private void SetCardToGrave(Card c)
        {
            MoveCardInstance.SetCardToGrave(c);
            MultiplayManager.singleton.SendCardToGrave(c.User.InGameData.PhotonId);
        }
        public void AttackerWinFight(CreatureCard atkInst, int damage )
        {
            PlayerHolder currentPlayer = gc.CurrentPlayer;
            PlayerHolder enemy = gc.GetOpponentOf(currentPlayer);

            enemy.CardManager.DropCardOnField(atkInst);
            currentPlayer.InGameData.DoDamage(damage);
            if(damage>0)
                Debug.LogFormat("BattleResult_AttackerWin: {0} took damage of {1}", currentPlayer.PlayerProfile.UniqueId, damage);
        }
    }
}