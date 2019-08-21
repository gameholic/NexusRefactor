using GH.Multiplay;
using UnityEngine;
using GH.GameCard.CardInfo;
using GH.GameElements;
using GH.Player;

namespace GH.GameCard.CardLogics
{
    public class CardUseBridge                //Check errors and send connects to multiplay manager
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
            if (true)//if(def.UseCard())
            {
                inst = Setting.RayCastCard(def.PhysicalCondition);        
                if(inst!=null)
                {
                    atkCard = (CreatureCard)inst.OriginCard;
                    if (atkCard.Data.Name == def.Data.Name)
                    {
                        Debug.LogError("defending == attacking");
                    }
                    /*if (!error.IsAttacking(atkCard))
                    {
                        return;
                    }*/
                    //MultiplayManager.singleton.PlayerBlocksTargetCard();
                    Debug.LogFormat("AttackingCard: {0}, DefendingCard: {1}", atkCard.Data.Name, def.Data.Name);
                    int count = 0;
                    
                }    
            }
        }
        public void DropCard(CreatureCard targetCard)
        {
            Area a = Setting.RayCastArea();
            Player.PlayerHolder p = Setting.gameController.CurrentPlayer;

            //TODO:
            //1. Error check
            //2. Link to Multiplayer
            //

        }
        public void UseMagic()
        {

        }
        public void SendToBattle(CreatureCard c)
        {

        }
        private void CardTracking(Card currentCard)
        {
            currentCard.PhysicalCondition.transform.position 
                = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
            currentCard.PhysicalCondition.transform.SetAsLastSibling();
        }
        public void CardReturn(Card c)
        {
            c.PhysicalCondition.transform.position = c.PhysicalCondition.OldPos;
        }
    }

}