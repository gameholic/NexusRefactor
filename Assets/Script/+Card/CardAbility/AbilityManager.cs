using System;
using UnityEngine;

namespace GH.GameCard.CardAbility
{
    public enum AbilityCondition { ToArena, DeathDefied}
    [Serializable]
    public class AbilityManager
    {
        private enum AbilityType { None, Buff, Nerf, Damage, Heal, DemonShade}
#pragma warning disable 0649
        [SerializeField]
        private int _IncreaseAtk;
        [SerializeField]
        private int _IncreaseDef;
        [SerializeField]
        private AbilityType _abilityType;
        //[SerializeField]
        //private

        public void Buff()
        {

        }
        public void Nerf()
        {

        }

        public void Damage()
        {

        }

        public void Heal()
        {

        }
#pragma warning restore 0649
    }
}
