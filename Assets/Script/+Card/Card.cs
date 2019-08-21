using GH.GameCard.CardInfo;
using GH.Player;
using UnityEngine;

namespace GH.GameCard
{
    [CreateAssetMenu(menuName ="+Card")]
    public abstract class Card : ScriptableObject
    {
#pragma warning disable 0649
        [SerializeField]
        private CardData[] _Data;
        protected PhysicalAttribute _PhysicInstance;
        protected ConditionAttribute _ConditionAttribute;
        protected Player.PlayerHolder _User;

#pragma warning restore 0649


        public PlayerHolder User
        {
            set { _User = value; }
            get { return _User; }
        }
        public CardData Data
        {
            get { return _Data[0]; }
        }
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
