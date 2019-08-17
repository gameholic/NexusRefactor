#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using System.IO;

using GH.Player;


namespace GH.AssetEditor
{
    public class PlayerProfileEditor : EditorWindow
    {
        Vector2 scrollPos;

        public PlayerProfile playerProfile;

        private string playerProfileFilePath = "/StreamingAssets/playerProfile.json";

        [MenuItem("Editor/Player Profile Editor")]
        static void Init()
        {
            //create new 'GameDataEditor' window using 'EditorWindow' 
            PlayerProfileEditor window = (PlayerProfileEditor)EditorWindow.GetWindow(typeof(PlayerProfileEditor), true, "EditPlayerProfile");
            window.Show();
        }

        private void OnGUI()
        {

            EditorGUILayout.BeginHorizontal();
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);


            if (playerProfile != null)
            {
                SerializedObject serializedObject = new SerializedObject(this);
                SerializedProperty serializedProperty = serializedObject.FindProperty("playerProfile");

                EditorGUILayout.PropertyField(serializedProperty, true);
                serializedObject.ApplyModifiedProperties();
                if (GUILayout.Button("Save data"))
                {
                    SaveProfile();
                }
            }

            if (GUILayout.Button("New data"))
            {
                playerProfile = new PlayerProfile();
            }
            if (GUILayout.Button("Load data"))
            {
                LoadProfile();
            }
            if (GUILayout.Button("Convert data as Asset"))
            {
                ConvertProfile();
            }
            if(GUILayout.Button("Convert Asset to data"))
            {
                LoadAsset();
            }


            EditorGUILayout.EndScrollView();
        }

        private string path = "PlayerProfile/PlayerProfile";

        private void LoadAsset()
        {
            ConvertPlayerProfileToAsset profileAsset = Resources.Load(path) as ConvertPlayerProfileToAsset;
            if(profileAsset!=null)
                playerProfile = profileAsset.playerProfile;
            else
                Debug.LogError("ConverProfile: Cannot Find ProfileAsset");


        }

        private void ConvertProfile()
        {
            ConvertPlayerProfileToAsset profileAsset = Resources.Load(path) as ConvertPlayerProfileToAsset;
            if (profileAsset != null)
                profileAsset.playerProfile = playerProfile;
            else
                Debug.LogError("ConverProfile: Cannot Find ProfileAsset");

        }

        private void LoadProfile()
        {
            string filePath = Application.dataPath + playerProfileFilePath;


            if (File.Exists(filePath))
            {
                string dataAsJson = File.ReadAllText(filePath);
                Debug.Log(dataAsJson);
                playerProfile = JsonUtility.FromJson<PlayerProfile>(dataAsJson);
            }
            else
            {
                playerProfile = new PlayerProfile();
            }
        }


        private void SaveProfile()
        {
            string dataAsJson = JsonUtility.ToJson(playerProfile);
            Debug.Log(dataAsJson);
            string filePath = Application.dataPath + playerProfileFilePath;

            File.WriteAllText(filePath, dataAsJson);
        }
    }




}
#endif