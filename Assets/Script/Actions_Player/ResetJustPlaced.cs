using UnityEngine;
using System.Collections;
using GH.GameCard;

namespace GH.GameAction
{
    [CreateAssetMenu(menuName = "Actions/Reset isJustPlaced")]
    public class ResetJustPlaced : PlayerAction
    {

        public override void Execute(PlayerHolder p)
        {
            foreach(CardInstance c in p.fieldCard)
            {
                if (!c.GetCanAttack())
                {
                    c.SetCanAttack(true);
                    c.ColorCard(false);
                    //Debug.Log(c.viz.card.name + " can attack now");
                }
            }
        }
    }
}
