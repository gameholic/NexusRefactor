using UnityEngine;
using System.Collections;


namespace GH.GameTurn
{

    [CreateAssetMenu(menuName = "Turns/phase")]
    public abstract class Phase : ScriptableObject
    {
        [SerializeField]
        protected bool _PhaseForceExit;
        [SerializeField]
        private string _PhaseName;
        [System.NonSerialized]
        protected bool _IsInit;
        public abstract bool IsComplete();
        public abstract void OnStartPhase();
        public abstract void OnEndPhase();
        public bool PhaseForceExit
        {
            set { _PhaseForceExit = value;}
            get { return _PhaseForceExit; }
        }

        public string PhaseName
        {
            get
            {
                return _PhaseName;
            }

        }
    }
}
    