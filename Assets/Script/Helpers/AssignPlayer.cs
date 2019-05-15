using UnityEngine;
using System.Collections;

namespace GH
{
    public class AssignPlayer : MonoBehaviour
    {
        public PlayerHolder player;
        public CardHolders cardHolder;

      
        public PlayerHolder GetPlayer()
        {

            player = cardHolder.thisPlayer;
            return player;
        }
    }
}