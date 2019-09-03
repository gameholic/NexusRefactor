using UnityEngine;
using System.Collections;


namespace GH.GameTurn
{

    public enum PhaseId { Control, Block, Battle}

    [CreateAssetMenu(menuName = "Turns/phase")]
    public abstract class Phase : ScriptableObject
    {
        [SerializeField]
        private bool _PhaseForceExit;
        [SerializeField]
        private string _PhaseName;
        [System.NonSerialized]
        private bool _IsInit;
        [SerializeField]
        private PhaseId _phaseLogic;
        
        public PhaseId GetPhaseId { get { return _phaseLogic; } }
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
    