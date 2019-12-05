using GH.GameCard.CardInfo;
using GH.GameCard.CardAbility;
using GH.Player;
using UnityEngine;

namespace GH.GameCard
{
    public abstract class Card: ScriptableObject
    {
#pragma warning disable 0649
        [SerializeField]
        protected CardData _CardData;
        [SerializeField]
        protected AbilityManager _Ability;
        protected PhysicalAttribute _PhysicInstance;
        protected ConditionAttribute _ConditionAttribute;        
        protected Player.PlayerHolder _User;
#pragma warning restore 0649
        public PlayerHolder User
        {
            set { _User = value; }
            get { return _User; }
        }
        public abstract void SetCardData(CardData data);
        public abstract CardData GetCardData { get; }

        public PhysicalAttribute PhysicalCondition
        {
            get { return _PhysicInstance; }
        }
        public ConditionAttribute CardCondition
        {
            get { return _ConditionAttribute; }
        }

        public abstract void Init(GameObject go);
        public abstract bool CanUseCard();
        public abstract bool CanDropCard();
        public abstract bool UseCard(); 
       

    }

}
