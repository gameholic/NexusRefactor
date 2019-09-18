﻿using GH.GameCard.CardAbility;
using GH.GameCard.CardInfo;
using GH.GameCard.ErrorCheck;
using GH.GameElements;
using GH.GameTurn;
using UnityEngine;

namespace GH.GameCard
{
    [CreateAssetMenu(menuName = "Card/CreatureCard")]
    public class CreatureCard : Card
    {
        #region Serialized
        //private CreatureAbility _Ability;
        private ErrorCheck_Creature errorCheck = new ErrorCheck_Creature();

        #endregion
        #region Properties
        #endregion
        #region Init
        #endregion
        /// Below codes are run after considering state and phases are checked
        #region ManipulateCard

        ///<summary>
        ///Initialise Card. Set Physical, Condition Attributes and connect them
        ///</summary>
        public override void Init(GameObject go)
        {
            _PhysicInstance = go.GetComponent<PhysicalAttribute>();
            _ConditionAttribute = new ConditionAttribute();
            CardCondition.OriginCard = this;
            PhysicalCondition.OriginCard = this;

        }
        /// <summary>
        /// Check Conditions through Error Handler.
        /// </summary>
        /// <returns></returns>
        public override bool CanDropCard()
        {
            bool result = false;
            Area a = Setting.RayCastArea(); 
            if (errorCheck.CheckAreaCondition(a) && errorCheck.CheckCanDrop(this))
            {
                result = true;
                Debug.Log("All Condtions are good");
                this.PhysicalCondition.SetOriginFieldLocation(a.transform);
            }
            return result;
        }
        public override bool CanUseCard()   
        {
            bool result = false;
            if (errorCheck.CheckCanDrop(this))         //Check Card Position
            {
                result = true;
            }
            return result;
        }
        /// <summary>
        /// Check card is attackable or blockable
        /// On Battle Phase, check error if it can attack 
        /// On Block Phase, check error if it can block
        /// </summary>
        /// <returns></returns>
        public override bool UseCard()
        {
            Phase phase = GameController.singleton.CurrentPhase;
            bool result = false;
            if(phase is BlockPhase)
            {
                if(errorCheck.CheckCanBlock(this))
                {
                    
                    result = true;
                }                
            }
            else if(phase is BattlePhase)
            {
                if (errorCheck.CheckCanAttack(this))         //Check Card Condition
                {
                    result = true;
                }
            }
            else { Debug.LogWarningFormat("UseCard: Current Phase is Neither block or battle"); }
            return result;
        }

        #endregion

    }
}
