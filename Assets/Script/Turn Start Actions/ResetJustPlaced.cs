﻿using UnityEngine;
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
            MultiplayManager.singleton.PlayerResetFlatFootedCard(p.PhotonId);
        }
    }
}