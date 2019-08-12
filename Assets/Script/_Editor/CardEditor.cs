using UnityEngine;
using UnityEditor;


//namespace GH.Editor
//{
//    public class CardEditor : EditorWindow
//    {
//        Vector2 scrollPos;
//        string cardName;

//        static CardEditor mainWindow;
//        static SubCardWindow subWindow;


//        bool addingMode = false;
//        bool subWinMode = false;

//        [MenuItem("Editor/Card Editor")]
//        private static void Init()
//        {
//            //create new 'GameDataEditor' window using 'EditorWindow' 
//            mainWindow = (CardEditor)EditorWindow.GetWindow(typeof(CardEditor), true, "CardEditor");
//            subWindow = CreateInstance<SubCardWindow>();

//            mainWindow.Show();
//        }
//        private void OnGUI()
//        {

//            Event e = Event.current;
//            EditorGUILayout.BeginHorizontal();
//            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

//            GUILayout.Label("Type the card you are trying to search");
//            cardName = EditorGUILayout.TextField(cardName);
//            if (subWindow.CardSearch())
//            {
//                subWindow.ShowCard();
//            }
//            if (!addingMode)
//            {
//                if (GUILayout.Button("Search") || e.keyCode == KeyCode.Return)
//                {
//                    //ShowSubWin();
//                    subWindow.SearchCard(cardName);
//                }
//                if (GUILayout.Button("AddCard"))
//                {
//                    //ShowSubWin();
//                    subWindow.ResetCardNData();
//                    addingMode = true;
//                }
//                if (GUILayout.Button("DeleteCard"))
//                {
//                    subWindow.DeleteCard();
//                }

//            }
//            else if (addingMode)
//            {
//                if (GUILayout.Button("SaveCard"))
//                {
//                    subWindow.SaveCard();
//                    addingMode = false;
//                }
//                if (GUILayout.Button("ClearData"))
//                {
//                    subWindow.ClearCard();
//                }
//                if (GUILayout.Button("Cancel"))
//                {
//                    addingMode = false;
//                    subWindow.ClearCard();
//                }

//            }
//            /*
//            if(subWinMode)
//            {
//                if(GUILayout.Button("CloseWindow"))
//                {
//                    CloseSubWin();
//                }
//            }
//            */
//            EditorGUILayout.EndScrollView();

//        }
//        /*
//        void CloseSubWin()
//        {
//            subWindow.Close();
//            subWinMode = false;

//        }
//        void ShowSubWin()
//        {
//            subWindow.Show();
//            subWinMode = true;
//        }
//        */
//    }


//    public class SubCardWindow : CardEditor
//    {


//        bool initialised = false;

//        SerializedObject serializedObject;
//        SerializedProperty serializedProperty;

//        CardDataTest cardData = null;
//        CardTest currentCard = null;


//        public void ShowCard()
//        {
//            if (!initialised)
//            {
//                Debug.Log("hey");
//                serializedObject = new SerializedObject(currentCard);
//                serializedProperty = serializedObject.FindProperty("data");
//                initialised = true;
//            }
//            EditorGUILayout.PropertyField(serializedProperty, true);
//            serializedObject.ApplyModifiedProperties();

//            if (GUILayout.Button("Revert"))
//            {
//                serializedObject.Update();
//            }
//        }

//        public bool CardSearch()
//        {
//            if (currentCard)
//                return true;
//            else
//                return false;
//        }
//        public void ResetCardNData()
//        {
//            cardData = new CardDataTest();
//            currentCard = CreateInstance<CardTest>();
//            currentCard.data = new CardDataTest[1];
//        }
//        public void SaveCard()
//        {
//            cardData = currentCard.data[0];
//            if (cardData != null)
//            {
//                AssetDatabase.CreateAsset(currentCard, "Assets/Resources/Card/" + cardData.name + ".asset");
//                AssetDatabase.SaveAssets();
//                AssetDatabase.Refresh();
//                ResetCardNData();
//            }
//            else
//            {
//                Debug.LogError("CardDATA IS NULL");
//            }
//        }
//        public void DeleteCard()
//        {
//            AssetDatabase.DeleteAsset("Assets/Resources/Card/" + currentCard.data[0].name + ".asset");
//            AssetDatabase.Refresh();
//            Debug.Log("CardDeleted");
//            ClearCard();
//        }

//        public void ClearCard()
//        {
//            cardData = null;
//            currentCard = null;
//        }

//        public void SearchCard(string str)
//        {

//            CardTest c = Resources.Load<CardTest>("Card/" + str);
//            if (c != null)
//            {
//                currentCard = c;
//                cardData = c.data[0];
//            }
//            else
//            {
//                Debug.LogWarning("FailToSearch " + str);
//            }
//        }
//    }


//}