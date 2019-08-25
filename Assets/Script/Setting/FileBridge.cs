using GH.Player;
using System.IO;
using UnityEngine;

namespace GH
{
    public class FileBridge
    {
        string profileFilePath = "/StreamingAssets/playerProfile.json";

        public PlayerProfile LoadProfile()
        {
            PlayerProfile v = null;
            string path = Application.dataPath + profileFilePath;
            if (File.Exists(path))
            {
                string dataFromJson = File.ReadAllText(path);
                v = JsonUtility.FromJson<PlayerProfile>(dataFromJson);
                Debug.Log("Profile Loaded Success");
            }
            else
                v = new PlayerProfile();
            return v;
        }
        public void SaveProfile(PlayerProfile target)
        {
            string dataToJson = JsonUtility.ToJson(target);
            string path = Application.dataPath + profileFilePath;
            File.WriteAllText(path, dataToJson);
            Debug.Log("Profile Save Sucess");
        }
    }

}