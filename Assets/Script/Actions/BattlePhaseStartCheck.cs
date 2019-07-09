using System.Collections;
using UnityEngine;
using GH.GameStates;

namespace GH
{
    [CreateAssetMenu(menuName = "Actions/Condition/BattleAvailable")]
    public class BattlePhaseStartCheck : Condition
    {
        [SerializeField]
        private bool _CanStartBattle;
        public override bool IsValid()
        {
            GameController controller = GameController.singleton;
            if (controller.CurrentPlayer.fieldCard.Count > 0 && _CanStartBattle)
            {
                Debug.Log("BattlePhaseStartCheck: Start Battle Phase");
                return true;
            }
            else
            {
                Debug.Log("BattlePhaseStartCheck: Can't start battle phase");
                return false;
            }
        }
    }

}