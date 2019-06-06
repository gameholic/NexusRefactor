using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GH.GameCard
{
    [CreateAssetMenu(menuName ="Card")]
    public class Card : ScriptableObject
    {
        
        public CardType cardType;
        public CardProperties[] properties;

        public int cardCost;
        public bool canAttack;

        private int _InstId;
        private CardViz _Viz;
        public CardViz Viz
        {
            set { _Viz = value; }
            get { return _Viz; }
        }

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
