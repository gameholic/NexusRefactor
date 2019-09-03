using GH.GameCard;
using GH.GameCard.CardInfo;
using GH.GameCard.CardLogics;
using GH.GameTurn;
using GH.Player;

using UnityEngine;

namespace GH.MouseLogics
{
    public class TestMouseOperation : MonoBehaviour
    {
        private PhysicalAttribute currentCard = null;

        private void Update()
        {
            HandleMouse();
        }


        private void HandleMouse()
        {
            bool isMouseDown = Input.GetMouseButton(0);

            if (!isMouseDown)
            {
                HandleCardDetection();
            }
            else
            {
                HandleMouseDrag();
            }
        }
        private void HandleMouseDrag()
        {
            if (currentCard != null)
            {
                MouseDragging(currentCard);
                Debug.Log("DRAGGGING");
            }

        }
        public void MouseDragging(PhysicalAttribute c)
        {
            Debug.Log("BeforeTracking: "+
            c.transform.position);
            c.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 50));
            c.transform.SetAsLastSibling();
            Debug.Log("AFeterTracking: " +
            c.transform.position);

        }
        private void HandleCardClick()
        {
            bool isMouseDown = Input.GetMouseButtonDown(0);

            if (!isMouseDown)
            {
                if (currentCard != null)
                {
                    //TODO: Check current state / phase and perform action
                }
            }
        }
        private void HandleCardDetection()
        {
            RaycastHit[] hits = GetUIObjs();
            PhysicalAttribute detectedCard = null;
            for (int i = 0; i < hits.Length; i++)
            {
                detectedCard = hits[i].transform.gameObject.GetComponentInParent<PhysicalAttribute>();

                //If dectected card isn't current card
                if (detectedCard != null)
                {
                    //If there is card instance in 'currentCard', break
                    break;
                }
            }

            if (detectedCard != null)
            {
                if (currentCard != null)
                {
                    currentCard.DeHighlight();
                }
                currentCard = detectedCard;
                currentCard.Highlight();
            }
            else
            {
                if (currentCard != null)
                {
                    currentCard.DeHighlight();
                    currentCard = null;
                }
            }

        }

        public static RaycastHit[] GetUIObjs()
        {
            Camera mainCam = Camera.main;
            Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] ray = Physics.RaycastAll(cameraRay, 100.0f);

            return ray;
        }


    }

}