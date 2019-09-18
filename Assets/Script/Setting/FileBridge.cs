using GH.AssetEditor;
using GH.Player;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace GH
{
    public class FileBridge
    {
        static string profileFilePath = "/StreamingAssets/playerProfile.json";
        static string logFilePath= "/StreamingAssets/ErrorLog.txt";
        private PlayerProfile playerProfile;

        public static PlayerProfile LoadProfile()
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
        public static string LoadLog()
        {
            string log = null;
            string path = Application.dataPath + logFilePath;
            if(File.Exists(path))
            {
                string logData = File.ReadAllText(path);
                log = logData;
                Debug.Log("LoadLogSucess///"+ logData);
            }
            return log;
        }
        public static void SaveProfile(PlayerProfile target)
        {
            string dataToJson = JsonUtility.ToJson(target);
            string path = Application.dataPath + profileFilePath;
            if(target !=null)
                File.WriteAllText(path, dataToJson);

            Debug.Log("Profile Save Sucess");
        }
        public static void SaveLogFile(string log)
        {            
            string path = Application.dataPath + logFilePath;
            if(log !=null)
            {
                if(!File.Exists(path))
                {
                    Debug.Log("Can't Find LogFile");
                    File.Create(path);
                    return;
                }
                File.WriteAllText(path, log);
                Debug.Log("LogFileSave");
            }
        }
        public static void UpdateAsset(PlayerProfile p)
        {
            ConvertPlayerProfileToAsset a = Resources.Load("PlayerProfile/PlayerProfile") as ConvertPlayerProfileToAsset;
            if (a != null)
            {
                a.playerProfile= p;
                Debug.Log("ProfileAssetUpdated");
            }
            else
                Debug.LogError("ProfileAssetFailedSaving");
        }
    }

}