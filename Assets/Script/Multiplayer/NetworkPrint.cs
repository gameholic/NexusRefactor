using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using GH.GameCard;

namespace GH.Multiplay
{
    public class NetworkPrint : Photon.MonoBehaviour
    {
        public int photonId;
        private bool isLocal;
        string[] cardIds;
        private Dictionary<int, Card> _MyCards = new Dictionary<int, Card>();

        public void AddCard(Card c)
        {
            _MyCards.Add(c.InstId, c);
        }
        public bool IsLocal
        {
            set { isLocal = value; }
            get { return isLocal; }
        }

        public string[] GetStartingCardids()
        {
            return cardIds;
        }

       

        void OnPhotonInstantiate(PhotonMessageInfo info)
        {            
            photonId = photonView.ownerId;
            isLocal = photonView.isMine;
            object[] data = photonView.instantiationData;
            cardIds = (string[])data[0];
            MultiplayManager.singleton.AddPlayer(this);
        }
    }

}