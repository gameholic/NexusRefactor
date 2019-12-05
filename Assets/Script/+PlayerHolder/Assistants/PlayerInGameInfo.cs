using GH.GameCard;
using System;
using UnityEngine;

namespace GH.Player.Assists
{
    [CreateAssetMenu(menuName ="PlayerData/IngameInfo")]
    public class PlayerInGameInfo : PlayerAssists
    {
        private ManaManager manaResourceManager;
        private PlayerStatsUI statsUI;
        [SerializeField]                        //Should Be Deleted
        private int _PhotonId = -1;

        private int health;
        public int PhotonId
        {
            get { return _PhotonId; }
        }
        public int SetPhotonId
        {
            set { _PhotonId = value; }
        }
        public PlayerHolder Player { get { return player; } }

        public int Health
        {
            set
            {
                health = value;
                statsUI.UpdateHealthUI();
            }
            get { return health; }
        }
        public PlayerStatsUI StatsUI
        {
            set { statsUI = value; }
            get { return statsUI; }
        }
        public ManaManager ManaManager
        {
            set { manaResourceManager = value; }
            get { return manaResourceManager; }
        }
        public override void Init(PlayerHolder p)
        {
            Debug.LogFormat("InGameInfo  Init");
            manaResourceManager = new ManaManager();
            for(int i=0;i<2;i++)
            {
                if (Setting.gameController.GetPlayer(i) == null)
                    break;
                if (p==Setting.gameController.GetPlayer(i))
                {
                    statsUI = Setting.gameController.GetPlayerUIInfo(i);
                }
            }
            LoadPlayerOnStatsUI();
            player = p;
            Health = 30;
        }


        public void DoDamage(int v)
        {
            //Debug.LogFormat("PlayerTookDamagE: {0} took {1} damage", this.player, v);
            health -= v;
            if (statsUI != null)
            {
                statsUI.UpdateHealthUI();
            }
        }
        public bool PayMana(Card c)
        {
            bool result = false;

            int currentMana = manaResourceManager.CurrentMana;
            if (c.GetCardData.ManaCost <= currentMana)
                result = true;
            else
                Setting.RegisterLog("Not Enough Mana", Color.black);

            return result;
        }
        public void LoadPlayerOnStatsUI()
        {
            if (statsUI != null)
            {
                //statsUI.player = this;
                statsUI.UpdateAll();
            }
            else
            {
                Debug.Log("StatsUI is null");
            }
        }
    }
}