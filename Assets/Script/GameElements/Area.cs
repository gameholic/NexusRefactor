using UnityEngine;


namespace GH.GameElements
{

    public class Area : MonoBehaviour
    {
#pragma warning  disable 0649
        private bool _IsPlaced = false;
        public bool IsPlaced
        {
            set { _IsPlaced = value; }
            get { return _IsPlaced; }
        }
#pragma warning restore 0649
    }

}