using UnityEngine;

namespace GH.GameCard.CardInfo
{
    public class ConditionAttribute : MonoBehaviour
    {
        #region Variables
        private NewCard _OriginCard;
        private bool canAttack;
        private bool isAttacking;
        private bool isDead;
        private Transform fieldTransform;

        #endregion

        #region Properties
        
        public NewCard OriginalCard
        {
            set { _OriginCard = value; }
            get { return _OriginCard; }
        }

        public bool CanAttack
        {
            set { canAttack = value; }
            get
            {
                if (_OriginCard.Data.Attack == 0)
                    canAttack = false;
                return canAttack;
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
        public void SetOriginFieldLocation(Transform t)
        {
            fieldTransform = t;
        }       
        public Transform GetOriginFieldLocation()
        {
            if (fieldTransform == null)  //This might occur if card instance is called by its id
            {
                Debug.LogErrorFormat("GetOriginalFieldLocationError: {0}'s {1} Field Location isn't saved",
                    this.OriginalCard.Owner, this.OriginalCard.Data.Name);
                return null;
            }
            return fieldTransform;
        }

        #endregion


    }

}

