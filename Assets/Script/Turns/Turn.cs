using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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


        [System.NonSerialized]
        private int _TurnIndex = 0;
        [System.NonSerialized]
        private bool _TurnBegin = true; 
        // Somehow, this is initialised different as I initialise. 
        // If I initialise as True, it becomes False
        // If I initialise as False, it becomes True;
     

        public void TurnStart()
        {
            if (_TurnStartAction == null)
                return;
            
            for (int i = 0; i < _TurnStartAction.Length; i++)
            {
                _TurnStartAction[i].Execute(_ThisTurnPlayer);
            }

            ///If current player has less than 10 mana resources, add 1. Nor, just initialise it.
            if (_ThisTurnPlayer.manaResourceManager.GetMaxMana() < 10)
                _ThisTurnPlayer.manaResourceManager.UpdateMaxMana(1);
            _ThisTurnPlayer.manaResourceManager.InitMana();
            Setting.RegisterLog(_ThisTurnPlayer.name + " turn starts ", _ThisTurnPlayer.playerColor);
        }
        public bool Execute()
        {
            bool result = false;
            _CurrentPhase.value = _Phases[_TurnIndex];
            _Phases[_TurnIndex].OnStartPhase();
            
            bool IsComplete = _Phases[_TurnIndex].IsComplete();
            if (_TurnBegin && _TurnIndex == 0)
            {
                //Debug.Log("This is the problem");
                TurnStart();
                _TurnBegin = false;
            }
            if (IsComplete)
            {                
                _Phases[_TurnIndex].OnEndPhase();
                _TurnIndex++;
                if(_TurnIndex>_Phases.Length-1)
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
            _Phases[_TurnIndex].PhaseForceExit = true;
        }       
        public PlayerHolder ThisTurnPlayer
        {
            set { _ThisTurnPlayer = value; }
            get{ return _ThisTurnPlayer; }
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
            set { _CurrentPhase = value; }
            get { return _CurrentPhase; }
        }
    }
}

