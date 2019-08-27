using GH.AssetEditor;
using GH.Player;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace GH
{
    public class FileBridge
    {
        string profileFilePath = "/StreamingAssets/playerProfile.json";
        private PlayerProfile playerProfile;

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
        public void UpdateAsset(PlayerProfile p)
        {
            ConvertPlayerProfileToAsset a = Resources.Load("PlayerProfile/PlayerProfile") as ConvertPlayerProfileToAsset;
            if (a != null)
            {
                Debug.Log("ProfileAssetSaved");
                a.playerProfile._DeckToPlay = p._DeckToPlay;
            }
            else
                Debug.LogError("ProfileAssetFailedSaving");
        }
    }

}