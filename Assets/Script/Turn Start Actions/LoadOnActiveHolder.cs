using UnityEngine;
using System.Collections;


namespace GH.GameAction
{

    [CreateAssetMenu(menuName = "Actions/Player Actions/Load Active Holder")]
    public class LoadOnActiveHolder : PlayerAction
    {

        public override void Execute(PlayerHolder p)
        {
            //GameController.singleton.LoadPlayerOnActive(p);
            GameController.singleton.LoadPlayerUI.LoadPlayerOnActive(p);
        }
    }
}
