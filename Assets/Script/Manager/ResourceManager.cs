using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GH.GameCard
{

    //It is more likely to be called as Card manager
    [CreateAssetMenu(menuName = "Manager/Resource manager")]
    public class ResourceManager : ScriptableObject
    {
        public Element typeElement;
        public Card[] allCards;
        Dictionary<string, Card> cardDict = new Dictionary<string, Card>();

        public void Init()
        {
            cardDict.Clear();
            for (int i = 0; i < allCards.Length; i++)
            {
                cardDict.Add(allCards[i].name, allCards[i]);
            }
        }


        public Card GetCardFromDict(string id)
        {
            Card originCard = GetCard(id);
            if (originCard == null)
                return null;

            Card newInst = Instantiate(originCard);
            newInst.name = originCard.name;
            return newInst;
        }
        Card GetCard(string id)
        {
            Card result = null;
            cardDict.TryGetValue(id, out result);
            return result;

        }
    }
}

