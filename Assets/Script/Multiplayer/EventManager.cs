using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GH.GameCard;

namespace GH.Multiplay
{
    public class EventManager : MonoBehaviour
    {
        #region My Calls
        /// <summary>
        ///  
        /// </summary>
        /// <param name="instId">Which card is dropped</param>
        /// <param name="ownerId">Who dropped the card</param>
        public void CardIsDroppedDown(int instId, int ownerId)
        {
            Card c = NetworkManager.singleton.GetCard(instId, ownerId);
        }

        public void CardIsPickedupFromDeck(int instId, int ownerId)
        {
            Card c = NetworkManager.singleton.GetCard(instId, ownerId);


        }
        #endregion

    }

}
