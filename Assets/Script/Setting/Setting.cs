using GH.GameCard;
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

    
        public static void DropCreatureCard(Transform cardTransform, Transform fieldTransform, Card card)
        {
            Debug.LogFormat("DropCreatureCard: Field Transform is {0}", fieldTransform);
            SetParentForCard(cardTransform, fieldTransform);
            card.Instance.CanUseByViz(false);
            card.Instance.SetAttackable(false);
            card.Instance.gameObject.SetActive(true);
            card.Instance.SetOriginFieldLocation(fieldTransform.transform);
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

        /// <summary>
        /// Move 'c' to 'p'
        /// </summary>
        /// <param name="c">Moving Transform</param>
        /// <param name="p">target transform</param>
        public static void SetParentForCard(Transform c, Transform p)
        {
            c.SetParent(p);
            if (p.name != "BattleLine")
                c.rotation = p.rotation;
            c.localPosition = Vector3.zero;
            c.localScale = p.localScale;

        }


        /// <summary>
        /// This is for placing defending cards to attacking card
        /// </summary>
        /// <param name="defendCard"></param>
        /// <param name="attackingCard"></param>
        /// <param name="newLocalPosition"></param>
        /// <param name="newEuler"></param>
        public static void SetParentForCard(Transform defendCard, Transform attackingCard, Vector3 newLocalPosition, Transform defendLine)
        {
            defendCard.SetParent(defendLine);
            defendCard.rotation = attackingCard.rotation;
            defendCard.position = newLocalPosition;
            defendCard.localScale = attackingCard.localScale;
        }

       public static void SetCardsForAttack(Transform card, Transform battleLine)
        {
            Debug.LogFormat("SetCardsForAttack: {0} is moved to {1}", card.name, battleLine.name); 
            card.SetParent(battleLine);
            card.position = new Vector3(card.position.x, card.position.y, card.position.z +  ((battleLine.position.z - card.position.z)*0.7f));
            card.localScale = battleLine.localScale;
        }
        /// <summary>
        /// This function only changes gameobject's location.
        /// Move 'defendingCard' position to near 'attackingCard'.
        /// </summary>
        /// <param name="defendCard"></param>
        /// <param name="attackingCard"></param>
        /// <param name="count"></param>
        public static void SetCardsForBlock(CardInstance defendCardInst, CardInstance attackCardInst, int count)
        {
            Transform defendCardTransform = defendCardInst.transform;
            Transform attackCardTransform = attackCardInst.transform;
            Vector3 blockPosition = attackCardTransform.position;
            Transform defendLine = defendCardInst.owner._CardHolder.defenceLine.value;
            blockPosition.x += 3 * count;
            //blockPosition.y -= 0.2f;
            blockPosition.z-= 3;
            Debug.LogFormat("Attacking card transform: {0}, Defending card transform: {1}, Blocking position: {2}"
                , attackCardTransform.position, defendCardTransform.position, blockPosition);
            SetParentForCard(defendCardTransform, attackCardTransform, blockPosition, defendLine);
        }
    }

}

