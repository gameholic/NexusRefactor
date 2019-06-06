using UnityEngine;
using System.Collections;


namespace GH.GameElements
{

    public class Area : MonoBehaviour
    {
        private bool _IsPlaced = false;
        [SerializeField]
        private AreaLogic _logic;
        public bool IsPlaced
        {
            set { _IsPlaced = value; }
            get { return _IsPlaced; }
        }
        public void OnDrop(Area a)
        {
            //if (isPlaced == false)
                _logic.Execute(a);
        }

    }

}