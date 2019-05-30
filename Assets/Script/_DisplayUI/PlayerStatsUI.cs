using UnityEngine;
using UnityEngine.UI;
using System.Collections;
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

        private void Start()
        {
            if (!avatarObj.GetComponent<SpriteRenderer>())
            {
                Debug.Log("Avatar sprite component is needed on " + player);
                return;
            }
            player.statsUI = this;
            UpdateAll();

        }
        public void UpdateUserInfo()
        {
            userID.text = player.userID;
            avatarObj.GetComponent<SpriteRenderer>().sprite = player.playerAvatar;

        }
        public void UpdateHealthUI()
        {
            health.text = player.health.ToString();
        }        
        public void UpdateAll()
        {
            UpdateUserInfo();
            UpdateHealthUI();
        }
        public void UpdateManaUI()
        {
            manaCurrent.text = player.manaResourceManager.GetCurrentMana().ToString();
            manaMax.text = player.manaResourceManager.GetMaxMana().ToString();
        }

    }
}
