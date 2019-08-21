using UnityEngine;
using System.Collections;

namespace GH
{
    public class ManaManager 
    {
        private int currentMana;
        private int maxMana;

        
        public void UpdateMaxMana(int changes)
        {
            maxMana = maxMana + changes;
        }
        public void UpdateCurrentMana(int changes)
        {
            currentMana = currentMana + changes;
        }
        public void UseMana(int use)
        {
            UpdateCurrentMana(-use);
        }
        public bool HaveEnoughMana(int mana)
        {
            if (currentMana < mana)
                return false;
            return true;
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
        public int CurrentMana
        {
            set { currentMana = value; }
            get { return currentMana; }
        }
        public int MaxMana
        {
            set { maxMana = value; }
            get { return maxMana; }
        }
    }

}
