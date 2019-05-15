using UnityEngine;
using System.Collections;
using GH.GameStates;

namespace GH
{
    public class Button_Instance : MonoBehaviour, IClickable
    {
        public GameObject buttonObj;
        public GH.GameElements.Button_InstanceLogic buttonLogic;
        public void OnClick()
        {
            if (buttonLogic == null)
                return;
            Setting.gameController.EndPhase();
        }

        public void OnHighlight()
        {
            if (buttonLogic == null)
                return;
            buttonLogic.OnHighlight(buttonObj);
        }
    }
}
