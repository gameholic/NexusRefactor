using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GH
{

    [CreateAssetMenu(menuName = "Turns/Turn")]
    public class Turn : ScriptableObject
    {

        public PlayerHolder player;
        public PhaseVariable currentPhase; 
        public Phase[] phases;
        public PlayerAction[] turnStartActions;


        [System.NonSerialized]
        public int index = 0;
        [System.NonSerialized]
        public bool firstTime = true; 
        // Somehow, this is initialised different as I initialise. 
        // If I initialise as True, it becomes False
        // If I initialise as False, it becomes True;
     

        public void TurnStart()
        {
            if (turnStartActions == null)
                return;
            
            for (int i = 0; i < turnStartActions.Length; i++)
            {
                turnStartActions[i].Execute(player);
            }

            ///If current player has less than 10 mana resources, add 1. Nor, just initialise it.
            if (player.manaResourceManager.GetMaxMana() < 10)
                player.manaResourceManager.UpdateMaxMana(1);
            player.manaResourceManager.InitMana();
            Setting.RegisterLog(player.name + " turn starts ", player.playerColor);
        }
        public bool Execute()
        {
            bool result = false;
            currentPhase.value = phases[index];
            phases[index].OnStartPhase();
            
            bool IsComplete = phases[index].IsComplete();
            if (firstTime && index == 0)
            {
                //Debug.Log("This is the problem");
                TurnStart();
                firstTime = false;
            }

            if (IsComplete)
            {
                
                phases[index].OnEndPhase();
                index++;
                if(index>phases.Length-1)
                {
                    index = 0;

                    firstTime = true;
                    result = true;
                }

            }

            return result;
        }


        public void EndCurrentPhase()
        {
            phases[index].forceExit = true;
        }
        
    }
}

