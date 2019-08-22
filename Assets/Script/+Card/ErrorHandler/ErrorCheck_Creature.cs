using GH.GameCard.CardInfo;
using GH.GameElements;
using UnityEngine;
namespace GH.GameCard.ErrorCheck
{
    public class ErrorCheck_Creature        
    {
        public bool IsAttacking(CreatureCard c)
        {
            bool result = false;
            if (c.User.CardManager.attackingCards.Contains(c.Data.UniqueId))
                result = true;
            if(!result)
                Debug.LogErrorFormat("IsAttackingError: {0} is not in  the  attacking card list");
            return result;
        }
        /// <summary>
        /// Check if 'c' can be dropped on field
        /// 1. c is in hand,
        /// 2. Player can afford its mana cost.
        /// 3. Area is empty and ready to be placed.
        /// Returns True if all conditions are good.
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public bool CheckCanDrop(CreatureCard c, Area a=null)
        {
            bool result = false;
            Player.PlayerHolder p = c.User;
            int card = c.Data.UniqueId;
            if (!p.CardManager.handCards.Contains(card)
                || p.InGameData.ManaManager.CurrentMana < c.Data.ManaCost)
                return result;
            if(a!=null)
                if (a.IsPlaced)
                return result;
            else
                result = true;
            return result;
        }
        /// <summary>
        /// Check If 'c' can attack now
        /// 1. c is  on  field.
        /// 2. c is able to attack.
        /// 3. c is not attacking.
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public bool CheckCanAttack(CreatureCard c )
        {
            bool result = false;
            ConditionAttribute condition = c.CardCondition;
            if (!c.PhysicalCondition.IsOnField()
                || !condition.CanUse 
                || !condition.IsAttacking)
            {
                return result;
            }
            else
                result = true;
            return result;
        }
                    
        public bool CheckCanBlock(CreatureCard c)
        {
            bool result = false;
            if (c.CardCondition.CanUse  
                || c.PhysicalCondition.IsOnField())
                result = true;
            if (!result)
                Debug.LogError("CanBlockError: {0} can't block", c);
            return result;
        }   
    }
}
