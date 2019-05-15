using UnityEngine;
using System.Collections;


namespace GH.GameElements
{

    public class Area : MonoBehaviour
    {
        //public bool isPlaced = false;
        public AreaLogic logic;

        public void OnDrop(Area a)
        {
            //if (isPlaced == false)
                logic.Execute(a);
        }

        //public void OnDrop()
        //{
        //    logic.Execute();
        //}
    }

}