using UnityEngine;
using UnityEditor;
using GH.GameCard;
namespace GH
{
    //[CreateAssetMenu(menuName ="Player Profile")]
    //public class PlayerProfile : ScriptableObject
    //{
    //    [SerializeField]
    //    private string[] _CardId;

    //    public string[] GetCardIds()
    //    {
    //        return _CardId;
    //    }

    //}

    [System.Serializable]
    public class PlayerProfile
    {
        [SerializeField]
        private string uniqueId;
        [SerializeField]
        private string name;
        [SerializeField]
        private Sprite playerAvatar;
        [SerializeField]
        private Card[] _Card;
        [SerializeField]
        private string[] _CardId;

        //[SerializeField]
        //public ProfileData_Deck[] deckList;
        public string[] GetCardIds()
        {
            return _CardId;
        }

        //public string GetCardIds(int i)
        //{
        //    return _Card[i].properties[i].stringValue;              //1st property of Card is card name
        //}
    }
}
