using UnityEngine;
using System.Collections;

namespace GH.GameAction
{
    public abstract class PlayerAction : ScriptableObject
    {
        public abstract void Execute(PlayerHolder p);
    }
}
