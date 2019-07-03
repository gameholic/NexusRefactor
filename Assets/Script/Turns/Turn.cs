using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GH.GameAction;
using GH.Multiplay;

namespace GH.GameTurn
{

    [CreateAssetMenu(menuName = "Turns/Turn")]
    public class Turn : ScriptableObject
    {

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

        #region Get/SetFunctions and Properties
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
            if (_TurnStartAction == null)
                return;
            for (int i = 0; i < _TurnStartAction.Length; i++)
            {
                _TurnStartAction[i].Execute(ThisTurnPlayer);
            }
            Setting.RegisterLog(ThisTurnPlayer.name + " started turn", ThisTurnPlayer.playerColor);

            if (ThisTurnPlayer.manaResourceManager.GetMaxMana() < 10)
                ThisTurnPlayer.manaResourceManager.UpdateMaxMana(1);

            ThisTurnPlayer.manaResourceManager.InitMana();
            MultiplayManager.singleton.SendPhase(ThisTurnPlayer.name, _Phases[PhaseIndex].PhaseName);
        }

        public bool Execute()
        {

            //Return value. Only gets true when turn runs all phases.
            bool result = false;

            _CurrentPhase.value = _Phases[PhaseIndex];

            //At the first phase, which is beginning of the turn runs TurnStartActions
            if (PhaseIndex == 0 && TurnBegin == true)
            {
                TurnStartActions();
                TurnBegin = false;
            }

            if(phaseStart)
            {
                _Phases[PhaseIndex].OnStartPhase();
                phaseStart = false;

            }

            //Run Current Phase until 'IsComplete' is true
            bool IsComplete = _Phases[PhaseIndex].IsComplete();

            if (IsComplete)
            {
                _Phases[PhaseIndex].OnEndPhase();
                PhaseIndex++;
                phaseStart = true;
                if (PhaseIndex + 1 >= _Phases.Length)
                {
                    TurnBegin = true;
                    result = true;
                    PhaseIndex = 0;
                }
                MultiplayManager.singleton.SendPhase(ThisTurnPlayer.name, _Phases[PhaseIndex].PhaseName);

            }
            return result;

            //bool result = false;
            //_CurrentPhase.value = _Phases[PhaseIndex];
            //if (TurnBegin)
            //{
            //    TurnStartActions();
            //    TurnBegin = false;
            //}

            //_Phases[PhaseIndex].OnStartPhase();
            //bool IsComplete = _Phases[PhaseIndex].IsComplete();

            //if (IsComplete)
            //{
            //    _Phases[PhaseIndex].OnEndPhase();
            //    _PhaseIndex++;
            //    if (PhaseIndex > _Phases.Length - 1)
            //    {
            //        _PhaseIndex = 0;
            //        TurnBegin = true;
            //        result = true;
            //    }
            //    MultiplayManager.singleton.SendPhase(ThisTurnPlayer.name, _Phases[PhaseIndex].PhaseName);
            //}
            //return result;
        }

        #endregion



    }
}

