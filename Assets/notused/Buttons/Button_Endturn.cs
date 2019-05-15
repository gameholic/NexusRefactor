using UnityEngine;
using UnityEditor;

namespace GH.GameElements
{
    [CreateAssetMenu(menuName ="Button/End Button")]
    public class Button_Endturn : Button_InstanceLogic
    {
        public ButtonVariablwe currentButton;
        public GH.GameEvent buttonPushed;
        public override void OnClick(GameObject buttonObj)
        {
            Setting.gameController.EndPhase();
            buttonPushed.Raise();
        }
        public override void OnHighlight(GameObject buttonObj)
        {
           SpriteRenderer bgImage = buttonObj.GetComponent<SpriteRenderer>();
           bgImage.color = new Color(0, 70, 50, 30);
        }

    }
}