using UnityEngine;
using System.Collections;
using GH.GameCard.CardElement;
namespace GH
{
    [CreateAssetMenu(menuName ="Holders/Main Data Holder")]
    public class MainDataHolder : ScriptableObject
    {
        #pragma warning disable 0649
        [SerializeField]
        private GameObject _CardPrefab;
        [SerializeField]
        private Element elementAttack;
        [SerializeField]
        private Element elementHealth;
        #pragma warning restore 0649


        public GameObject CardPrefab
        {
            get { return _CardPrefab; }
        }
        public Element AttackElement
        {
            get { return elementAttack; }
        }
        public Element HealthElement
        {
            get { return elementHealth; }
        }


    }

}
