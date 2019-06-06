using UnityEngine;
using UnityEditor;
using System.Collections;


namespace GH.Multiplay
{
    public class NetworkPrint : Photon.MonoBehaviour
    {
        public int photonId;
        private bool isLocal;
        string[] cardIds;

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
            Debug.Log(cardIds);
            //MultiplayManager.singleton.Players = this;
            MultiplayManager.singleton.AddPlayer(this);
        }
    }

}