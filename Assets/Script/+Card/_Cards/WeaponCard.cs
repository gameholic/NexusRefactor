using UnityEngine;
using UnityEditor;
using GH.GameCard.CardInfo;

namespace GH.GameCard
{
    [CreateAssetMenu(menuName = "Card/Weapon Card")]
    public class WeaponCard : Card
    {
#pragma warning  disable 0649
        [SerializeField]
        private WeaponData _UniqueData;
        //[SerializeField]
        //private CardData _CardData;
#pragma warning restore 0649

        public override void SetCardData(CardData data)
        {
            _CardData = data;
        }
        public WeaponData WeaponData
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
            ///Errorchecking needed
            ///However Errorchecking in Card seems very dirty.
            ///Considering deleting All  these functions and check error upper layer
            return true;
        }

        public override bool CanUseCard()
        {
            return true;
        }

        public override void Init(GameObject go)
        {
            _PhysicInstance = go.GetComponent<PhysicalAttribute>();
            _ConditionAttribute = new ConditionAttribute();
            CardCondition.OriginCard = this;
            PhysicalCondition.OriginCard = this;
        }


        public override bool UseCard()
        {
            return true;
        }
    }
}