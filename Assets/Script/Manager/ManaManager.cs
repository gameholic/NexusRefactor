using UnityEngine;
using System.Collections;

namespace GH
{
    public class ManaManager 
    {
        public int currentMana;
        public int maxMana;

        
        public void UpdateMaxMana(int variation)
        {
            maxMana = maxMana + variation;
        }

        public void UpdateCurrentMana(int variation)
        {
            currentMana = currentMana + variation;
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
