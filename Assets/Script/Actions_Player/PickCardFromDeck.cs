using UnityEngine;
using System.Collections;


namespace GH
{

    [CreateAssetMenu(menuName = "Actions/Player Actions/Pick Card From Deck")]
    public class PickCardFromDeck : PlayerAction
    {

        public override void Execute(PlayerHolder p)
        {
            GameController.singleton.PickNewCardFromDeck(p);

        }
    }
}
