using UnityEngine;
using UnityEditor;

namespace GH
{
    public class PositionHolder : MonoBehaviour
    {
        public bool isBottom;

        public bool IsAtBottom()
        {
            return isBottom;
        }
    }

}   