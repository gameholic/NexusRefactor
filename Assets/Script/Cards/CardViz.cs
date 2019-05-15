using UnityEngine;
using System.Collections;
using UnityEngine.UI;
namespace GH
{ 
    public class CardViz : MonoBehaviour
    {         
        public Card card;
        public CardVizProperties[] properties;
        public GameObject statsHolder;
        [System.NonSerialized]
        public GameObject weaponHolder; 

        
        //private void Start()
        //{
        //    LoadCard(card);
        //}


        public void LoadCard(Card c)
        {
            if (c == null)
                return;
            card = c;

            c.cardType.OnSetType(this);

            CloseAll();


            for(int i=0; i<c.properties.Length; i++)
            {
                CardProperties cp = c.properties[i];
                CardVizProperties p = GetProperty(cp.element);

                if (p == null)
                    continue;
                if(cp.element is ElementInt)
                {
                    p.text.text = cp.intValue.ToString();
                    p.text.gameObject.SetActive(true);
                }
                else if(cp.element is ElementText)
                {
                    p.text.text = cp.stringValue;
                    p.text.gameObject.SetActive(true);
                }
                else if(cp.element is ElementImage)
                {
                    p.renderer.sprite = cp.sprite;
                    p.renderer.gameObject.SetActive(true);
                }
            }

        }

        public void CloseAll()
        {
            foreach(CardVizProperties p in properties)
            {
                if(p.renderer != null)
                    p.renderer.gameObject.SetActive(false);
                if (p.text != null)
                    p.text.gameObject.SetActive(false);
 
            }
        }
        public CardVizProperties GetProperty(Element e)
        {
            CardVizProperties result = null;

            for(int i=0; i<properties.Length; i++)
            {
                if(properties[i].element == e)
                {
                    result = properties[i];
                    break;
                }
            }


            return result;
        }
    }
}