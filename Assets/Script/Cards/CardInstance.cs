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
        private bool attackable =false ; //Indicates that card is just placed and can't attak this turn.
        public bool dead;
        private bool _isOnAttack = false;
        [SerializeField]
        private Transform parentFieldTransform;
        private int fieldIndex;

        public bool GetAttackable()
        {
            bool result = true;

            if (viz.card.cardType.TypeAllowsAttack(this))
            {
                result = true;
            }
            if (!attackable)
                result = false;
            return result;
        }
        public void SetAttackable(bool available)
        {
            attackable = available;
        }
        public bool IsOnAttack
        {
            get { return _isOnAttack; }
            set { _isOnAttack = value; }
        }

        public void SetOriginFieldLocation(Transform fieldTransform)
        {
            parentFieldTransform = fieldTransform;
        }
        /// <summary>
        /// Get this card instance's original field location.
        /// Field location is saved to return back to its position after moves.
        /// </summary>
        /// <returns></returns>
        public Transform GetOriginFieldLocation()
        {
            if (parentFieldTransform == null)
            {
                //This might occur if card instance is called by its id
                Debug.LogErrorFormat("GetOriginalFieldLocationError: {0}'s {1} Field Location isn't saved",this.owner.player,this.viz.card.name);
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
            //Debug.Log("CanUseByViz: This card owner "+owner.player);
            if (usable)
            {
                //Debug.LogFormat("This card_{0} can use now", this.viz.card.name);
                viz.gameObject.GetComponentInChildren<SpriteRenderer>().color = Color.white;
                this.gameObject.transform.localRotation= Quaternion.Euler(0,0,0);

            }
            else
            {
                //Debug.LogFormat("This card_{0} cant use now", this.viz.card.name);
                viz.gameObject.GetComponentInChildren<SpriteRenderer>().color = Color.cyan;
                this.gameObject.transform.localRotation = Quaternion.Euler(0, 0, -45);
            }
        }
        public void CardInstanceToGrave()
        {
            //Card die
            attackable = false;
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
