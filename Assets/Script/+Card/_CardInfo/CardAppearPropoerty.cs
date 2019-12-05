using UnityEngine;
using UnityEditor;


namespace GH.GameCard.CardInfo
{
    [System.Serializable]
    public class CardAppearPropoerty
    {
        public TextMesh text;
        public SpriteRenderer renderer;
        public GH.GameCard.CardElement.Element element;
    }

}