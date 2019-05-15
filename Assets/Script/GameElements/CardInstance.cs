using UnityEngine;
using System.Collections;
using GH.GameStates;

namespace GH
{
    public class CardInstance : MonoBehaviour, IClickable
    {
        public PlayerHolder owner;
        public GH.GameElements.Instance_logic currentLogic;
        public CardViz viz;
        public bool canAttack; //Indicates that card is just placed and can't attak this turn.

        [System.NonSerialized]
        public bool isOnAttack = false;

        private Transform originFieldTransform;
        void Start()
        {
            viz = GetComponent<CardViz>();
        }
        public void OnClick()
        {

            if (currentLogic == null)
                return;
            currentLogic.OnClick(this);
        }

        public void OnHighlight()
        {
            if (currentLogic == null)
                return;
            currentLogic.OnHighlight(this);
        }

        public void SetIsJustPlaced(bool isPlaced)
        {

            if (isPlaced)
                viz.gameObject.GetComponentInChildren<SpriteRenderer>().color = Color.cyan;            
            else
                viz.gameObject.GetComponentInChildren<SpriteRenderer>().color = Color.white;          
        }
        
        public void CardInstanceToGrave()
        {
            //Card die
            Setting.RegisterLog("Card die", Color.black);
        }
        public bool CanBeBlocked(CardInstance block)
        {
            bool result =  false;

            if (owner.attackingCards == null)
                Setting.RegisterLog("Attacking cards error", Color.black);

            else
                result = owner.attackingCards.Contains(this);

            Debug.Log(result);
            //Error point.
            //result is false and it doesn't go into if statement below.
            //owner.AttackingCards need some change
            if (result && viz.card.cardType.canAttack)
            {
                Debug.Log("Check1");
                result = true;
                //if a card has flying that can be blocked by non flying, you can check it here
                //Or cases like that should be here
                if (result)
                {
                    Debug.Log("Check2");
                    Setting.gameController.AddBlockInstance(this, block);
                }
                return result;
            }
            else
            {
                Debug.Log("check3");
                return false;
            }

        }
        public bool CanAttack()
        {
            bool result = true;

            //Debug.Log(canAttack);

            if(viz.card.cardType.TypeAllowsAttack(this))
            {
                result = true;
            }
            if (!canAttack)
                result = false;
            return result;
        }

        public void SetIsOnAttackTrue()
        {
            isOnAttack = true;
        }
        public bool GetIsOnAttack()
        {
            return isOnAttack;
        }
        public void SetOriginFieldLocation(Transform t)
        {
            originFieldTransform = t;
        }
        public Transform GetOriginFieldLocation()
        { return originFieldTransform; }

    }

}
