using UnityEngine;
using System.Collections;
using GH.GameCard.CardState;

namespace GH
{
    [CreateAssetMenu(menuName ="Holders/Main Data Holder")]
    public class MainDataHolder : ScriptableObject
    {
        [SerializeField]
        private CardStateLogic _HandCardLogic;
        [SerializeField]
        private CardStateLogic _FieldCardLogic;
        [SerializeField]
        private GameObject _CardPrefab;
        [SerializeField]
        private Element elementAttack;
        [SerializeField]
        private Element elementHealth;

        public CardStateLogic HandCardLogic
        {
            get { return _HandCardLogic; ; }
        }
        public CardStateLogic FieldCardLogic
        {
            get { return _FieldCardLogic; ; }
        }
        public GameObject CardPrefab
        {
            get { return _CardPrefab; }
        }
        public Element AttackElement
        { get { return elementAttack; } }
        public Element HealthElement
        {
            get { return elementHealth; }
        }


    }

}
