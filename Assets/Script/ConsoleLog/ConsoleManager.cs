using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GH
{
    public class ConsoleManager : MonoBehaviour
    {
        public Transform consoleGrid;
        public GameObject prefab;
        Text[] textObjs;
        int index;

        public ConsoleHook hook;

        private void Awake()
        {
            hook.consoleManager = this;
            textObjs = new Text[5];
            for (int i=0;i< textObjs.Length; i++)
            {
                GameObject go = Instantiate(prefab) as GameObject;
                textObjs[i] = go.GetComponent<Text>();
                go.transform.SetParent(consoleGrid);
            }
        }

        public void RegisterEvent(string s,Color color)
        {
            index++;       
            if (index > textObjs.Length -1)
            {
                index = 0;
            } 
            textObjs[index].color = color;
            textObjs[index].text = s;
            textObjs[index].gameObject.SetActive(true);
            textObjs[index].transform.SetAsLastSibling();
        }


    }
}