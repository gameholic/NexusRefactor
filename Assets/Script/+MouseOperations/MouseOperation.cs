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
    public enum MouseState { Holding,Releasing}
    public class MouseOperation : MonoBehaviour
    {
        private MouseState mouseState;
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
            HandleCardDetection();

            if (selectedCard == null)
                return;

            Phase currentPhase = Setting.gameController.CurrentPhase;
            if (isMouseDown)       //Mouse is Pressed
            {
                if (mouseState == MouseState.Releasing)
                    selectedCard.OldPos = selectedCard.transform.position;
                else if (mouseState == MouseState.Holding)
                    HandleMouseClick(selectedCard, currentPhase); // Start holding card
                Debug.Log("HoldingCard");
                mouseState = MouseState.Holding;
            }

            if (!isMouseDown)       //Mouse is relased
            {
                if (mouseState == MouseState.Holding) 
                    // Below codes handle when mouse was holding or clicking something.
                {
                    bool canDropCard = false;
                    if (!(selectedCard.OriginCard is CreatureCard))
                        return;
                    Card c = selectedCard.OriginCard;
                    canDropCard = DropAfterDrag(currentPhase.GetPhaseId, c);

                    if (canDropCard == false)
                    {
                        cardLogic.ReturnToOldPos(c);        //This should work when it failed to defend or drop                    
                        Debug.Log("Return card");
                    }
                                       
                }
                mouseState = MouseState.Releasing;
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
            int uniqueId = c.GetCardData.UniqueId;
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
        /// Drop Creature Card On Field. 
        /// Navigate Card Instance to appropriate logics.
        /// </summary>
        /// <param name="logic"></param>
        /// <param name="target"></param>
        private bool DropAfterDrag(PhaseId logic, Card target)
        {
            SpellCard spell = null;
            CreatureCard creature = null;
            CardLogic cardLogic = new CardLogic();
            CardSyncManager cardPlayManager = CardSyncManager.singleton;
            bool ret = false;

            if (target is SpellCard)
                spell = (SpellCard)target;
            else if (target is CreatureCard)
                creature = (CreatureCard)target;

            if (logic == PhaseId.Control)
            {
                if(spell)
                {
                    cardLogic.UseSpell(spell);
                }   
                else if(creature)
                {
                    ret = cardLogic.DropCard(creature);
                    if (ret)
                    {
                        Area a = target.PhysicalCondition.GetOriginFieldLocation().GetComponent<Area>();
                        cardPlayManager.CardPlayDrop(creature, a);
                    }
                }
            }
            else if (logic == PhaseId.Block)
            {
                ret= cardLogic.BlockCard(creature);
                if(ret)
                {
                    CreatureCard atkCard = cardLogic.GetAttackingCard;
                    cardPlayManager.CardPlayBlock(creature, atkCard);                    
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