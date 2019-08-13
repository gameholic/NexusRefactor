using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GH.GameCard.CardInfo;

namespace GH.GameCard
{
    [CreateAssetMenu(menuName ="Card")]
    public abstract class Card : ScriptableObject
    {

        #region Transplanting


        [SerializeField]
        private CardData[] _Data;
        private CardAppearance _Appearance;
        private PhysicalAttribute _PhysicInstance;
        private PlayerHolder _Owner;


        public CardData[] Data
        {
            get { return _Data; }
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
        #endregion

        
        [Tooltip("This should be same as CardMana_Int Property")]
        [SerializeField]
        private int _CardCost;
        public CardType cardType;
        public CardProperties[] properties;
        
        private CardInstance _CardInstance;
        private int _InstId;
        private CardViz _Viz;

        public int CardCost
        {
            get { return _CardCost; }
        }
        public CardViz Viz
        {
            set { _Viz = value; }
            get { return _Viz; }
        }
        public CardInstance Instance
        {
            set { _CardInstance = value; }
            get { return _CardInstance; }
        }
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
    public class RuntimeValues
    {
        private int instId;
    }
}
