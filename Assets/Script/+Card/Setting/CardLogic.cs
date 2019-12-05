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
        private CardSyncManager cardPlayManager = CardSyncManager.singleton;

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
            
            if (def.UseCard() == false)     //If defend card can't be used, can't be blocked
                return false; 
            else
                attackInst = Setting.RayCastCard(def.PhysicalCondition);
            if (attackInst!=null)
            {
                atkCard = (CreatureCard)attackInst.OriginCard;
                if (!error.CheckAttackingCard(def, atkCard))
                    ret = false;
                Debug.LogFormat("AttackingCard: {0}, DefendingCard: {1}", atkCard.GetCardData.Name, def.GetCardData.Name);
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

        public void UseSpell(SpellCard spell)
        {

            //Check if this card is available. If it can't be used, return
            if(!spell.CanUseCard())
            {
                return;
            }
            CreatureCard targetCard = null;
            //Ray cast target Instance
            PhysicalAttribute targetInst = Setting.RayCastCard(spell.PhysicalCondition);
            if (targetInst)
            {
                if (targetInst.OriginCard is CreatureCard)
                    targetCard = (CreatureCard)targetInst.OriginCard;
            }
            else
                return;

            //Based on Spell Card type, run it's spell to card.
            //How to build that logic?
            
            switch(spell.GetType)
            {
                //Modify Target Card stats
                case SpellType.Attack:
                    targetCard.CreatureData.ModifyHealth = -(spell._HealthChange);
                    break;
                case SpellType.Debuff:
                    targetCard.CreatureData.ModifyAttack = -(spell._AttackChange);
                    targetCard.CreatureData.ModifyHealth = -(spell._HealthChange);
                    break;
                case SpellType.Buff:
                    targetCard.CreatureData.ModifyAttack =  spell._AttackChange;
                    targetCard.CreatureData.ModifyHealth =  spell._HealthChange;
                    break;
                default:
                    Debug.LogError("This Spell don't have any type");
                    break;
            }


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