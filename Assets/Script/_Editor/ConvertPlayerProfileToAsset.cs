using UnityEngine;
using GH.Player;


namespace GH.AssetEditor
{
    [CreateAssetMenu(menuName = ("PlayerProfileAsset"))]
    public class ConvertPlayerProfileToAsset : ScriptableObject
    {
        public PlayerProfile playerProfile;

    }

}