using GH.GameCard.CardAbility;
using GH.GameCard.CardInfo;
using GH.GameCard.ErrorCheck;
using UnityEngine;

namespace GH.GameCard
{
    [CreateAssetMenu(menuName = "+Cards/CreatureCard")]
    public class CreatureCard : NewCard
    {
        #region Serialized

        private CreatureAbility _Ability;

        readonly private ConditionAttribute condition  = new ConditionAttribute();
        

        #endregion

        #region Properties

        public ConditionAttribute Condition
        {
            get { return condition; }
        }

        #endregion

        #region Init

        private void Awake()
        {
            condition.OriginalCard = this;
        }
        #endregion

        /// Below codes are run after considering state and phases are checked
        #region ManipulateCard

        /// <summary>
        /// Returns card can be used.
        /// This only runs when card is on hand;
        /// </summary>
        public override bool CanDropCard()
        {
            CardErrorHandler errorCheck = new CardErrorHandler();

            bool result = false;

            if(errorCheck.CheckCanDrop(this))         //Check Card Position
            {
                Debug.LogError("ConditionError: This card is not on hand.");
                result  = true;
            }


            return result;

        }

        public override bool CanUseCard()
        {
            CardErrorHandler errorCheck = new CardErrorHandler();
            bool result = false;

            if (errorCheck.CheckCanDrop(this))         //Check Card Position
            {
                result = true;
            }
            return result;
        }

        /// <summary>
        /// Check card is attackable
        /// </summary>
        /// <returns></returns>
        public override bool UseCard()
        {
            bool result = false;

            return result;
        }

        #endregion

    }
}
