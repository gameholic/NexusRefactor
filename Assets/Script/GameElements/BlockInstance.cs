using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GH.GameCard;

namespace GH
{
    /// <summary>
    /// Block instance is consist with attacker and defender list.
    /// It's designed for 1 attacker can be blocked by multiple defenders
    /// </summary>
    public class BlockInstance
    {
        public Card attacker;
        public List<Card> defenders = new List<Card>();
    }

}