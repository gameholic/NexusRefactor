using UnityEngine;
using System.Collections;
using GH.GameCard;
using GH.Multiplay;

namespace GH.GameAction
{
    [CreateAssetMenu(menuName = "Actions/Reset isJustPlaced")]
    public class ResetJustPlaced : PlayerAction
    {
        public override void Execute(PlayerHolder p)
        {
            Debug.Log("ResetCard_Current Player: " + p.player);
            MultiplayManager.singleton.PlayerResetFlatFootedCard(p.PhotonId);
        }
    }
}
