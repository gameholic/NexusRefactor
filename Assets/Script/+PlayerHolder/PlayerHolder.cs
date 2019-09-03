using GH.GameCard;
using GH.Player.Assists;

using System.Collections.Generic;

using UnityEngine;

namespace GH.Player
{
    [CreateAssetMenu(menuName ="Holders/Player Holder")]
    public class PlayerHolder : ScriptableObject
    {
        [SerializeField]            //To Check The Data. Needs To Be Deleted
        private PlayerProfile _ProfileInfo;

        [SerializeField]
        private PlayerInGameInfo _InGameData;
        [SerializeField]
        private PlayerCardTransform _CardTransform;
        [SerializeField]                //Needs To Be Deleted
        private PlayerCardManager _CardManager;


        public void Init()
        {
            Debug.Log("InitializePlayerHolder");
            _InGameData.Init(this);
            _CardManager.Init(this);
            //SetPlayerProfile(Multiplay.MultiplayManager.singleton.GetPlayer(_InGameData.PhotonId).PlayerProfile);
        }
        public Color playerColor;


        private List<Card> _AllCardInstances = new List<Card>();

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
        public PlayerProfile PlayerProfile
        {
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
     
        public void SetPlayerProfile(PlayerProfile p)
        {
            _ProfileInfo = p;
        }


    }
} 