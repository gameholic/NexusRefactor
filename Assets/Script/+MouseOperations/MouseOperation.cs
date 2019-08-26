using GH.GameCard;
using GH.GameCard.CardInfo;
using GH.GameCard.CardLogics;
using GH.GameTurn;
using GH.Player;
using UnityEngine;

namespace GH.MouseLogics
{
    public enum HandleLogics { Drop, Block, Battle, Spell }
    public class MouseOperation : MonoBehaviour
    {
        private bool dragging = false;
        private PhysicalAttribute selectedCard = null;
        private void Update()
        {   
            HandleMouse();
        }
        private void HandleMouse()              //Detect Mouse 
        {
            bool isMouseDown = Input.GetMouseButton(0);
            CardLogic cardLogic = new CardLogic();
            if (!isMouseDown)       //Mouse is relased
            {
                if(dragging)
                {
                    dragging = false;
                    cardLogic.ReturnToOldPos(selectedCard.OriginCard);
                    Debug.Log("Return card");
                }
                Debug.Log("CardDetecting");
                HandleCardDetection();
            }
            if(isMouseDown)             //Mouse is Pressed
            {
                if (selectedCard != null)
                {
                    if (!dragging)
                        selectedCard.OldPos = selectedCard.transform.position;
                    HandleMouseClick(selectedCard);
                    dragging = true;
                }
            }
        }

        private void HandleMouseClick(PhysicalAttribute inst)
        {
            Card c = inst.OriginCard;
            CardManager cardManager = c.User.CardManager;
            Phase currentPhase = Setting.gameController.CurrentPhase;

            int uniqueId = c.Data.UniqueId;
            bool IsAtHand = cardManager.handCards.Contains(uniqueId);
            //bool IsOnField = cardManager.handCards.Contains(uniqueId);
            Debug.LogFormat("Current Card is {0}", selectedCard);

            if (Setting.gameController.CurrentPlayer !=c.User)
            {
                Debug.LogError("Current Player Isn't The Card");
                return;
            }

            if (IsAtHand && currentPhase is ControlPhase)
            {
                if(c is CreatureCard)
                    CardDrag(HandleLogics.Drop, c);      //Drag creature to field
                else
                    CardDrag(HandleLogics.Spell, c);  //Drag Magic to use
            }
            if (currentPhase is BattlePhase)
                CardClick(HandleLogics.Battle, (CreatureCard)c);   //Click card on field to attack

            if (currentPhase is BlockPhase)
               CardDrag(HandleLogics.Block, c);     //Drag this card to attacking card to block          

            return;
        }

        /// <summary>
        /// Navigate Card Instance to appropriate logics.
        /// Check If This is Creature
        /// </summary>
        /// <param name="logic"></param>
        /// <param name="c"></param>
        private void CardDrag(HandleLogics logic, Card c)
        {
            CardLogic cardLogic = new CardLogic();
            CreatureCard creature = null;
            //SpellCard spell = null;
            if(c is CreatureCard)
            {
                creature = (CreatureCard)c;
                if (logic == HandleLogics.Drop)
                {
                    cardLogic.DropCard(creature);
                }
                if (logic == HandleLogics.Block)
                {
                    cardLogic.BlockCard(creature);
                }
            }
            if (logic == HandleLogics.Spell)
            {

            }
        }
        /// <summary>
        /// Check current phase/state and card's position and perform mouse drag.
        /// </summary>
        private void CardClick(HandleLogics logic, CreatureCard c)
        {
            bool isMouseDown = Input.GetMouseButtonDown(0);
            if (!isMouseDown)
            {
                if (c != null)   
                {


                    //TODO: Check current state / phase and perform action
                }
            }
        }
        private void HandleCardDetection()
        {
            PhysicalAttribute detectedCard = Setting.RayCastCard();

            if (detectedCard != null)
            {
                if (selectedCard != null)
                {
                    selectedCard.DeHighlight();
                }
                selectedCard = detectedCard;
                selectedCard.Highlight();
            }
            else
            {
                if (selectedCard != null)
                {
                    selectedCard.DeHighlight();
                    selectedCard = null;
                }
            }

        }
    }


}