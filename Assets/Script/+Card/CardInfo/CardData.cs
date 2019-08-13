using UnityEngine;
using UnityEditor;
using GH.GameCard.CardAbility;
namespace GH.GameCard.CardInfo
{
    [System.Serializable]
    public class CardData
    {

        //Below codes should not be modifiable.        
        [SerializeField]
        private string _Name;                 // Card name
        [SerializeField]
        private string _Description;          // Short description  of card
        [SerializeField]
        private string _AbilityDescript;      // Ability description.
        [SerializeField]
        private string _Region;               // Region for this card. When card design is finished, this should be changed as enum as Ability.
        [SerializeField]
        private string _Class;               // Class of the card.  Need to be developed in future
        [SerializeField]
        private Ability _Ability;             // Ability enum. Make code easy to  check  its ability.

        [SerializeField]
        private int _Mana;                    // ManaCost
        [SerializeField]
        private int _Attack;                  // Attack value
        [SerializeField]
        private int _Defend;                  // Defend Value
        [SerializeField]
        private CardType _CardType;           // CardType
        [SerializeField]
        private Sprite _Art;                  // Art Work

        private int uniqueId;               // Used to manage card in 'PlayerHolder' lists. This is saved when player save deck lists.  If card isn't in  deck,  unique id is -1;


        #region Properties
        public string Name  {get{ return _Name; }}
        public string Description { get{ return _Description; }}
        public string AbilityDescription    {get{ return _AbilityDescript; }}
        public string Region    {get{ return _Region; }}
        public string Class{get{ return _Class; }}
        public int Mana { get { return _Mana; } }
        public int Attack { get { return _Attack; } }
        public int Defend { get { return _Defend; } }
        public CardType CardType { get { return _CardType;} }
        public int UniqueId { get { return uniqueId; } }
       






        #endregion
    }
}