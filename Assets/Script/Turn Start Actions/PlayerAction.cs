using UnityEngine;
using System.Collections;
using GH.Player;

namespace GH.GameAction
{
    public abstract class PlayerAction : ScriptableObject
    {
        public abstract void Execute(PlayerHolder p);
    }
}
