using UnityEngine;

using GH.GameCard;
using GH.GameCard.CardInfo;
using GH.GameTurn;
using GH.Player;
using System;

namespace GH.Nexus.Manager
{
    public class TestManager : MonoBehaviour
    {
        public Card[] targetCard;

        [SerializeField]
        private GameObject _CardPrefab;
        [SerializeField]
        private GameObject _Hand;
        public Phase controlPhase;
        int count = 0;
        private void Start()
        {
            for( count = 0; count < targetCard.Length; count++)
            {
                targetCard[count] = Instantiate(targetCard[count]);
                targetCard[count].Data.TestSetUniqueId = count;
                LoadCard(targetCard[count]);
            }
            InitForTest();
        }
        private void InitForTest()
        {
            PlayerHolder currentPlayer = Setting.gameController.LocalPlayer;
            Setting.gameController.CurrentPhase = controlPhase;
            currentPlayer.InGameData.Init(Setting.gameController.LocalPlayer);
            currentPlayer.InGameData.ManaManager.MaxMana = 10;
            currentPlayer.InGameData.ManaManager.CurrentMana = 10;
        }
        public void LoadCard(Card c)         //Needed when loading card
        {
            GameObject go = Instantiate(_CardPrefab) as GameObject;
            CardAppearance v = go.GetComponent<CardAppearance>();
            if(v!=null)
            {
                v.LoadCard(c, go);
                c.Init(go);
                c.User = Setting.gameController.LocalPlayer;
                c.User.CardManager.handCards.Add(c.Data.UniqueId);
                go.transform.SetParent(_Hand.transform);
                go.transform.localPosition = Vector3.zero;
                go.transform.localScale = Vector3.one;


            }
        }

        static string log = null;
        public void WriteLog()
        {
            log = log + "WritingLog\n";
            Debug.Log(log);
        }
        public void UpdateLog()
        {
            log = FileBridge.LoadLog() + "*********\nUpdateLog\n"+log;
            Debug.Log(log);
            FileBridge.SaveLogFile(log);
        }
        public void LoadLog()
        {
            string load = FileBridge.LoadLog();
            Debug.Log(load);
        }
    }
}
