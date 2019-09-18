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
            ConvertPlayerProfileToAsset profileAsset = Resources.Load("PlayerProfile/PlayerProfile") as ConvertPlayerProfileToAsset;


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
                ConvertProfile(profileAsset);
            }
            if(GUILayout.Button("Convert Asset to data"))
            {
                LoadAsset(profileAsset);
            }


            EditorGUILayout.EndScrollView();
        }

        private void LoadAsset(ConvertPlayerProfileToAsset profileAsset)
        {
            if(profileAsset!=null)
                playerProfile = profileAsset.playerProfile;
            else
                Debug.LogError("ConverProfile: Cannot Find ProfileAsset");
        }

        private void ConvertProfile(ConvertPlayerProfileToAsset profileAsset)
        {
            if (profileAsset != null)
                profileAsset.playerProfile = playerProfile;
            else
            {
                ConvertPlayerProfileToAsset newAsset = new ConvertPlayerProfileToAsset();
                newAsset.playerProfile = playerProfile;
                AssetDatabase.CreateAsset(newAsset,"Assets/Data/Resources/PlayerProfile/PlayerProfile.asset");
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();

            }   

        }

        private void LoadProfile()
        {
            playerProfile = FileBridge.LoadProfile();
        }

        private void SaveProfile()
        {
            FileBridge.SaveProfile(playerProfile);
        }
    }




}
#endif