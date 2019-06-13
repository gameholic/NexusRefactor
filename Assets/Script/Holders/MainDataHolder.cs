using UnityEngine;
using System.Collections;
using GH.GameElements;

namespace GH
{
    [CreateAssetMenu(menuName ="Holders/Main Data Holder")]
    public class MainDataHolder : ScriptableObject
    {
        [SerializeField]
        private GameElements.Instance_logic _HandCardLogic;
        [SerializeField]
        private GameElements.Instance_logic _FieldCardLogic;
        [SerializeField]
        private GameObject _CardPrefab;
        [SerializeField]
        private Element elementAttack;
        [SerializeField]
        private Element elementHealth;

        public GameElements.Instance_logic HandCardLogic
        {
            get { return _HandCardLogic; ; }
        }
        public GameElements.Instance_logic FieldCardLogic
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
