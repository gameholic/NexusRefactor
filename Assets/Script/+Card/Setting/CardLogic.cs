using GH.GameCard.CardInfo;
using GH.GameElements;
using GH.Player;
using UnityEngine;

namespace GH.GameCard.CardLogics
{
    public class CardLogic                //Check errors and send connects to multiplay manager
    {
        private GameController gc = Setting.gameController;
        private ErrorCheck.ErrorCheck_Creature error = new ErrorCheck.ErrorCheck_Creature();


        /// <summary>
        /// Block attacking card using selected card(def)
        /// </summary>
        /// <param name="def"></param>
        public void BlockCard(CreatureCard def)
        {
            CardTracking(def);
            CreatureCard atkCard = null;
            PhysicalAttribute inst = null;
            if (!def.UseCard() || def  == null)
                return;
            inst = Setting.RayCastCard(def.PhysicalCondition);    
            
            if(inst!=null)
            {
                atkCard = (CreatureCard)inst.OriginCard;
                if (atkCard.Data.Name == def.Data.Name)
                    Debug.LogError("defending == attacking");
                else if (!error.IsAttacking(atkCard))
                    return;
                //MultiplayManager.singleton.PlayerBlocksTargetCard();
                Debug.LogFormat("AttackingCard: {0}, DefendingCard: {1}", atkCard.Data.Name, def.Data.Name);                    
            }    
        }        
        public void DropCard(CreatureCard targetCard)
        {
            Area a = Setting.RayCastArea();
            PlayerHolder p = gc.CurrentPlayer;

            CardTracking(targetCard);

            if (a == null)
                return;
            if (!error.CheckCanDrop(targetCard,a))
                return;
            //MultiplayManager.singleton.PlayerTryToUseCard();

        }
        public void UseSpell()
        {

        }
        public void SetToAttack(CreatureCard attackCard)
        {
            Transform battleLine = attackCard.User.CardTransform.AttackingLine.value;

            if (!error.CheckCanAttack(attackCard))
                return;
            //MultiplayManager.singleton.PlayerTryToUseCard();
        }
        private void CardTracking(Card currentCard)
        {
            currentCard.PhysicalCondition.transform.position 
                = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
            currentCard.PhysicalCondition.transform.SetAsLastSibling();
        }
        public void ReturnToOldPos(Card c)
        {
            c.PhysicalCondition.transform.position = c.PhysicalCondition.OldPos;
        }
    }

}