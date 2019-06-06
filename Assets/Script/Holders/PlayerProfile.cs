using UnityEngine;
using UnityEditor;

namespace GH
{
    [CreateAssetMenu(menuName ="Player Profile")]
    public class PlayerProfile : ScriptableObject
    {
        [SerializeField]
        private string[] _CardId;

        public string[] GetCardIds()
        {
            return _CardId;
        }

    }
}