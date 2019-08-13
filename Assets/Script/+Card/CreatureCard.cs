using UnityEngine;
using UnityEditor;

using GH.GameCard.CardAbility;
using GH.GameCard.CardInfo;

namespace GH.GameCard
{
    [CreateAssetMenu(menuName = "+Cards/CreatureCard")]
    public class CreatureCard : Card
    {
        #region Serialized

        private CreatureAbility _Ability;

        readonly private ConditionAttribute condition;
        

        #endregion

        #region Properties



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
            bool result = false;

            if(!condition.IsAtHand)         //Check Card Position
            {
                Debug.LogError("ConditionError: This card is not on hand.");
                return false;
            }
            //if(!this.Owner.fieldCard.Contains(this))



            return result;

        }

        public override bool CanUseCard()
        {
            bool result = false;

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
