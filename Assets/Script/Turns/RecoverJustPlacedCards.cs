using UnityEngine;
using System.Collections;

namespace GH
{
    [CreateAssetMenu(menuName ="Actions/Player Actions/RecoverJustPlaced")]
    public class RecoverJustPlacedCards : PlayerAction
    {
        public override void Execute(PlayerHolder p)
        {
            foreach(CardInstance c in p.fieldCard)
            {
                if (c.canAttack)
                    c.SetIsJustPlaced(false);
            }
        }

    }


}