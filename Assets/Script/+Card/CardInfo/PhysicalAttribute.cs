using UnityEngine;

namespace GH.GameCard.CardInfo
{
    public class PhysicalAttribute : MonoBehaviour
    {
        #region Variables

        //private Card _OriginCard;

        #endregion

        #region Properties

        //public Card OriginalCard
        //{
        //    set
        //    {
        //        _OriginCard = value;
        //        _OriginCard.PhysicInstance = this;
        //    }
        //    get
        //    {
        //        return _OriginCard;
        //    }
        //}

        #endregion

        #region Functions

        public void Highlight()
        {
            transform.localScale = new Vector3(1.5f, 1.7f, 0.01f);
        }

        public void DeHighlight()
        {
            transform.localScale = new Vector3(1f, 1.5f, 0.01f);
        }
        #endregion
    }

}