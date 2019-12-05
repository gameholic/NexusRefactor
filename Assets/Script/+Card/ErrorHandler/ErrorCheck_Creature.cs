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
            if (c.User.CardManager.attackingCards.Contains(c.GetCardData.UniqueId))
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
        public bool CheckCanDrop(Card c)
        {
            Debug.Log("Check Card Can Be dropped");
            bool result = false;
            Player.PlayerHolder p = c.User;
            int card = c.GetCardData.UniqueId;
            if (!p.CardManager.handCards.Contains(card))
                Debug.LogError("This Card Isn't In Hand");
            if (c.User.InGameData.ManaManager.CurrentMana < c.GetCardData.ManaCost)      //if card's mana cost is higher than current mana
                Debug.LogError("CurrentMana Can't afford the card cost");
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
                    
        /// <summary>
        /// Check Conditions for Blocking
        /// 1. This card can be used.
        /// 2. This card is on field.
        /// </summary>
        /// <param name="def"></param>
        /// <returns></returns>
        public bool CheckCanBlock(CreatureCard def)
        {
            bool result = false;
            if (def.CardCondition.CanUse  
                || def.PhysicalCondition.IsOnField())
                result = true;
            if (!result)
                Debug.LogError("CanBlockError: {0} can't block", def);
            return result;
        }   

        public bool CheckAreaCondition(Area a)
        {
            bool result = false;
            if (a == null)
            {
                Debug.LogError("AreaIsNUll");
                return result;
            }
            if (a.gameObject.transform.childCount > 0)           //Check if there is other object in Area
                Debug.LogError("There is something in Area");
            else
            {
                Debug.Log("Area Error Checked. You can Drop Card here");
                result = true;
            }
            return result;
        }

        public bool CheckAttackingCard(CreatureCard def, CreatureCard atk)
        {
            bool ret = false;
            if (atk.User == def.User)
                Debug.Log("You can't block your own card");
            else if (atk.GetCardData.UniqueId == def.GetCardData.UniqueId)
                Debug.Log("Attacking Card's unique Id is same as defending Card's unique Id");
            else if (!IsAttacking(atk))
                Debug.Log("This card isn't attacking now");
            else
                ret = true;

            return ret;
        }
    }
}
