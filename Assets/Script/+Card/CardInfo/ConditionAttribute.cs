using UnityEngine;

namespace GH.GameCard.CardInfo
{
    public class ConditionAttribute 
    {
        #region Variables
        private Card _OriginCard;
        private bool canUse;
        private bool isAttacking;
        private bool isDead;

        #endregion

        #region Properties
        
        public Card OriginCard
        {
            set { _OriginCard = value; }
            get { return _OriginCard; } 
        }

        public bool CanUse
        {
            set { canUse = value; }
            get
            {
                //if (_OriginCard.Data.Attack == 0)
                //    canUse = false;
                return canUse;
            }
        }
        public bool IsAttacking
        {
            set { isAttacking = value; }
            get { return isAttacking; }
        }
        public bool IsDead
        {
            set { isDead = value; }
            get { return isDead; }
        }
        #endregion


    }

}

