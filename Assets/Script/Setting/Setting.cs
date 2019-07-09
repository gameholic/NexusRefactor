﻿using GH.GameCard;
using UnityEngine;

namespace GH
{
    public class Setting : MonoBehaviour
    {
        private static ResourceManager resmanager;
        private static ConsoleHook _consoleHook;
        public static GameController gameController;

        public static ResourceManager GetResourceManager()
        {
            if(resmanager == null)
            {
                resmanager = Resources.Load("ResourceManager") as ResourceManager;
                resmanager.Init();
            }
            return resmanager;
        }
        
        public static RaycastHit[] GetUIObjs()
        {
            Camera mainCam = Camera.main;
            Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] ray = Physics.RaycastAll(cameraRay, 100.0f);
            
            return ray;
        }

    
        public static void DropCreatureCard(Transform cardTransform, Transform destTransform, Card card)
        {
            SetParentForCard(cardTransform, destTransform);
            card.Instance.CanUseViz(false);
            card.Instance.SetCanAttack(false);
            card.Instance.gameObject.SetActive(true);
            gameController.CurrentPlayer.PayMana(card);
            gameController.CurrentPlayer.DropCardOnField(card.Instance);

        }

        public static void RegisterLog(string s, Color c)
        {
            
            if (_consoleHook == null)
            {
                _consoleHook = Resources.Load("ConsoleHook") as ConsoleHook;
            }

            _consoleHook.RegisterEvent(s, c);
        }


        public static void SetParentForCard(Transform c, Transform p)
        /// Move card 'c' to 'p'
        {
            c.SetParent(p);
            c.rotation = p.rotation;
            c.localPosition = Vector3.zero;
            c.localScale = p.localScale;
        }

        public static void SetParentForCard(Transform c, Transform p, Vector3 newLocalPosition, Vector3 newEuler)
        /// Move card 'c' to 'p'
        {
            c.SetParent(p.parent);
            //c.rotation = Quaternion.Euler(newEuler);
            c.rotation = p.rotation;
            c.localPosition = newLocalPosition;
            c.localScale = p.localScale;
        }

       
        public static void SetCardsForBlock(Transform defendCard, Transform attackingCard, int count)
        {
            //Change numbers that looks good to player
            Vector3 blockPosition = Vector3.zero;
            blockPosition.x += 2 * count;
            blockPosition.y -= 2;
            SetParentForCard(defendCard, attackingCard, blockPosition, Vector3.zero);
            Debug.Log("SetCardForBlock: Works well");
        }
    }

}

