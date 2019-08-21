using UnityEngine;

using GH.GameCard;
using GH.GameCard.CardInfo;

namespace GH.Nexus.Manager
{
    public class TestManager : MonoBehaviour
    {
        public Card[] targetCard;

        [SerializeField]
        private GameObject _CardPrefab;

        int count = 0;
        private void Start()
        {

            for( count = 0; count < targetCard.Length; count++)
            {
                targetCard[count] = Instantiate(targetCard[count]);
                LoadCard(targetCard[count]);
            }
        }
        
        public void LoadCard(Card c)         //Needed when loading card
        {
            GameObject go = Instantiate(_CardPrefab) as GameObject;
            CardAppearance v = go.GetComponent<CardAppearance>();
            if(v!=null)
            {
                v.LoadCard(c, go);
                go.transform.SetParent(this.gameObject.transform);
                go.transform.localPosition = new Vector3(-20f + count * 10, 0, 0);
                go.transform.localScale = Vector3.one;


            }
        }
    }
}
