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
        [SerializeField] //This needs to be deleted
        private bool canAttack =false ; //Indicates that card is just placed and can't attak this turn.
        public bool dead;
        private bool _isOnAttack = false;
        private Transform parentFieldTransform;
        private int fieldIndex;

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
        public void SetOriginFieldLocation(Transform fieldTransform)
        {
            parentFieldTransform = fieldTransform;
            //Debug.Log("ParentFieldSET: " + parentFieldTransform + "/ Card: " + this.viz.card.name);

        }
        public Transform GetOriginFieldLocation()
        {
            if (parentFieldTransform == null)
            {
                Debug.Log("ParentFieldNULL/ Card: " + this.viz.card.name);
                return null;
            }
            return parentFieldTransform;
        }




        public int FieldIndex
        {
            set { fieldIndex = value; }
            get { return fieldIndex; }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="usable"></param>
        public void CanUseByViz(bool usable)
        {
            Debug.Log("CanUseByVizRun");
            if (usable)
            {
                Debug.LogFormat("This card_{0} can use now", this.viz.card.name);
                viz.gameObject.GetComponentInChildren<SpriteRenderer>().color = Color.white;
                this.gameObject.transform.localRotation= Quaternion.Euler(0,0,0);

            }
            else
            {
                Debug.LogFormat("This card_{0} cant use now", this.viz.card.name);
                viz.gameObject.GetComponentInChildren<SpriteRenderer>().color = Color.cyan;
                this.gameObject.transform.localRotation = Quaternion.Euler(0, 0, -45);
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
                Debug.LogError("CardInstance: Attacking cards error");
                return false;

            }
            else
            {
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
