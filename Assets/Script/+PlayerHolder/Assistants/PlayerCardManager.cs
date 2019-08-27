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

    [CreateAssetMenu(menuName = "PlayerData/PlayerCardManager")]
    public class PlayerCardManager : PlayerAssists
    {
        //[System.NonSerialized]
        public List<int> handCards;
        //[System.NonSerialized]
        public List<int> fieldCards;
        //[System.NonSerialized]
        public List<int> attackingCards;
        //[System.NonSerialized]
        public List<int> deadCards;
        //[System.NonSerialized]
        public Dictionary<int, Card> allCards;







        public override void Init(PlayerHolder p)
        {
            player = p;
            InitAllCards(p.PlayerProfile._DeckToPlay.Cards);
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
            allCards = new Dictionary<int, Card>();
            for (int i = 0; i < deck.Length; i++)
            {
                allCards.Add(i, deck[i]);
                deck[i].Data.SetUniqueId = i;
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

        /// <summary>
        /// Check Card Existance In List Of CardContainer.
        /// </summary>
        /// <param name="position">Enum value for the List</param>
        /// <param name="c">Target</param>
        /// <returns></returns>
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

        /// <summary>
        /// Search Card From AllCards List of Player
        /// If Return value is null, It means there is no card in deck
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Card SearchCard(int id)
        {
            Card v = null;
            allCards.TryGetValue(id, out v);
            if (v == null)
                Debug.LogError("Can'tFindTheCard");
      
            return v;
        }

    }
}