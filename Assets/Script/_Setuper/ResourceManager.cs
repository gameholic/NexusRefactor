using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GH.GameCard;
using GH.GameCard.CardElement;
namespace GH
{

    //It is more likely to be called as Card manager
    [CreateAssetMenu(menuName = "Manager/Resource manager")]
    public class ResourceManager : ScriptableObject
    {

        public Card[] allCards;
        Dictionary<string, Card> cardDict = new Dictionary<string, Card>();
        private int cardInstIndex;

        public int CardIndex
        {
            set { cardInstIndex = value; }
            get { return cardInstIndex; }
        }
        public void Init()
        {
            CardIndex = -1;
            cardDict.Clear();
            for (int i = 0; i < allCards.Length; i++)
            {
                cardDict.Add(allCards[i].name, allCards[i]);
            }
        }


        public Card GetCardInstFromDeck(string id)
        {
            Card originCard = GetCard(id);
            if (originCard == null)
            {
                Debug.LogError("Card not found: "+id);
                return null;
            }
            Card newInst = Instantiate(originCard);
            CardIndex++;

            return newInst;
        }
        Card GetCard(string id)
        {
            Card result;

            if (id != null)
                cardDict.TryGetValue(id, out result);
            else
            {
                Debug.Log(id);
                result = null;
            }
            return result;
        }
    }
}

