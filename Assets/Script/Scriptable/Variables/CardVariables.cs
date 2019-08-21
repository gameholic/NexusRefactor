using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GH.GameCard;

namespace GH
{
    [CreateAssetMenu(menuName ="Variables/Card Variable")]
    public class CardVariables : ScriptableObject
    {
        public Card value;

        public void Set(Card v)
        {
            value = v;
        }
    }
}