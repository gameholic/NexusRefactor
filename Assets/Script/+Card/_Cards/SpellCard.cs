using UnityEngine;
using GH.GameCard.ErrorCheck;
using UnityEditor;
using GH.GameCard.CardInfo;

namespace GH.GameCard
{
    public enum SpellType
    {
        Attack, Buff, Debuff
    }
    [CreateAssetMenu(menuName = "Card/Spell Card")]
    public class SpellCard : Card
    {
#pragma warning  disable 0649
        #region Serialized
        private ErrorCheck_Spell errorCheck = new ErrorCheck_Spell();
        [SerializeField]
        private SpellType type;
        [SerializeField]
        private int healthChange;
        [SerializeField]
        private int attackChange;
        [SerializeField]
        private SpellData _UniqueData;
        //[SerializeField]
        //private CardData _CardData;
        #endregion
#pragma warning restore 0649

        public int _HealthChange { get { return healthChange; } }   
        public int _AttackChange { get { return attackChange; } }
        public SpellType GetType { get { return type; } }
        public override void Init(GameObject go)
        {

        }

        public override void SetCardData(CardData data)
        {
            _CardData = data;
        }

        public SpellData SpellData
        {
            set { _UniqueData = value; }
            get { return _UniqueData; }
        }
        public override CardData GetCardData
        {
            get
            {
                return _CardData;
            }
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