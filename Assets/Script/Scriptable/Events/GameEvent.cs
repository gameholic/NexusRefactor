using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GH
{
    [CreateAssetMenu(menuName = "Game Event")]
    public class GameEvent : ScriptableObject
    {
        List<GameEventListener> listeners = new List<GameEventListener>();

        public void Register(GameEventListener l)
        {
            //Debug.Log(this.name + " Registered");
            listeners.Add(l);
        }

        public void UnRegister(GameEventListener l)
        {
            //Debug.Log(this.name + " Removed");
            listeners.Remove(l);
        }

        public void Raise()
        {
            for (int i = 0; i < listeners.Count; i++)
            {
                //Debug.Log(this.name + " Raised");
                listeners[i].Response();
            }
        }
    }
}
