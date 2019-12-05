#pragma warning disable 0108


using UnityEngine;
using System.Collections;

using GH.GameCard.CardElement;

namespace GH.GameCard.CardInfo
{
    public class CardAppearance : MonoBehaviour
    {
        #region SerializeField

        public CardAppearPropoerty[] property;
        private Card card;
        [SerializeField]
        private GameObject _StatsHolder;

        #endregion
        public Card Card { get { return card; } }
        #region Properties


        #endregion


        public void LoadCard(Card c, GameObject go)
        { 
            if (c == null)
                return;
            c.Init(go);
            card = c;
            DisableCard();
            for (int i = 0; i < property.Length; i++)
            {
                CardAppearPropoerty p = property[i];
                if (p == null)
                    continue;
                if (c is CreatureCard)
                {
                    CreatureCard tmp = (CreatureCard)c;

                    ApplyText(p, tmp.GetCardData);
                    ApplyCreature(p, tmp.CreatureData);//Apply Creature elements

                }
                else if (c is WeaponCard)
                {
                    WeaponCard tmp = (WeaponCard)c;
                    WeaponData wData = tmp.WeaponData;

                }
           //Apply weapon  elements
                else if (c is SpellCard)
                {
                    SpellCard tmp = (SpellCard)c;
                    SpellData sData = tmp.SpellData;
                }

            }
        }
        private void ApplyCreature(CardAppearPropoerty p, CreatureData data )
        {
            ElementType e = p.element.type;
            switch (e)
            {
                case ElementType.Attack:
                    p.text.text = data.Attack.ToString();
                    p.text.gameObject.SetActive(true);
                    break;
                case ElementType.Defend:
                    p.text.text = data.Defend.ToString();
                    p.text.gameObject.SetActive(true);
                    break;
                case ElementType.Class:
                    p.text.text = data.Class;
                    p.text.gameObject.SetActive(true);
                    break;
                case ElementType.CardType:
                    p.text.text = "Creature";
                    p.text.gameObject.SetActive(true);
                    break;

            }
        }
        private void ApplyText(CardAppearPropoerty p, CardData data)
        {
            ElementType e = p.element.type;
            switch (e)
            {
                case ElementType.Art:
                    p.renderer.sprite = data.Art;
                    p.renderer.gameObject.SetActive(true);
                    break;
                case ElementType.Name:
                    p.text.text = data.Name;
                    p.text.gameObject.SetActive(true);
                    break;
                case ElementType.Descript:
                    p.text.text = data.Description;
                    p.text.gameObject.SetActive(true);
                    break;
                case ElementType.Ability:
                    p.text.text = data.AbilityDescription;
                    p.text.gameObject.SetActive(true);
                    break;
                case ElementType.Mana:
                    p.text.text = data.ManaCost.ToString();
                    p.text.gameObject.SetActive(true);
                    break;                    
                case ElementType.Region:
                    p.text.text = data.Region.ToString();
                    p.text.gameObject.SetActive(true);
                    break;
                default:
                    break;
            }
            if (e != ElementType.Art)
                p.text.gameObject.SetActive(true);
        }
        public void DisableCard()
        {
            foreach (CardAppearPropoerty p in property)
            {
                if (p.renderer != null)
                    p.renderer.gameObject.SetActive(false);
                if (p.text != null)
                    p.text.gameObject.SetActive(false);
            }
        }
        /// <summary>
        /// Find VizProperty that is using same element with Card Property
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public CardAppearPropoerty GetProperty(Element e)
        {
            CardAppearPropoerty result = null;

            for (int i = 0; i < property.Length; i++)
            {
                if (property[i].element == e)
                {
                    result = property[i];
                    break;
                }
            }
            return result;
        }

        public GameObject StatsHolder  //Disable for spell and weapon card
        {
            get { return _StatsHolder; }
        }
    }


}
#pragma warning restore CS0108