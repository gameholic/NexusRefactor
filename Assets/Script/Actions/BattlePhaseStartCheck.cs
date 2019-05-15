using System.Collections;
using UnityEngine;
using GH.GameStates;

namespace GH
{
    [CreateAssetMenu(menuName = "Actions/Condition/BattleAvailable")]
    public class BattlePhaseStartCheck : Condition
    {
        public bool available;
        public override bool IsValid()
        {
            GameController controller = GameController.singleton;
            if (controller.currentPlayer.fieldCard.Count > 0 && available)
            {
                return true;
            }
            else
            {
                Setting.RegisterLog("Cant start battle phase", Color.black);
                return false;
            }
        }
    }

}