using UnityEngine;
using UnityEditor;

namespace GH
{
    public class PositionHolder : MonoBehaviour
    {
        [SerializeField]
        private bool _IsBottom;

        public bool IsAtBottom
        {
            get { return _IsBottom; }
        }
    }

}   