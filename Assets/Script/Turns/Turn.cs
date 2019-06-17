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
        [SerializeField]
        private PlayerHolder _ThisTurnPlayer;
        [SerializeField]
        private PhaseVariable _CurrentPhase;
        [SerializeField]
        private Phase[] _Phases;
        [SerializeField]
        private PlayerAction[] _TurnStartAction;


        private int _PhaseIndex;
        private bool _TurnBegin;
        // Somehow, this is initialised different as I initialise. 
        // If I initialise as True, it becomes False
        // If I initialise as False, it becomes True;


        #region Get/SetFunctions
        public void EndCurrentPhase()
        {
            _Phases[PhaseIndex].PhaseForceExit = true;
        }
        public PlayerHolder ThisTurnPlayer
        {
            set { _ThisTurnPlayer = value; }
            get { return _ThisTurnPlayer; }
        }
        public bool TurnBegin
        {
            set { _TurnBegin = value; }
            get { return _TurnBegin; }
        }
        public int PhaseIndex
        {
            get { return _PhaseIndex; }
        }
        public PhaseVariable CurrentPhase
        {
            set { _CurrentPhase = value; }
            get { return _CurrentPhase; }
        }

        #endregion

        private void Awake()
        {
            _PhaseIndex = 0;
            TurnBegin = true;
        }
        public void TurnStart()
        {
            if (_TurnStartAction == null)
                return;            
            for (int i = 0; i < _TurnStartAction.Length; i++)
            {
                _TurnStartAction[i].Execute(_ThisTurnPlayer);
            }

            Setting.RegisterLog(_ThisTurnPlayer.name + " started turn", _ThisTurnPlayer.playerColor);
            ///If current player has less than 10 mana resources, add 1. Nor, just initialise it.
            if (_ThisTurnPlayer.manaResourceManager.GetMaxMana() < 10)
                _ThisTurnPlayer.manaResourceManager.UpdateMaxMana(1);
            _ThisTurnPlayer.manaResourceManager.InitMana();


            MultiplayManager.singleton.SendPhase(ThisTurnPlayer.name, _Phases[PhaseIndex].PhaseName);
        }
        public bool Execute()
        {
            bool result = false;
            _CurrentPhase.value = _Phases[PhaseIndex];
            _Phases[PhaseIndex].OnStartPhase();
            bool IsComplete = _Phases[PhaseIndex].IsComplete();
            if (TurnBegin && PhaseIndex == 0)
            {
                TurnStart();
                TurnBegin = false;
            }

            if (IsComplete)
            {
                _Phases[PhaseIndex].OnEndPhase();
                _PhaseIndex++;
                if (PhaseIndex>_Phases.Length-1)
                {
                    _PhaseIndex = 0;
                    TurnBegin = true;
                    result = true;
                }

                MultiplayManager.singleton.SendPhase(ThisTurnPlayer.name, _Phases[PhaseIndex].PhaseName);

            }
            return result;
        }
    }
}

