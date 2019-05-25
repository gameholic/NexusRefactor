using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GH.GameTurn
{

    [CreateAssetMenu(menuName = "Turns/Turn")]
    public class Turn : ScriptableObject
    {

        [SerializeField]
        private PlayerHolder _thisTurnPlayer;
        [SerializeField]
        private PhaseVariable _currentPhase;
        [SerializeField]
        private Phase[] _phases;
        [SerializeField]
        private PlayerAction[] _turnStartActions;


        [System.NonSerialized]
        private int _TurnIndex = 0;
        [System.NonSerialized]
        private bool _TurnBegin = true; 
        // Somehow, this is initialised different as I initialise. 
        // If I initialise as True, it becomes False
        // If I initialise as False, it becomes True;
     

        public void TurnStart()
        {
            if (_turnStartActions == null)
                return;
            
            for (int i = 0; i < _turnStartActions.Length; i++)
            {
                _turnStartActions[i].Execute(_thisTurnPlayer);
            }

            ///If current player has less than 10 mana resources, add 1. Nor, just initialise it.
            if (_thisTurnPlayer.manaResourceManager.GetMaxMana() < 10)
                _thisTurnPlayer.manaResourceManager.UpdateMaxMana(1);
            _thisTurnPlayer.manaResourceManager.InitMana();
            Setting.RegisterLog(_thisTurnPlayer.name + " turn starts ", _thisTurnPlayer.playerColor);
        }
        public bool Execute()
        {
            bool result = false;
            _currentPhase.value = _phases[_TurnIndex];
            _phases[_TurnIndex].OnStartPhase();
            
            bool IsComplete = _phases[_TurnIndex].IsComplete();
            if (_TurnBegin && _TurnIndex == 0)
            {
                //Debug.Log("This is the problem");
                TurnStart();
                _TurnBegin = false;
            }
            if (IsComplete)
            {                
                _phases[_TurnIndex].OnEndPhase();
                _TurnIndex++;
                if(_TurnIndex>_phases.Length-1)
                {
                    _TurnIndex = 0;
                    _TurnBegin = true;
                    result = true;
                }
            }
            return result;
        }
        public void EndCurrentPhase()
        {
            _phases[_TurnIndex].PhaseForceExit = true;
        }       
        public PlayerHolder ThisTurnPlayer
        {
            set { _thisTurnPlayer = value; }
            get{ return _thisTurnPlayer; }
        }
        public bool TurnBegin
        {
            set { _TurnBegin = value;}
            get { return _TurnBegin; }
        }
        public int TurnIndex
        {
            get { return _TurnIndex; }
        }
        public PhaseVariable CurrentPhase
        {
            set { _currentPhase = value; }
            get { return _currentPhase; }
        }
    }
}

