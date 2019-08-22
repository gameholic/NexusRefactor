using GH.GameCard;
using GH.GameCard.CardInfo;
using GH.GameCard.CardLogics;
using GH.GameTurn;
using GH.Player;
using UnityEngine;

namespace GH.MouseLogics
{
    public enum HandleLogics { Drop, Block, Battle, UseMagic }
    public class MouseOperation : MonoBehaviour
    {
        private bool dragging = false;
        private PhysicalAttribute selectedCardInst = null;
        private void Update()
        {   
            HandleMouse();
        }
        private void HandleMouse()              //Detect Mouse 
        {
            bool isMouseDown = Input.GetMouseButton(0);
            CardUseBridge bridge = new CardUseBridge();
            if (!isMouseDown)       //Mouse is relased
            {
                if(dragging)
                {
                    dragging = false;
                    bridge.CardReturn(selectedCardInst.OriginCard);
                    Debug.Log("Return card");
                }
                else
                    Debug.Log("Dragging is false");

                Debug.Log("CardDetecting");
                HandleCardDetection();
            }
            if(isMouseDown)             //Mouse is Pressed
            {
                if (selectedCardInst != null)
                {
                    if (!dragging)
                        selectedCardInst.OldPos = selectedCardInst.transform.position;
                    Debug.Log("CardDragging");
                    bridge.CheckBlockCard((CreatureCard)selectedCardInst.OriginCard);
                    HandleMouseClick(selectedCardInst);
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
            Debug.LogFormat("Current Card is {0}", selectedCardInst);
            if (IsAtHand && currentPhase is ControlPhase)
            {
                if(c is CreatureCard)
                    CardDrag(HandleLogics.Drop, inst);      //Drag creature to field
                else
                    CardDrag(HandleLogics.UseMagic, inst);  //Drag Magic to use
            }
            if(currentPhase is BlockPhase)
                    CardDrag(HandleLogics.Block, inst);     //Drag this card to attacking card to block          
            if (currentPhase is BattlePhase)
                    CardClick(HandleLogics.Battle, inst);   //Click card on field to attack
        }


        private void CardDrag(HandleLogics logic, PhysicalAttribute c)
        {
            CardLogic cardLogic = new CardLogic();
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
        private void CardClick(HandleLogics logic, PhysicalAttribute c)
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
                if (selectedCardInst != null)
                {
                    selectedCardInst.DeHighlight();
                }
                selectedCardInst = detectedCard;
                selectedCardInst.Highlight();
            }
            else
            {
                if (selectedCardInst != null)
                {
                    selectedCardInst.DeHighlight();
                    selectedCardInst = null;
                }
            }

        }
    }


}