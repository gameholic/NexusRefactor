using GH.GameCard.CardInfo;
using GH.GameElements;
using GH.Player;
using UnityEngine;
using GH.Multiplay;
using GH.GameCard.ErrorCheck;

namespace GH.GameCard.CardLogics
{
    public class CardLogic               //Check errors and send connects to multiplay manager
    {
        private GameController gc = Setting.gameController;
        private ErrorCheck_Creature error = new ErrorCheck_Creature();
        private CardPlayManager cardPlayManager = CardPlayManager.singleton;

        /// <summary>
        /// Block attacking card using selected card(def)
        /// </summary>
        /// <param name="def"></param>
        public bool BlockCard(CreatureCard def)
        {
            Debug.LogFormat("Block Card With {0}",def);
            bool ret = true;
            CreatureCard atkCard = null;
            PhysicalAttribute attackInst = null;



            if (!def.UseCard())
                ret = false;
            attackInst = Setting.RayCastCard(def.PhysicalCondition);
            if (attackInst!=null)
            {
                atkCard = (CreatureCard)attackInst.OriginCard;
                if (!error.CheckAttackingCard(def, atkCard))
                    ret = false;
                Debug.LogFormat("AttackingCard: {0}, DefendingCard: {1}", atkCard.Data.Name, def.Data.Name);
            }
            return ret;
        }
        private CreatureCard tmp = new CreatureCard();
        private CreatureCard SaveAttackingCard
        {
            set { tmp = value; }
        }
        public CreatureCard GetAttackingCard { get { return tmp; } }
        public bool DropCard(CreatureCard targetCard)
        {
            PlayerHolder p = gc.CurrentPlayer;

            if (!targetCard.CanDropCard())
            {
                Debug.LogError("TargetCan'tDropCard");
                return false;

            }
            Debug.LogWarning("Released");
            return true;
        }

        public void UseSpell()
        {

        }
        public void SetToAttack(CreatureCard attackCard)
        {
            if (!error.CheckCanAttack(attackCard))
                return;
            cardPlayManager.CardPlayAttack(attackCard);
        }
        public void CardTracking(Card currentCard)
        {
            //Debug.LogFormat("currentCard : {0} / GameObjecT:{1} ", currentCard,  currentCard.PhysicalCondition.gameObject);

            currentCard.PhysicalCondition.transform.position
                = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 50));
            currentCard.PhysicalCondition.transform.SetAsLastSibling();
        }
        public void ReturnToOldPos(Card c)
        {
            c.PhysicalCondition.transform.position = c.PhysicalCondition.OldPos;
        }
    }

}