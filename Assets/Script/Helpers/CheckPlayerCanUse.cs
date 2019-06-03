using UnityEngine;
using System.Collections;

namespace GH
{
    public class CheckPlayerCanUse
    {
        private bool result = false;
        public bool CheckPlayer(GameObject obj)
        {
            if (obj.GetComponentInParent<PositionHolder>().IsAtBottom()) // Is at Bottom = current player is at bottom position
            {
                result = true;
            }
            else
            {
                result = false;
                Debug.Log("You can't control this");

            }
            return result;
        }
    } 
}