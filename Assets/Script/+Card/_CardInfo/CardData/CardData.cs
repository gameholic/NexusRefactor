using UnityEngine;
using UnityEditor;
using GH.GameCard.CardAbility;
namespace GH.GameCard.CardInfo
{
    public enum CardType
    {
        Creature, Spell, Weapon
    }
    public enum Region
    {
        None, Demacia, Noxus, Ionia, Targon, Freljord
    }
    [System.Serializable]
    public class CardData              //Maybe this should be seperated. Some data is not necessary on magic card
    {
#pragma warning  disable 0649
        //Below codes should not be modifiable.        
        [SerializeField]
        private string _Name;                 // Card name
        [SerializeField]
        private string _Description;          // Short description  of card
        [SerializeField]
        private string _AbilityDescript;      // Ability description.
        [SerializeField]
        private int _ManaCost;                // ManaCost
        [SerializeField]
        private Sprite _Art;                  // Art Work
        [SerializeField]
        private Region _Region;               // Region for this card. When card design is finished, this should be changed as enum as Ability.
        //[SerializeField]
        //private Ability _Ability;             // Ability enum. Make code easy to  check  its ability.
        //[SerializeField]
        //private CardType _CardType;           // CardType
        
        private int uniqueId;               // Used to manage card in 'PlayerHolder' lists. This is saved when player save deck lists.  If card isn't in  deck,  unique id is -1;

#pragma warning restore 0649

        #region Properties
        public string Name  {get{ return _Name; }}
        public string Description { get{ return _Description; }}
        public string AbilityDescription    {get{ return _AbilityDescript; }}
        public Region Region    {get{ return _Region; }}        
        public int ManaCost { get { return _ManaCost; } }

        //public CardType CardType { get { return _CardType;} }
        public Sprite Art { get { return _Art; } }
        public int UniqueId
        {
            get { return uniqueId; }
        }
        public int SetUniqueId
        {
            set { uniqueId = value; }
        }

        #endregion


        #region TestCodes

        public int TestSetUniqueId
        {
            set { uniqueId = value; }
        }
        #endregion
    }
}