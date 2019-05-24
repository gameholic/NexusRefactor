using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GH.GameCard;

namespace GH
{
    public class CurrentSelected : MonoBehaviour
    {
        public CardVariables currentCard;
        public CardViz cardViz;
        
        private Transform mTransform;
        
        public void LoadCard()
        {
            if (currentCard.value == null)
                return;

            currentCard.value.gameObject.SetActive(false);
            cardViz.LoadCard(currentCard.value.viz.card);
            
            cardViz.gameObject.SetActive(true);
        } 
        public void CloseCard()
        {
            cardViz.gameObject.SetActive(false);
        }

        private void Start()
        {
            mTransform = this.transform;
            CloseCard();
        }

        void Update()
        {
            mTransform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 40)); 
        }
    }
}