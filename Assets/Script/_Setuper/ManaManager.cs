using UnityEngine;
using System.Collections;

namespace GH
{
    public class ManaManager 
    {
        private int currentMana;
        private int maxMana;

        
        public void UpdateMaxMana(int chnages)
        {
            maxMana = maxMana + chnages;
        }
        public void UpdateCurrentMana(int changes)
        {
            currentMana = currentMana + changes;
        }
        public void InitMana()
        {
            currentMana = maxMana;
        }
        public void InitManaZero()
        {
            currentMana = 0;
            maxMana = 0;
        }
        public int GetCurrentMana(){return currentMana;}
        public int GetMaxMana() { return maxMana; }
    }

}
