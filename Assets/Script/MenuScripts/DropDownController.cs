using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.IO;
using GH.Player;
using GH.Player.ProfileData;


namespace GH
{
    public class DropDownController : MonoBehaviour
    {
        public Dropdown myDropdown;

        FileBridge fileBridge = new FileBridge();
        void Start()
        {
            myDropdown.ClearOptions();
            myDropdown.AddOptions(CallDeck());
            myDropdown.onValueChanged.AddListener(delegate {
                myDropdownValueChangedHandler(myDropdown);
            });
        }
        private List<string> CallDeck()
        {
            PlayerProfile p = fileBridge.LoadProfile();
            List<string> deckLists = new List<string>();
            if (p == null)
                return null;
            else
                Debug.Log(p.Name);

            foreach (ProfileData_Deck v in p.deckList)
            {
                if (v.Name != null)
                {
                    Debug.Log(v.Name);
                    deckLists.Add(v.Name);
                }
            }
            return deckLists;
        }
        void Destroy()
        {
            myDropdown.onValueChanged.RemoveAllListeners();
        }

        private void myDropdownValueChangedHandler(Dropdown target)
        {
            Debug.Log("selected: " + target.value);
        }

        public void SetDropdownIndex(int index)
        {
            myDropdown.value = index;
        }
    }

}