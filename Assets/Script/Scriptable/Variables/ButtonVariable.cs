using UnityEngine;
using UnityEditor;


namespace GH
{
    [CreateAssetMenu(menuName ="Variable/Button Variable")]
    public class ButtonVariablwe : ScriptableObject
    {

        public Button_Instance value;
        public void Set(Button_Instance v)
        {
            value = v;
        }

    }

}