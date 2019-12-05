#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using GH.GameCard;
using GH.GameCard.CardInfo;
using System.Collections.Generic;

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
                if (GUILayout.Button("Add Card"))
                {
                    
                    addingMode = true;
                }


                if (GUILayout.Button("DeleteCard"))
                {
                    subWindow.DeleteCard();
                }
            }
            else if (addingMode)
            {
                if (GUILayout.Button("AddCreature"))
                {
                    CreatureCard c = new CreatureCard();
                    subWindow.ResetCardNData<CreatureCard>(c);
                }
                if (GUILayout.Button("AddSpellCard"))
                {
                    SpellCard c = new SpellCard();
                    subWindow.ResetCardNData<SpellCard>(c);
                }
                if (GUILayout.Button("AddWeaponCard"))
                {
                    WeaponCard c = new WeaponCard();
                    subWindow.ResetCardNData<WeaponCard>(c);
                }
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
        }
    }


    public class SubCardWindow : CardEditor
    {
        bool initialised = false;

        SerializedObject serializedObject;
        SerializedProperty cardProperty;
        SerializedProperty uniqueProperty;
        SerializedProperty abilityProperty;
        Card currentCard = null;
        CardData cardData = null; 


        public void ShowCard()
        {
            if (!initialised)
            {
                serializedObject = new SerializedObject(currentCard);
                cardProperty = serializedObject.FindProperty("_CardData");
                uniqueProperty = serializedObject.FindProperty("_UniqueData");
                abilityProperty = serializedObject.FindProperty("_Ability");
                initialised = true;
            }
            EditorGUILayout.PropertyField(cardProperty, true);
            EditorGUILayout.PropertyField(uniqueProperty, true);
            EditorGUILayout.PropertyField(abilityProperty, true);
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
        public void ResetCardNData<T> (T card) where T : Card
        {
            cardData = new CardData();
            currentCard = null;
            if (card is CreatureCard)
            {
                CreatureData creatureData = new CreatureData();
            }
            else if (card is SpellCard)
            {
                SpellData spellData = new SpellData();
            }
            else if (card is WeaponCard)
            {
                WeaponData weponData = new WeaponData();
            }

            currentCard = CreateInstance<T>();
            currentCard.SetCardData(cardData);
        }
        public void SaveCard()
        {
            cardData = currentCard.GetCardData;
            if (cardData != null)
            {
                Debug.Log(cardData.Name);
                //Check if there is directory folder for card. And If there isn't send warning and make new folder
                AssetDatabase.CreateAsset(currentCard, "Assets/CardResources/" + cardData.Name + ".asset");
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();

                //ResetCardNData<Card>();
            }
            else
            {
                Debug.LogError("CardDATA IS NULL");
            }
        }
        public void DeleteCard()
        {
            AssetDatabase.DeleteAsset("Assets/Resources/Card/" + currentCard.GetCardData.Name + ".asset");
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
                cardData = c.GetCardData;
            }
            else
            {
                Debug.LogWarning("FailToSearch " + str);
            }
        }
    }
}
#endif