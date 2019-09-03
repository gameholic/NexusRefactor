using UnityEngine;
using GH.GameCard.ErrorCheck;
using UnityEditor;


namespace GH.GameCard
{
    public class SpellCard : Card
    {
        #region Serialized

        private ErrorCheck_Spell errorCheck = new ErrorCheck_Spell();

        #endregion

        public override void Init(GameObject go)
        {

        }
        public override bool CanDropCard()
        {
            Debug.Log("This is Spell Card. Can't be dropped");

            return false;
        }
        public override bool CanUseCard()
        {
            bool ret = false;
            if(errorCheck.CheckCanSpell(this))
            {
                ret = true;
            }
            return ret;
        }
        public override bool UseCard()
        {
            bool ret = false;

            return ret;
        }
    }
}