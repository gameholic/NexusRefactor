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
        public GameObject avatarObj;
        public TextMesh health;
        public TextMesh userID;
        public TextMesh manaCurrent;
        public TextMesh manaMax;
        public void Init()
        {
            player.InGameData.StatsUI = this;
            UpdateAll();
        }
        public void UpdateUserInfo()
        {
            userID.text = player.PlayerProfile.UniqueId;            
            avatarObj.GetComponent<SpriteRenderer>().sprite = player.PlayerProfile.PlayerAvatar;   //  This needs to be deleted.

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
