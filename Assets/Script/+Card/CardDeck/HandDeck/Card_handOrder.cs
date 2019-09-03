using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GH.GameElements
{
    /*
     * Arrange cards on hand
     * The logic place cards from middle to edge like spreading cards
     * Number of cards determine which logic to use, even or odd.
     * The card angle and distance differences are same.
     * Difference of even and odd is existence of middle card.
     */

    public class Card_handOrder : MonoBehaviour
    {
        private bool isBottom;
        private int cardsOnHand;
        private int midIndex;
        private int currentIndex;
        private Transform leftEnd;
        private Transform midCenter;
        private Transform rightEnd;
        private Transform card;


        private void Awake()
        {
            isBottom = this.gameObject.GetComponentInParent<PositionHolder>().IsAtBottom;



        }
        private void EvenLogic()
        {
            float transDif_even = 3.5f;
            currentIndex = cardsOnHand / 2 - 1;
            midIndex = currentIndex;
            for (int i = 0; i < cardsOnHand / 2; i++)
            {
                Transform left = gameObject.transform.GetChild(currentIndex);
                Transform right = gameObject.transform.GetChild(midIndex + i + 1);

                left.localPosition = new Vector3(-transDif_even, 0, -(i * 2));
                right.localPosition = new Vector3(transDif_even, 0, -(i * 2));

                if(isBottom)
                {
                    //Player1 Arrange
                    left.rotation = Quaternion.Euler(90, 0, transDif_even);
                    right.rotation = Quaternion.Euler(90, 0, -transDif_even);
                }
                else if(!isBottom)
                {
                    //Player2 Arrange
                    left.rotation = Quaternion.Euler(90, 0, -transDif_even+180);
                    right.rotation = Quaternion.Euler(90, 0, transDif_even+180);
                }

                transDif_even = transDif_even + 7;
                currentIndex = currentIndex - 1;
            }

        }
        private void OddLogic()
        {
            float transDif_odd = 7;
            midIndex = cardsOnHand / 2 + 1; 
            midCenter = gameObject.transform.GetChild(cardsOnHand / 2);
            
            midCenter.localPosition = new Vector3(0, 0, 0);
            
            midCenter.rotation = Quaternion.Euler(90,0,0);

            if(!isBottom)
                midCenter.rotation = Quaternion.Euler(90, 0, 180);
       
            for (int i = 0; i < cardsOnHand / 2; i++)
            {
                currentIndex = cardsOnHand / 2 - 1;
                Transform left = gameObject.transform.GetChild(currentIndex);
                Transform right = gameObject.transform.GetChild(midIndex + i);

                left.localPosition = new Vector3(-transDif_odd, 0, -(i * 2));
                right.localPosition = new Vector3(transDif_odd, 0, -(i * 2));

                if (isBottom)
                {
                    //Player1 Arrange
                    left.rotation = Quaternion.Euler(90, 0, transDif_odd);
                    right.rotation = Quaternion.Euler(90, 0, -transDif_odd);
                }
                else if (!isBottom)
                {
                    //Player2 Arrange
                    left.rotation = Quaternion.Euler(90, 0, -transDif_odd + 180);
                    right.rotation = Quaternion.Euler(90, 0, transDif_odd + 180);
                }
                transDif_odd = transDif_odd + 7;
                currentIndex = currentIndex - 1;
            }

        }

        private void Update()
        {
            cardsOnHand = gameObject.transform.childCount;
            if (cardsOnHand == 0)
                return;
            if (cardsOnHand > 7)
            {
                Setting.RegisterLog("Too many Cards",Color.green);

                return;
            }

            //Logic for even counts
            if (cardsOnHand % 2 == 0)
                EvenLogic();
            //Logic for odd counts
            else if (cardsOnHand % 2 == 1)
                OddLogic();
            else
                Debug.LogError("HandOrderErr");

        }

    }
}
    