﻿
using UnityEngine;
using GH.GameAction;
using GH.Multiplay;
using UnityEditor;
using GH.Player;
namespace GH.GameTurn
{

    [CreateAssetMenu(menuName = "Turns/Turn")]
    public class Turn : ScriptableObject
    {

#pragma warning disable 0649

        #region Private Field

        private int _PhaseIndex = 0;
        private bool _TurnBegin = true;
        private bool phaseStart = true;

        #endregion

        #region Private SerializeField

        [Tooltip("Check current phase during gameplay. This value won't needed in future.")]
        [SerializeField]
        private PhaseVariable _CurrentPhase;


        #endregion

        #region ReadOnly Field
        [SerializeField]
        private PlayerHolder _ThisTurnPlayer;
        [SerializeField]
        private Phase[] _Phases;
        [SerializeField]
        private PlayerAction[] _TurnStartAction;
        #endregion

#pragma warning restore 0649

        #region Properties
        public void EndCurrentPhase()
        {
            _Phases[PhaseIndex].PhaseForceExit = true;
        }
        public PlayerHolder ThisTurnPlayer
        {
            get { return _ThisTurnPlayer; }
        }
        public bool TurnBegin
        {
            set { _TurnBegin = value; }
            get { return _TurnBegin; }
        }
        public int PhaseIndex
        {
            set { _PhaseIndex = value; }
            get { return _PhaseIndex; }
        }
        public PhaseVariable CurrentPhase
        {
            get { return _CurrentPhase; }
        }
        #endregion


        #region Public Methods

        public void TurnStartActions()
        {

            Debug.Log("TurnStartActionOn");
            if (_TurnStartAction == null)
            {
                Debug.LogError("TurnStartAction Is Null");
                return;
            }
            for (int i = 0; i < _TurnStartAction.Length; i++)
            {
                Debug.LogFormat("Turn_TurnStartAction_{0} Run", _TurnStartAction[i].name);
                _TurnStartAction[i].Execute(ThisTurnPlayer);
            }
            if (ThisTurnPlayer.InGameData.ManaManager.MaxMana < 10)
                ThisTurnPlayer.InGameData.ManaManager.UpdateMaxMana(1);
            ThisTurnPlayer.InGameData.ManaManager.InitMana();
            MultiplayManager.singleton.SendPhase(ThisTurnPlayer.name, _Phases[PhaseIndex].PhaseName);
        }

        public bool Execute()
        {
            //Return value. Only gets true when turn runs all phases.
            bool result = false;
            _CurrentPhase.value = _Phases[PhaseIndex];
            Debug.LogFormat("CurrentPhase IS {0}", CurrentPhase.value.ToString());
            //At the first phase, which is beginning of the turn runs TurnStartActions
            if (PhaseIndex == 0 && TurnBegin == true)
            {
                Debug.Log("Turn: Execute Turn Start Action");
                TurnStartActions();
                TurnBegin = false;
            }
            else if (TurnBegin  ==true)
            {
                Debug.Log("Phase Index Error. Current Index is " + PhaseIndex);
            }

            if(phaseStart)
            {
                Debug.Log("Turn_PhaseStart: "+ _Phases[PhaseIndex].PhaseName);
                _Phases[PhaseIndex].OnStartPhase();
                phaseStart = false;
            }
            //Run Current Phase until 'IsComplete' is true
            bool IsComplete = _Phases[PhaseIndex].IsComplete();
            
            if (IsComplete)
            {
                _Phases[PhaseIndex].OnEndPhase();
                Debug.Log("Turn_PhaseEnd: " + _Phases[PhaseIndex].PhaseName);                
                PhaseIndex++;
                phaseStart = true;
                if (PhaseIndex + 1 > _Phases.Length)
                {
                    TurnBegin = true;
                    result = true;
                    PhaseIndex = 0;
                }
                MultiplayManager.singleton.SendPhase(ThisTurnPlayer.name, _Phases[PhaseIndex].PhaseName);
            }
            return result;
        }
        #endregion
    }
}

