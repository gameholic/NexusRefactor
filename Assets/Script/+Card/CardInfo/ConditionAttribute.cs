using UnityEngine;
using System.Collections;

using GH.GameCard.CardState;

namespace GH.GameCard.CardInfo
{
    public class ConditionAttribute : MonoBehaviour
    {
        #region Variables
        private Card _OriginCard;

        private bool isAtHand;          // True when card is at hand, False when card is  on field
        private bool canAttack;
        private bool isAttacking;
        private bool isDead;
        private Transform fieldTransform;

        #endregion

        #region Properties
        
        public Card OriginalCard
        {
            set { _OriginCard = value; }
            get { return _OriginCard; }
        }
        public bool IsAtHand
        {
            set { isAtHand = value; }
            get { return isAtHand; }
        }
        public bool CanAttack
        {
            set { canAttack = value; }
            get { return canAttack; }
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
        public void SetOriginFieldLocation(Transform t)
        {
            fieldTransform = t;
        }
        public Transform GetOriginFieldLocation()
        {
            if (fieldTransform == null)  //This might occur if card instance is called by its id
            {
                Debug.LogErrorFormat("GetOriginalFieldLocationError: {0}'s {1} Field Location isn't saved",
                    this.OriginalCard.Owner, this.OriginalCard.Data[0].name);
                return null;
            }
            return fieldTransform;
        }

        #endregion


    }

}

