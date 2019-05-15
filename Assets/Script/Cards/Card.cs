using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GH
{
    [CreateAssetMenu(menuName ="Card")]
    public class Card : ScriptableObject
    {
      
        public CardType cardType;
        public CardProperties[] properties;

        public int cardCost;
        public bool canAttack;

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
