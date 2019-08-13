#pragma warning disable CS0108


using UnityEngine;
using System.Collections;

using GH.GameCard.CardElement;

namespace GH.GameCard.CardInfo
{
    public class CardAppearance : MonoBehaviour
    {
        public Card originCard;        //Do I need this? If I make CardAppearance only can access through Card, this won't be needed

        #region SerializeField

        public CardAppearPropoerty[] property;


        #endregion

        #region GetSerializedVariable

        public string GetCardName
        {
            get { return name; }
        }
        #endregion
        public void LoadCard(Card c)
        {
            if (c == null)
                return;
            originCard = c;
            c.Appearance = this;
            CardData data = c.Data[0];
            for (int i = 0; i < property.Length; i++)
            {
                CardAppearPropoerty p = property[i];

                if (p == null)
                    continue;
                ApplyText(p, data);

            }
        }

        public void ApplyText(CardAppearPropoerty p, CardData data)
        {
            ElementType e = new ElementType();

            e = p.element.type;
            switch (e)
            {
                case ElementType.Art:
                    p.renderer.sprite = data.art;
                    p.renderer.gameObject.SetActive(true);
                    break;
                case ElementType.Name:
                    p.text.text = data.name;
                    break;
                case ElementType.Descript:
                    p.text.text = data.description;
                    break;
                case ElementType.Ability:
                    p.text.text = data.abilityDescript;
                    break;
                case ElementType.Mana:
                    p.text.text = data.mana.ToString();
                    break;
                case ElementType.Attack:
                    p.text.text = data.attack.ToString();
                    break;
                case ElementType.Defend:
                    p.text.text = data.defend.ToString();
                    break;
                case ElementType.CardType:
                    p.text.text = data.cardType.ToString();
                    break;
                case ElementType.Region:
                    p.text.text = data.region;
                    break;
                case ElementType.Class:
                    p.text.text = data._class;
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
    }


}
#pragma warning restore CS0108