using UnityEngine;
using System.Collections;

namespace GH
{

    public class ManaHolder : MonoBehaviour
    {
        
        public TextMesh currentMana;
        public TextMesh maxMana;

        public void initMana()
        {
            currentMana.text = "0";
            maxMana.text = "0";
        }
    }

}
