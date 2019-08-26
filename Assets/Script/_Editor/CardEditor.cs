#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using GH.GameCard;
using GH.GameCard.CardInfo;

namespace GH.Editor
{
    public class CardEditor : EditorWindow
    {
        Vector2 scrollPos;
        string cardName;

        static CardEditor mainWindow;
        static SubCardWindow subWindow;

        bool addingMode = false;
        bool subWinMode = false;

        [MenuItem("Editor/Card Editor")]
        private static void Init()
        {
            //create new 'GameDataEditor' window using 'EditorWindow'
            mainWindow = (CardEditor)EditorWindow.GetWindow(typeof(CardEditor), true, "CardEditor");
            subWindow = CreateInstance<SubCardWindow>();

            mainWindow.Show();
        }
        private void OnGUI()
        {

            Event e = Event.current;
            EditorGUILayout.BeginHorizontal();
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

            GUILayout.Label("Type the card you are trying to search");
            cardName = EditorGUILayout.TextField(cardName);
            if (subWindow.CardSearch())
            {
                subWindow.ShowCard();
            }
            if (!addingMode)
            {
                if (GUILayout.Button("Search") || e.keyCode == KeyCode.Return)
                {
                    //ShowSubWin();
                    subWindow.SearchCard(cardName);
                }
                if (GUILayout.Button("AddCard"))
                {
                    //ShowSubWin();
                    subWindow.ResetCardNData();
                    addingMode = true;
                }
                if (GUILayout.Button("DeleteCard"))
                {
                    subWindow.DeleteCard();
                }

            }
            else if (addingMode)
            {
                if (GUILayout.Button("SaveCard"))
                {
                    subWindow.SaveCard();
                    addingMode = false;
                }
                if (GUILayout.Button("ClearData"))
                {
                    subWindow.ClearCard();
                }
                if (GUILayout.Button("Cancel"))
                {
                    addingMode = false;
                    subWindow.ClearCard();
                }
            }
            EditorGUILayout.EndScrollView();
            //if(subWinMode)
            //{
            //    if(GUILayout.Button("CloseWindow"))
            //    {
            //        CloseSubWin();
            //    }
            //}
        }
        //void CloseSubWin()
        //{
        //    subWindow.Close();
        //    subWinMode = false;

        //}
        //void ShowSubWin()
        //{
        //    subWindow.Show();
        //    subWinMode = true;
        //}        
    }


    public class SubCardWindow : CardEditor
    {
        bool initialised = false;

        SerializedObject serializedObject;
        SerializedProperty serializedProperty;

        Card currentCard = null;
        CardData cardData = null;


        public void ShowCard()
        {
            if (!initialised)
            {
                Debug.Log("hey");
                serializedObject = new SerializedObject(currentCard);
                serializedProperty = serializedObject.FindProperty("_Data");
                initialised = true;
            }
            EditorGUILayout.PropertyField(serializedProperty, true);
            serializedObject.ApplyModifiedProperties();

            if (GUILayout.Button("Revert"))
            {
                serializedObject.Update();
            }
        }

        public bool CardSearch()
        {
            if (currentCard)
                return true;
            else
                return false;
        }
        public void ResetCardNData()
        {
            cardData = new CardData();
            currentCard = CreateInstance<Card>();
            currentCard.SetCardData = cardData;
        }
        public void SaveCard()
        {
            cardData = currentCard.Data;
            if (cardData != null)
            {
                AssetDatabase.CreateAsset(currentCard, "Assets/Resources/Card/" + cardData.Name + ".asset");
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                ResetCardNData();
            }
            else
            {
                Debug.LogError("CardDATA IS NULL");
            }
        }
        public void DeleteCard()
        {
            AssetDatabase.DeleteAsset("Assets/Resources/Card/" + currentCard.Data.Name + ".asset");
            AssetDatabase.Refresh();
            Debug.Log("CardDeleted");
            ClearCard();
        }

        public void ClearCard()
        {
            cardData = null;
            currentCard = null;
        }

        public void SearchCard(string str)
        {
            Card c = Resources.Load<Card>("Card/" + str);
            if (c != null)
            {
                currentCard = c;
                cardData = c.Data;
            }
            else
            {
                Debug.LogWarning("FailToSearch " + str);
            }
        }
    }
}
#endif