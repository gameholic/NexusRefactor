using UnityEngine;
using UnityEditor;


namespace GH.AssetEditor
{
    [CreateAssetMenu(menuName = ("PlayerProfileAsset"))]
    public class ConvertPlayerProfileToAsset : ScriptableObject
    {
        public PlayerProfile playerProfile;

    }

}