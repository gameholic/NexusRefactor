//using UnityEngine;
//using System.Collections.Generic;
//using System.Collections;
//using UnityEngine.EventSystems;



//namespace GH.GameStates
//{
//    [CreateAssetMenu(menuName = "Actions/MouseOverDetection")]
//    public class MouseOverDetection : Action
//    {
//        public override void Execute(float d)
//        {
//            //int count = 0;
//            RaycastHit[] results = Setting.GetUIObjs();

//            IClickable c = null;

//            for (int i = 0; i < results.Length; i++)
//            {
//                RaycastHit hit = results[i];
//                c = hit.transform.gameObject.GetComponentInParent<IClickable>();
//                if (c != null)
//                {
//                    c.OnHighlight();
//                    break;
//                }
//            }

//        }

//    }
//}

