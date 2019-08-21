using GH.Player;
using GH.GameCard.CardInfo; 

namespace GH.GameCard.ErrorCheck
{
    public class ErrorCheck_Creature        
    {
        public bool IsAttacking(CreatureCard c)
        {
            bool result = false;
            if (c.User.CardManager.attackingCards.Contains(c.Data.UniqueId))
                result = true;
            return result;
        }
        public bool CheckCanDrop(Card c)
        {
            bool result = false;
            Player.PlayerHolder p = c.User;
            int card = c.Data.UniqueId;
            if (!p.CardManager.handCards.Contains(card)
                || p.InGameData.ManaManager.CurrentMana < c.Data.ManaCost)
                return result;
            else
                result = true;
            return result;
        }
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
            return result;
        }   
    }
}