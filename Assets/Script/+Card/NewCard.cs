using GH.GameCard.CardInfo;
using GH.Player;
using UnityEngine;

namespace GH.GameCard
{
    [CreateAssetMenu(menuName ="Card")]
    public abstract class NewCard : ScriptableObject
    {

        [SerializeField]
        private CardData[] _Data;
        private CardAppearance _Appearance;
        private PhysicalAttribute _PhysicInstance;
        private PlayerHolder _Owner;
        private NewPlayerHolder _User;



        public NewPlayerHolder User
        {
            set { _User = value; }
            get { return _User; }
        }

        public CardData Data
        {
            get { return _Data[0]; }
        }
        public CardAppearance Appearance
        {
            set { _Appearance = value; }
            get { return _Appearance; }
        }

        public PhysicalAttribute PhysicInstance
        {
            set { _PhysicInstance = value; }
            get { return _PhysicInstance; }
        }
        public PlayerHolder Owner
        {
            set { _Owner = value; }
            get { return _Owner; }
        }


        public abstract bool CanUseCard();
        public abstract bool CanDropCard();
        public abstract bool UseCard();
        

        public CardProperties[] properties;       

        private int _InstId;


        /// <summary>
        /// Card Inst Id is unique id.
        /// When Card Instance is used as copy of original card, Inst id let code to find instsance 
        /// </summary>
        public int InstId
        {
            set { _InstId = value; }
            get { return _InstId; }
        }
        public CardProperties GetProperties(Element e )
        {
            for(int i =0;i<properties.Length;i++)
            {
                if (properties[i].element == e)
                    return properties[i];
            }
            return null;
        }
    }

}
