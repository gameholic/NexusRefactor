using UnityEngine;
using System.Collections;


namespace GH.GameElements
{

    public class Area : MonoBehaviour
    {
        //public bool isPlaced = false;
        [SerializeField]
        private AreaLogic _logic;

        public void OnDrop(Area a)
        {
            //if (isPlaced == false)
                _logic.Execute(a);
        }

    }

}