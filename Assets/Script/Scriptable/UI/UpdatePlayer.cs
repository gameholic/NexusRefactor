using GH.Player;
using UnityEngine;
namespace GH.UI
{
    public class UpdatePlayer : MonoBehaviour
    {
        public TextMesh targetText;
       
        public void UpdatePlayerText(PlayerHolder p)
        {
            targetText.text = p.PlayerProfile.UniqueId;  
        }
    }
}
