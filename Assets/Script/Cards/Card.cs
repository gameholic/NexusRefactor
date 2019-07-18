using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GH.GameCard
{
    [CreateAssetMenu(menuName ="Card")]
    public class Card : ScriptableObject
    {
        [Tooltip("This should be same as CardMana_Int Property")]
        public int cardCost;
        public CardType cardType;
        public CardProperties[] properties;
        
        private CardInstance _CardInstance;
        private int _InstId;
        private CardViz _Viz;
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
}
