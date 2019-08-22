using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.IO;
using GH.Player;
using GH.Player.ProfileData;

public class DropDownController : MonoBehaviour
{

    public Dropdown myDropdown;
    void Start()
    {
        myDropdown.ClearOptions();
        
        myDropdown.AddOptions(CallDeck());
        myDropdown.onValueChanged.AddListener(delegate {
            myDropdownValueChangedHandler(myDropdown);
        });
    }
    public PlayerProfile CallProfileFromJSon()
    {
        PlayerProfile profile;
        string jsonPath = Application.dataPath + "/StreamingAssets/playerProfile.json";
        if (!File.Exists(jsonPath))
        {
            Debug.LogError("Can't find json");
            return null;
        }
        string dataAsJson = File.ReadAllText(jsonPath);
        profile = JsonUtility.FromJson<PlayerProfile>(dataAsJson);
        return profile;
    }
    private List<string> CallDeck()
    {
        PlayerProfile p = CallProfileFromJSon();
        List<string> deckLists = new List<string>();
        if (p == null)
            return null;
        else
            Debug.Log(p.Name);

        foreach (ProfileData_Deck v in p.deckList)
        {
            if (v.Name!=null)
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

    public void ButtonOnClick()
    {



    }
}