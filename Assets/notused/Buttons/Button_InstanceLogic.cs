using UnityEngine;
using UnityEditor;

namespace GH.GameElements
{
    public abstract class Button_InstanceLogic : ScriptableObject
    {
        public abstract void OnClick(GameObject buttonObj);
        public abstract void OnHighlight(GameObject buttonObj);
    }
}