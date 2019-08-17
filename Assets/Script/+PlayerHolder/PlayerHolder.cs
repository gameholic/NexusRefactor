using GH.GameCard;
using GH.Player.Assists;

using System.Collections.Generic;

using UnityEngine;

namespace GH.Player
{
    [CreateAssetMenu(menuName ="Holders/Player Holder")]
    public class NewPlayerHolder : ScriptableObject
    {
        #region Transplanting

        public PlayerProfile _ProfileInfo;
        private PlayerInGameInfo _InGameData;
        private PlayerCardTransform _CardTransform;
        private PlayerCardManager _CardManager;
        #endregion
        public string player;
        public string userID;
        public bool isHumanPlayer;


  
        public Color playerColor;


        private List<Card> _AllCardInstances = new List<Card>();

        public void Init()
        {
            _InGameData = new PlayerInGameInfo();
            _CardTransform = new PlayerCardTransform();
            _CardManager = new PlayerCardManager();

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
        public PlayerCardManager CardManager
        {
            get { return _CardManager; }
        }
        public void SetPlayerProfile(PlayerProfile profile)
        {
            _ProfileInfo = profile;
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