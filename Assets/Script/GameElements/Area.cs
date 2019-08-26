using UnityEngine;


namespace GH.GameElements
{

    public class Area : MonoBehaviour
    {
#pragma warning  disable 0649
        private bool _IsPlaced = false;
        public bool IsPlaced
        {
            get { return _IsPlaced; }
        }

        public void SetIsPlaced(bool a)
        {
            _IsPlaced = a;
        }
#pragma warning restore 0649
    }

}