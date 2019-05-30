using UnityEngine;
using System.Collections;


namespace GH
{

    [CreateAssetMenu(menuName ="Console/Hook")]
    public class ConsoleHook : ScriptableObject
    {
        [System.NonSerialized]
        private ConsoleManager _ConsoleManager;
        public void RegisterEvent(string s, Color color)
        {
            _ConsoleManager.RegisterEvent(s, color);
        }
        public ConsoleManager ConsoleManager
        {
            set { _ConsoleManager = value; }
            get { return _ConsoleManager; }
        }

    }
}
