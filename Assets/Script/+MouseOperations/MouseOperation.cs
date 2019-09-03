using GH.GameCard;
using GH.GameCard.CardInfo;
using GH.GameCard.CardLogics;
using GH.GameElements;
using GH.GameTurn;
using GH.Multiplay;
using GH.Player;
using UnityEngine;

namespace GH.MouseLogics
{
    public class MouseOperation : MonoBehaviour
    {
        private bool dragging = false;
        private PhysicalAttribute selectedCard = null;
        private void Awake()
        {
            DontDestroyOnLoad(this);
        }
        private void Update()
        {
            HandleMouse();
        }

        private void HandleMouse()              //Detect Mouse 
        {
            bool isMouseDown = Input.GetMouseButton(0);
            CardLogic cardLogic = new CardLogic();

            Phase currentPhase = Setting.gameController.CurrentPhase;
            if (isMouseDown)       //Mouse is Pressed
            {
                if (selectedCard == null)
                    return;

                if (!dragging)
                    selectedCard.OldPos = selectedCard.transform.position;
                else
                    HandleMouseClick(selectedCard, currentPhase); // Start holding card

                Debug.Log("HoldingCard");
                dragging = true;
                
            }
            if (!isMouseDown)       //Mouse is relased
            {
                if (dragging && selectedCard != null)
                {
                    if (!(selectedCard.OriginCard is CreatureCard))
                        return;
                    CreatureCard c = (CreatureCard)selectedCard.OriginCard;
                    bool ret = false;
                    ret = DropAfterDrag(currentPhase.GetPhaseId, c);
                    if (ret == false)
                        cardLogic.ReturnToOldPos(c);        //This should work when it failed to defend or drop
                    dragging = false;
                    Debug.Log("Return card");
                    
                }
                HandleCardDetection();
            }

        }

        private void HandleMouseClick(PhysicalAttribute inst, Phase currentPhase)
        {
            Card c = inst.OriginCard;

            Debug.LogFormat("Current Card is {0}", selectedCard);
            if (c is CreatureCard)
            {
                CreatureCard converted = (CreatureCard)c;

                HandleCreatureClick(converted, currentPhase);
            }
            else if (c is SpellCard)
            {
                SpellCard converted = (SpellCard)c;
                HandleSpellClick(converted, currentPhase);
            }
            //if (Setting.gameController.CurrentPlayer !=c.User)
            //{
            //    Debug.LogError("Current Player Isn't The Card Owner");
            //    return;
            //}
            //if (IsAtHand && currentPhase is ControlPhase)       //Drop CreatureCard 
            //{
            //        CardDrag(HandleLogics.Drop, c);      //Drag creature to field
            //}
            //else if (currentPhase is BattlePhase)
            //{
            //    if (c is CreatureCard)
            //        CardClick(HandleLogics.Battle, (CreatureCard)c);   //Click card on field to attack
            //    else
            //        CardDrag(HandleLogics.Spell, c);  //Drag Magic to use
            //}      
            //else if (currentPhase is BlockPhase)
            //   CardDrag(HandleLogics.Block, c);     //Drag this card to attacking card to block          
            //else
            //{
            //    Debug.LogError("NonOfThoseWorked");
            //}  
            return;
        }
        public void HandleSpellClick(SpellCard c, Phase currentPhase)
        {
            switch(currentPhase.GetPhaseId)
            {
                case PhaseId.Battle:
                    //Can use QuickAttack cards 
                    break;
                case PhaseId.Block:
                    //Can use QuickDefend cards
                    break;
                case PhaseId.Control:
                    //Can Use  all cards
                    break;
            }

        }
        public void HandleCreatureClick(CreatureCard c, Phase currentPhase)
        {
            int uniqueId = c.Data.UniqueId;
            if(currentPhase.GetPhaseId == PhaseId.Battle)
            {
                //Click To Attack
            }
            else
            {
                DragCard(c);
                //draggggg
                //When Phase that cards need to be dragged, Control Phase to drop and Block Phase to defend, is handled after holding is done
            }
        }
        private void DragCard(Card c)
        {
            CardLogic cardLogic = new CardLogic();
            Card_handOrder handOrder = null;
            if (c.User == Setting.gameController.LocalPlayer)
                handOrder = handObj[0].GetComponent<Card_handOrder>();
            else
                handOrder = handObj[1].GetComponent<Card_handOrder>();

            handOrder.enabled = false;
            cardLogic.CardTracking(c);
            handOrder.enabled = true;

        }
        [SerializeField]
        private GameObject[] handObj = new GameObject[2];

        /// <summary>
        /// Navigate Card Instance to appropriate logics.
        /// Check If This is Creature
        /// </summary>
        /// <param name="logic"></param>
        /// <param name="target"></param>
        private bool DropAfterDrag(PhaseId logic, CreatureCard target)
        {
            SpellCard spell = null;
            CardLogic cardLogic = new CardLogic();
            CardPlayManager cardPlayManager = CardPlayManager.singleton;
            bool ret = false;

            if (logic == PhaseId.Control)
            {
                ret = cardLogic.DropCard(target);
                if(ret)
                {
                    Area a = target.PhysicalCondition.GetOriginFieldLocation().GetComponent<Area>();
                    cardPlayManager.CardPlayDrop(target, a);                    
                }
            }
            else if (logic == PhaseId.Block)
            {
                ret= cardLogic.BlockCard(target);
                if(ret)
                {
                    CreatureCard atkCard = cardLogic.GetAttackingCard;
                    cardPlayManager.CardPlayBlock(target, atkCard);                    
                }

            }

            return ret;
        }
        /// <summary>
        /// Check current phase/state and card's position and perform mouse drag.
        /// </summary>
        private void CardClick(PhaseId logic, CreatureCard c)
        {
            CardLogic cardLogic = new CardLogic();
            bool isMouseDown = Input.GetMouseButtonDown(0);
            if (!isMouseDown)
            {
                if (c != null)   
                {
                    cardLogic.SetToAttack(c);
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