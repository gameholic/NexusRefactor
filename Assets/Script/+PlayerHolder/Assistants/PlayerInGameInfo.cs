using GH.GameCard;
using UnityEngine;

namespace GH.Player.Assists
{

    public class PlayerInGameInfo : PlayerAssists
    {

        private NewPlayerHolder p;
        private ManaManager manaResourceManager;

        [SerializeField]
        private PlayerStatsUI statsUI;        
        [SerializeField]                                        //To check photon Id easily. This should be NonSerialized
        private int _PhotonId = -1;

        private int health;
        public int PhotonId
        {
            set { _PhotonId = value; }
            get { return _PhotonId; }
        }
        public int Health
        {
            set
            {
                health = value;
                statsUI.UpdateHealthUI();
            }
            get { return health; }
        }
        

        public int CurrentMana
        {
            set { manaResourceManager.UpdateCurrentMana(value); }
            get { return manaResourceManager.GetCurrentMana(); }
        }
        public override void Init(NewPlayerHolder p)
        {
            manaResourceManager = new ManaManager();
            LoadPlayerOnStatsUI();
            this.p = p;
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

            int currentMana = manaResourceManager.GetCurrentMana();
            if (c.CardCost <= currentMana)
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