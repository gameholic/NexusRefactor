using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GH.GameElements;

namespace GH
{
    [CreateAssetMenu(menuName ="Holders/Player Holder")]
    public class PlayerHolder : ScriptableObject
    {
        public string player;
        public string userID;
        public bool isBottomPos;
        public bool isHumanPlayer;

        //public string[] startingCards;
  
        public Color playerColor;
        public Sprite playerAvatar;
        public PlayerStatsUI statsUI;
        public Instance_logic handLogic;
        public Instance_logic fieldLogic;

        public List<string> startingDeck = new List<string>();

        [System.NonSerialized]
        public List<CardInstance> handCards = new List<CardInstance>();
        [System.NonSerialized]
        public List<CardInstance> fieldCard = new List<CardInstance>();
        [System.NonSerialized]
        public List<CardInstance> attackingCards = new List<CardInstance>();
        [System.NonSerialized]
        public List<string> allCards = new List<string>();
        [System.NonSerialized]
        public ManaManager manaResourceManager = new ManaManager();
        [System.NonSerialized]
        public CardHolders currentCardHolder;
        [System.NonSerialized]
        public int health;


        public void Init()
        {
            health = 20;
            //Debug.Log("check");
            allCards.AddRange(startingDeck); 
        }
       

        public void DropCardOnField(CardInstance inst, bool registerEvent =true)
        {
            if (handCards.Contains(inst))
                handCards.Remove(inst);

            fieldCard.Add(inst);
            if(registerEvent)
                Setting.RegisterLog(userID + " used " + inst.viz.card.name, playerColor);
        }

        public bool PayMana(Card c)
        {
            bool result = false;

            int currentMana = manaResourceManager.GetCurrentMana();
            if (c.cardCost <= currentMana)
                result = true;
            else
                Setting.RegisterLog("Not Enough Mana", Color.black);

            return result;
        }

        public void DoDamage(int v)
        {
            health -= v;
            if(statsUI !=null)
            {
                statsUI.UpdateHealth();
            }
        }

        public void LoadPlayerOnStatsUI()
        {
            if(statsUI != null)
            {
                statsUI.player = this;
                statsUI.UpdateAll();
            }
        }


    }


} 