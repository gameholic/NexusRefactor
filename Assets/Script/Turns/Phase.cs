using UnityEngine;
using System.Collections;


namespace GH.GameTurn
{

    [CreateAssetMenu(menuName = "Turns/phase")]
    public abstract class Phase : ScriptableObject
    {
        public bool forceExit;
        public string phaseName;
        [System.NonSerialized]
        protected bool isInit;
        public abstract bool IsComplete();
        public abstract void OnStartPhase();
        public abstract void OnEndPhase();
    }
}
    