using GH.Player;
using UnityEngine;

namespace GH.GameAction
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
