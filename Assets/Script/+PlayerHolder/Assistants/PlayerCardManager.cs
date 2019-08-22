using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using GH.Player.Assists;
using GH.GameCard;

namespace GH.Player
{
    public enum CardContainer
    {
        Hand,Field,Attack,Grave,All
    }

    public class CardManager : PlayerAssists
    {

        [System.NonSerialized]
        public List<int> handCards = new List<int>();
        [System.NonSerialized]
        public List<int> fieldCards = new List<int>();
        [System.NonSerialized]
        public List<int> attackingCards = new List<int>();
        [System.NonSerialized]
        public List<int> deadCards = new List<int>();
        private Dictionary<int, Card> allCards = new Dictionary<int, Card>();







        public override void Init(PlayerHolder p)
        {
            player = p;
        }
        /// <summary>
        /// this 
        /// </summary>
        /// <param name="c"></param>
        public void AddCardInDeck(Card c)
        {
            int id = c.Data.UniqueId;
          
        }
        public void InitAllCards(Card[] deck)
        {
            for (int i = 0; i < deck.Length; i++)
            {
                allCards.Add(i, deck[i]);
                deck[i].Data.UniqueId = i;
            }
        }
        public void DropCardOnField(CreatureCard c)
        {
            int id = c.Data.UniqueId;

            if (handCards.Contains(id))
            {
                handCards.Remove(id);
                fieldCards.Add(id);
            }
            else
                Debug.LogErrorFormat("CantDropCard: Player Dont have {0} on hand",c.Data.Name);

        }
        public bool CheckCardContainer(CardContainer position, Card c)
        {
            bool v = false;
            List<int> tmp = null;
            tmp = CheckCardContainer(position);
            if(tmp.Contains(c.Data.UniqueId))
            {
                v = true;
            }
            return v;
        }

        public List<int> CheckCardContainer(CardContainer container)
        {
            List<int> tmp = null;
            switch (container)
            {
                case CardContainer.Hand:
                    tmp = handCards;
                    break;
                case CardContainer.Field:
                    tmp = fieldCards;
                    break;
                case CardContainer.Attack:
                    tmp = attackingCards;
                    break;
                case CardContainer.Grave:
                    tmp = deadCards;
                    break;
                default:
                    Debug.LogError("CantFindCardIn CardContainer");
                    break;
            }
            return tmp;
        }
        public Card FindCardIn(CardContainer cc, Card card)
        {
            Card value = null;
            List<int> tmp = CheckCardContainer(cc);
        
            if (tmp!=null)
            {
                value = FindCardIn(tmp, card);
            }
            return value;
        }
        private Card FindCardIn(List<int> list, Card card)
        {
            Card v = null;
            int id = card.Data.UniqueId;
            if(list.Contains(id))
            {
                v = SearchCard(id);
            }
            else
            {
                Debug.LogWarningFormat("CannotFindCardInList");
            }

            return v;
        }
        public Card SearchCard(int id)
        {
            Card v = null;
            allCards.TryGetValue(id, out v);
            return v;
        }
        public bool CheckCard(int id)
        {
            bool ret = false;
            Card v = null;
            allCards.TryGetValue(id, out v);
            if (v != null)
                ret = true;
            return ret;
        }

    }
}