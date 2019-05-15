using UnityEngine;
using System.Collections;

namespace GH
{
    public class CheckPlayerCanUse
    {
        bool result = false;
        public bool CheckPlayer(GameObject obj)
        {
            //PlayerHolder playerOfObj;
            //if (obj.GetComponentInParent<AssignPlayer>() == null)
            //{
            //    Debug.Log("this obj hasn't assigned player");
            //    Debug.Log("Obj: " + obj);
            //}
            //playerOfObj = obj.GetComponentInParent<AssignPlayer>().GetPlayer();


            //if (p == playerOfObj)
            //    result = true;
            //else
            //    result = false;
            //return result;



            if (obj.GetComponentInParent<PositionHolder>().IsAtBottom()) // Is at Bottom = current player is at bottom position
            {
                result = true;
            }
            else
                result = false;
            return result;
        }
    } 
}