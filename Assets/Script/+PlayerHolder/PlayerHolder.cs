using GH.GameCard;
using GH.Player.Assists;

using System.Collections.Generic;

using UnityEngine;

namespace GH.Player
{
    [CreateAssetMenu(menuName ="Holders/Player Holder")]
    public class PlayerHolder : ScriptableObject
    {
        #region Transplanting

        private PlayerProfile _ProfileInfo;
        private PlayerInGameInfo _InGameData;
        private PlayerCardTransform _CardTransform;
        private CardManager _CardManager;
        #endregion
        public bool isHumanPlayer;


  
        public Color playerColor;


        private List<Card> _AllCardInstances = new List<Card>();

        public void Init()
        {
            _InGameData = new PlayerInGameInfo();
            _CardTransform = new PlayerCardTransform();
            _CardManager = new CardManager();

            _InGameData.Init(this);
            _CardManager.Init(this);
            _CardManager.Init(this);
        }
        public PlayerInGameInfo InGameData
        {
            get { return _InGameData; }
        }
        public PlayerCardTransform CardTransform
        {
            get { return _CardTransform; }
        }
        public CardManager CardManager
        {
            get { return _CardManager; }
        }
        public PlayerProfile PlayerProfile
        {
            set { _ProfileInfo = value; }
            get { return _ProfileInfo; }
        }
        public List<Card> AllCardInst
        {
            get { return _AllCardInstances; }
        }
        public void AddCardToAllCardInst(Card c)
        {
            _AllCardInstances.Add(c);
        }
     



    }
} 