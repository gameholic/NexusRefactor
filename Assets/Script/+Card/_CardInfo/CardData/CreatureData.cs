using UnityEngine;
using UnityEditor;

namespace GH.GameCard.CardInfo
{
    [System.Serializable]
    public  class CreatureData
    {
        [SerializeField]
        private int _Attack;                  // Attack value
        [SerializeField]
        private int _Defend;                  // Defend Value
        [SerializeField]
        private string _Class;               // Class of the card.  Need to be developed in future

        public string Class { get { return _Class; } }
        public int Attack { get { return _Attack; } }
        /// <summary>
        /// Modify Health by adding value to current Attack
        /// </summary>
        
        public int ModifyAttack { set { _Attack = +value; } }
        public int Defend { get { return _Defend; } }
        /// <summary>
        /// Modify Health by adding value to current health
        /// </summary>
        public int ModifyHealth { set { _Defend = +value; } }

    }

}
