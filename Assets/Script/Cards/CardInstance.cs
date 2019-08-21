//using GH.GameCard.CardState;
//using GH.GameStates;
//using UnityEngine;
//using GH.Player;

//namespace GH.Old
//{
//    public class CardInstance : MonoBehaviour, IClickable
//    {
//        public PlayerHolder owner;
//        public CardStateLogic currentLogic;
//        public CardViz viz;
//        [SerializeField] //This needs to be deleted
//        private bool attackable = false; //Indicates that card is just placed and can't attak this turn.
//        public bool dead;
//        private bool _isOnAttack = false;
//        [SerializeField]
//        private Transform parentFieldTransform;
//        //private int fieldIndex;



//        public bool GetAttackable()
//        {
//            bool result = true;

//            if (viz.card.cardType.TypeAllowsAttack(this))
//            {
//                result = true;
//            }
//            if (!attackable)
//                result = false;
//            return result;
//        }
//        public void SetAttackable(bool available)
//        {
//            attackable = available;
//        }
//        public bool IsOnAttack
//        {
//            get { return _isOnAttack; }
//            set { _isOnAttack = value; }
//        }

//        public void SetOriginFieldLocation(Transform fieldTransform)
//        {
//            parentFieldTransform = fieldTransform;
//        }
//        /// <summary>
//        /// Get this card instance's original field location.
//        /// Field location is saved to return back to its position after moves.
//        /// </summary>
//        /// <returns></returns>
//        public Transform GetOriginFieldLocation()
//        {
//            if (parentFieldTransform == null)
//            {
//                //This might occur if card instance is called by its id
//                Debug.LogErrorFormat("GetOriginalFieldLocationError: {0}'s {1} Field Location isn't saved", this.owner.player, this.Data.Name);
//                return null;
//            }
//            return parentFieldTransform;
//        }




//        //public int FieldIndex
//        //{
//        //    set { fieldIndex = value; }
//        //    get { return fieldIndex; }
//        //}

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="usable"></param>
//        public void CanUseByViz(bool usable)
//        {
//            //Debug.Log("CanUseByViz: This card owner "+owner.player);
//            if (usable)
//            {
//                //Debug.LogFormat("This card_{0} can use now", this.Data.Name);
//                viz.gameObject.GetComponentInChildren<SpriteRenderer>().color = Color.white;
//                this.gameObject.transform.localRotation = Quaternion.Euler(0, 0, 0);

//            }
//            else
//            {
//                //Debug.LogFormat("This card_{0} cant use now", this.Data.Name);
//                viz.gameObject.GetComponentInChildren<SpriteRenderer>().color = Color.cyan;
//                this.gameObject.transform.localRotation = Quaternion.Euler(0, 0, -45);
//            }
//        }
//        public void CardInstanceToGrave()
//        {
//            //Card die
//            Debug.LogFormat("CardInstanceToGrave: {0}'s {1} Died", this.owner.player, this.Data.Name);
//            attackable = false;
//            Setting.gameController.PutCardToGrave(this);
//            Setting.RegisterLog(this.viz.card + "Card die", Color.black);

//        }
//        void Start()
//        {
//            //FieldIndex = -1;
//            viz = GetComponent<CardViz>();
//        }

//        public void OnClick()
//        {
//            if (currentLogic == null)
//            {
//                Debug.LogError("CurrentLogic is null");
//                return;
//            }
//            currentLogic.LOnClick(this);
//        }
//        public void OnHighlight()
//        {
//            if (currentLogic == null)
//                return;
//            currentLogic.LOnHighlight(this);
//        }
//    }
//}
