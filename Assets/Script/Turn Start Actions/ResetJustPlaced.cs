using GH.Multiplay;
using GH.Player;
using UnityEngine;

namespace GH.GameAction
{
    [CreateAssetMenu(menuName = "Actions/Reset isJustPlaced")]
    public class ResetJustPlaced : PlayerAction
    {
        public override void Execute(PlayerHolder p)
        {
            MultiplayManager.singleton.PlayerResetFlatFootedCard(p.InGameData.PhotonId);
        }
    }
}
