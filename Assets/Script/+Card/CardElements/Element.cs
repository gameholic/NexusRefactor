using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GH.GameCard.CardElement
{
    public enum ElementType
    {
        Art, Name, Descript, Ability, Mana, Attack, Defend, CardType, Region, Class
    }
    public abstract class Element : ScriptableObject
    {
        public ElementType type;
    }    
}