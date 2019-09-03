using GH.GameCard;
using GH.GameElements;
using UnityEngine;


using GH.Multiplay;
using GH.GameCard.CardInfo;
using GH.GameCard.CardLogics;

namespace GH
{
    public class Setting : MonoBehaviour
    {
        private static ResourceManager resmanager;
        private static ConsoleHook _consoleHook;
        public static GameController gameController;
        public readonly static CardPlayManager graveLogic;


        /// <summary>
        /// Raycast card except 'currentSelecting'
        /// </summary>
        /// <param name="currentSelecting"></param>
        /// <returns></returns>
        public static PhysicalAttribute RayCastCard(PhysicalAttribute currentSelecting =null)         
        {
            RaycastHit[] hits = GetUIObjs();
            PhysicalAttribute detectedCard = null;
            for (int i = 0; i < hits.Length; i++)
            {
                detectedCard = hits[i].transform.gameObject.GetComponentInParent<PhysicalAttribute>();
                if (currentSelecting != null)
                {
                    if (detectedCard == currentSelecting)
                    {
                        detectedCard = null;
                    }
                }
                //If dectected card isn't current card
                else
                {
                    if (detectedCard != null)
                    {
                        //Debug.LogFormat("BlockCard: AttackingCard Found, {0}", detectedCard.OriginCard.Data.Name);
                        return detectedCard;                    //If there is card instance in 'currentCard', break
                    }

                } 
            }
            return detectedCard;
        }
        public static Area RayCastArea()
        {
            RaycastHit[] hits = GetUIObjs();
            Area a = null;
            for (int i = 0; i < hits.Length; i++)
            {
                a = hits[i].transform.gameObject.GetComponentInParent<GameElements.Area>();
                if (a != null)
                {
                    return a;
                }
            }
            return a;

        }
        public static RaycastHit[] GetUIObjs()
        {
            Camera mainCam = Camera.main;
            Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] ray = Physics.RaycastAll(cameraRay, 100.0f);

            return ray;
        }
        public static ResourceManager GetResourceManager()
        {
            if(resmanager == null)
            {
                resmanager = Resources.Load("ResourceManager") as ResourceManager;
                resmanager.Init();
            }
            return resmanager;
        }      
        public static void RegisterLog(string s, Color c)
        {            
            if (_consoleHook == null)
            {
                _consoleHook = Resources.Load("ConsoleHook") as ConsoleHook;
            }

            _consoleHook.RegisterEvent(s, c);
        }
    }

}

