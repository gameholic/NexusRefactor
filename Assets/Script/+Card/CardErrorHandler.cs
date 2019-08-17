using GH.Player;
using GH.GameCard.CardInfo; 

namespace GH.GameCard.ErrorCheck
{

    public class CardErrorHandler        
    {
        public bool CheckCanDrop(NewCard c)
        {
            bool result = false;
            NewPlayerHolder p = c.User;
            int card = c.Data.UniqueId;
            if(!p.CardManager.handCards.Contains(card))
                return result;
            else if (p.InGameData.CurrentMana < c.Data.ManaCost)
                return result;
            else
                result = true;
            return result;
        }

        public bool CheckCanAttack(CreatureCard c )
        {
            bool result = false;
            ConditionAttribute condition = c.Condition;
            if (!c.User.CardManager.fieldCard.Contains(c.Data.UniqueId)) 
            {
                return result;
            }
            else if (!condition.CanAttack || condition.IsAttacking)
            {
                return result;
            }
            result = true;


            return result;
        }

    }
}