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
            UpdateAll();
            player.statsUI = this;

        }

        public void UpdateUserInfo()
        {
            userID.text = player.userID;
            avatarObj.GetComponent<SpriteRenderer>().sprite = player.playerAvatar;

        }

        public void UpdateHealth()
        {
            health.text = player.health.ToString();
        }
        
        public void UpdateAll()
        {
            UpdateUserInfo();
            UpdateHealth();
        }


        public void UpdateMana()
        {
            manaCurrent.text = player.manaResourceManager.GetCurrentMana().ToString();
            manaMax.text = player.manaResourceManager.GetMaxMana().ToString();
        }

    }
}
