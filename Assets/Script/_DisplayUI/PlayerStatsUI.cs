using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using GH.Player;
namespace GH
{
    public class PlayerStatsUI : MonoBehaviour
    {
        public PlayerHolder player;

        /// <summary>
        /// Should below variables be public? why not private? try to modify later
        /// </summary>
        public Image avatarImage;
        public Text health;
        public Text userID;
        public Text manaCurrent;
        public Text manaMax;
        public void Init()
        {
            player.InGameData.StatsUI = this;
            UpdateAll();
        }
        public void UpdateUserInfo()
        {
            userID.text = player.PlayerProfile.UniqueId;            
            avatarImage.sprite = player.PlayerProfile.PlayerAvatar;   //  This needs to be deleted.
            Debug.Log("Update User Info");

        }
        public void UpdateHealthUI()
        {
            health.text = player.InGameData.Health.ToString();
        }                   
        public void UpdateAll()
        {
            UpdateUserInfo();
            UpdateHealthUI();
        }
        public void UpdateManaUI()
        {
            manaCurrent.text = player.InGameData.ManaManager.CurrentMana.ToString();
            manaMax.text = player.InGameData.ManaManager.MaxMana.ToString();
        }
    }
}
