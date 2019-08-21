using UnityEngine;

using GH.GameCard.CardInfo;
using GH.GameCard;
using GH.GameCard.CardLogics;
using GH.GameTurn;
using GH.Player;

namespace GH.MouseLogics
{
    public enum HandleLogics { Drop, Block, Battle, UseMagic }
    public class MouseOperation : MonoBehaviour
    {
        private bool dragging = false;
        private PhysicalAttribute currentCard = null;
        private void Update()
        {   
            HandleMouse();
        }
        private void HandleMouse()              //Detect Mouse 
        {
            bool isMouseDown = Input.GetMouseButton(0);
            CardUseBridge l = new CardUseBridge();
            if (!isMouseDown)       //Mouse is relased
            {
                if(dragging)
                {
                    dragging = false;
                    l.CardReturn(currentCard.OriginCard);
                    Debug.Log("Return card");
                }
                else
                {
                    Debug.Log("Dragging is false");
                }
                Debug.Log("CardDetecting");
                HandleCardDetection();
            }
            if(isMouseDown)             //Mouse is Pressed
            {
                if (currentCard != null)
                {
                    if (!dragging)
                        currentCard.OldPos = currentCard.transform.position;

                    Debug.Log("CardDragging");
                    l.BlockCard((CreatureCard)currentCard.OriginCard);
                    //HandleMouseClick(currentCard);


                    dragging = true;
                    Debug.Log(dragging);
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
            Debug.LogFormat("Current Card is {0}", currentCard);
            if (IsAtHand && currentPhase is ControlPhase)
            {
                if(c is CreatureCard)
                    CardDrag(HandleLogics.Drop);      //Drag creature to field
                else
                    CardDrag(HandleLogics.UseMagic);  //Drag Magic to use
            }
            if(currentPhase is BlockPhase)
                    CardDrag(HandleLogics.Block);     //Drag this card to attacking card to block          
            if (currentPhase is BattlePhase)
                    CardClick(HandleLogics.Battle);   //Click card on field to attack
        }
        private void CardDrag(HandleLogics logic)
        {
            if(logic == HandleLogics.Drop)
            {
            }
            if (logic == HandleLogics.UseMagic)
            {

            }
            if (logic == HandleLogics.Block)
            {

            }
        }
        /// <summary>
        /// Check current phase/state and card's position and perform mouse drag.
        /// </summary>
        private void CardClick(HandleLogics logic)
        {
            bool isMouseDown = Input.GetMouseButtonDown(0);
            if (!isMouseDown)
            {
                if (currentCard != null)   
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
                if (currentCard != null)
                {
                    currentCard.DeHighlight();
                }
                currentCard = detectedCard;
                currentCard.Highlight();
            }
            else
            {
                if (currentCard != null)
                {
                    currentCard.DeHighlight();
                    currentCard = null;
                }
            }

        }
    }


}