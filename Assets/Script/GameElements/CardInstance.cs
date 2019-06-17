using UnityEngine;
using System.Collections;
using GH.GameStates;
using GH.GameElements;
using GH.Setup;
namespace GH.GameCard
{
    public class CardInstance : MonoBehaviour, IClickable
    {
        public PlayerHolder owner;
        public Instance_logic currentLogic;
        public CardViz viz;
        private bool canAttack =false ; //Indicates that card is just placed and can't attak this turn.
        public bool dead;
        private bool _isOnAttack = false;
        private Transform originFieldTransform;
        private int fieldIndex;

        public int FieldIndex
        {
            set { fieldIndex = value; }
            get { return fieldIndex; }
        }
        /// <summary>
        /// This function need changes 
        /// Color of the card should automatically changed based on canAttack.
        /// I suggest move this method to 'GameController.cs" -- 190524 Hwan
        /// </summary>
        /// <param name="usable"></param>
        public void IsAvailable(bool usable)
        {
            if (usable)
            {
                viz.gameObject.GetComponentInChildren<SpriteRenderer>().color = Color.cyan;
                this.gameObject.transform.Rotate(0,0,-45);

            }
            else
            {
                viz.gameObject.GetComponentInChildren<SpriteRenderer>().color = Color.white;
                this.gameObject.transform.Rotate(0, 0, 45);
            }
        }
        public void CardInstanceToGrave()
        {
            //Card die
            canAttack = false;
            Setting.gameController.PutCardToGrave(this);
            Setting.RegisterLog(this.viz.card + "Card die", Color.black);

        }
        public bool CanBeBlocked(CardInstance block, ref int count)
        {
            bool result = false;
            if (owner.attackingCards == null)
            {
                Setting.RegisterLog("Attacking cards error", Color.black);
                Debug.Log("CanbeBlockedErr");
                return false;

            }
            else
            {
                Debug.Log("Can block");
                result = owner.attackingCards.Contains(this);
            }
            if (result && viz.card.cardType.canAttack)
            {
                result = true;
                //if a card has flying that can be blocked by non flying, you can check it here
                //Or cases like that should be here
                if (result)
                {
                   // Setting.gameController.BlockManager.AddBlockInstance(this, block, ref count);
                }
                return result;
            }
            else
            {
                return false;
            }

        }
        public bool GetCanAttack()
        {
            bool result = true;

            if (viz.card.cardType.TypeAllowsAttack(this))
            {
                result = true;
            }
            if (!canAttack)
                result = false;
            return result;
        }
        public void SetCanAttack(bool available)
        {
            canAttack = available;
        }
        public bool IsOnAttack
        {
            get { return _isOnAttack; }
            set { _isOnAttack = value; }
        }
        public void SetOriginFieldLocation(Transform t)
        {
            originFieldTransform = t;
        }
        public Transform GetOriginFieldLocation()
        { return originFieldTransform; }


        void Start()
        {
            FieldIndex = -1;
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
    }
}
