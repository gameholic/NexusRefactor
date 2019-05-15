using UnityEngine;
using System.Collections;


namespace GH
{

    [CreateAssetMenu(menuName = "Actions/Reset isJustPlaced")]
    public class ResetJustPlaced : PlayerAction
    {

        public override void Execute(PlayerHolder p)
        {
            foreach(CardInstance c in p.fieldCard)
            {
                if (!c.canAttack)
                {
                    c.canAttack = true;
                    c.SetIsJustPlaced(false);
                    //Debug.Log(c.viz.card.name + " can attack now");
                }
            }
        }
    }
}
