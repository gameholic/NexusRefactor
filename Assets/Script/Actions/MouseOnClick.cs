using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.EventSystems;


namespace GH.GameStates
{
    [CreateAssetMenu(menuName = "Actions/MouseOnClick")]


    public class MouseOnClick : Action
    {
        
        private bool check = false;
        public override void Execute(float d)
        {

            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit[] results = Setting.GetUIObjs();
                IClickable c = null;

                for (int i = 0; i < results.Length; i++)
                {
                    RaycastHit hit = results[i];
                    c = hit.transform.gameObject.GetComponentInParent<IClickable>();

                    check = Setting.gameController.checkObjOwner.CheckPlayer(hit.transform.gameObject);
                    //Debug.Log("Check Obj Owner: " + check);
                    if (!check)//break when the gameObject is unclickable (other player's gameObject);
                    {
                        Debug.Log("This isn't your control");
                        break;

                    }

                    if (c != null)
                    {
                        c.OnClick();
                        break;
                    }
                }

            }
        }

    }
}

