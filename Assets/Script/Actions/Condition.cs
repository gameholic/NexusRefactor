using UnityEngine;
using System.Collections;

namespace GH
{
    public abstract class Condition : ScriptableObject
    {
        public abstract bool IsValid();
    
    }
}