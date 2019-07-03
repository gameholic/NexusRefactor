using UnityEngine;
using System.Collections;


namespace GH.GameTurn
{

    [CreateAssetMenu(menuName = "Turns/phase")]
    public abstract class Phase : ScriptableObject
    {
        [SerializeField]
        private bool _PhaseForceExit;
        [SerializeField]
        private string _PhaseName;
        [System.NonSerialized]
        private bool _IsInit;

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
            get{ return _PhaseName; }
        }
        public bool IsInit
        {
            set { _IsInit = value; }
            get { return _IsInit; }
        }
    }
}
    