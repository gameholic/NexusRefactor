using UnityEngine;

namespace GH.GameCard.CardInfo
{
    public class PhysicalAttribute : MonoBehaviour
    {
        #region Variables
        private Card _OriginCard;
        private Vector3 oldPos;
        private Transform fieldTransform;           //Field location of card. Registered when card is placed on field
        #endregion

        #region Properties
        public Vector3 OldPos
        {
            set { oldPos = value; }

            get { return oldPos; }
        }
        public Card OriginCard
        {
            set { _OriginCard = value; }
            get { return _OriginCard; }
        }
        #endregion

        #region Functions

        public void Highlight()
        {
            transform.localScale = new Vector3(1.5f, 1.7f, 0.01f);
        }
        public void DeHighlight()
        {
            transform.localScale = Vector3.one;
        }
        public void SetOriginFieldLocation(Transform t)
        {
            fieldTransform = t;
        }
        public Transform GetOriginFieldLocation()
        {
            if (fieldTransform == null)  //This might occur if card instance is called by its id
            {
                Debug.LogErrorFormat("GetOriginalFieldLocationError: {0}'s {1} Field Location isn't saved",
                    this.OriginCard.User, this.OriginCard.Data.Name);
                return null;
            }
            return fieldTransform;
        }
        public bool IsOnField()
        {
            bool v = false;
            int id = _OriginCard.Data.UniqueId;
            if(OriginCard is CreatureCard)
            {
                v = _OriginCard.User.CardManager.CheckCardContainer(Player.CardContainer.Field, this.OriginCard);
            }
            return v;
        }
        #endregion
    }

}