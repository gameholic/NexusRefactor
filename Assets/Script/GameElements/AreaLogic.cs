 using UnityEngine;
using System.Collections;

namespace GH
{
    public abstract class AreaLogic : ScriptableObject
    {
        public abstract void Execute(GameElements.Area a);
        //public abstract void Execute();
        
    }
}

