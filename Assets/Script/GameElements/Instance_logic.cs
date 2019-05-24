using UnityEngine;
using System.Collections;
using GH.GameCard;
namespace GH.GameElements
{
    public abstract class Instance_logic : ScriptableObject
    {
        public abstract void OnClick(CardInstance inst);
        public abstract void OnHighlight(CardInstance inst);
    }
}