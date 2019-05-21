using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GH.GameStates
{

    [CreateAssetMenu(menuName = "Actions/MouseHoldWithCard")]
    public class MouseHoldWithCard : Action
    {
        public CardVariables currentCard;
        public State playerControlState;
        public State playerBlockState;
        public GH.GameEvent onPlayerControlState;
        public Phase blockPhase;


        public override void Execute(float d)
        {
            bool mouseIsDown = Input.GetMouseButton(0);

            if (!mouseIsDown)
            {
                GameController gc = Setting.gameController;
                RaycastHit[] results = Setting.GetUIObjs();

                if (gc.turns[gc.turnIndex].currentPhase.value != blockPhase)
                {
                    for (int i = 0; i < results.Length; i++)
                    {
                        RaycastHit hit = results[i];
                        GameElements.Area a
                            = hit.transform.gameObject.GetComponentInParent<GameElements.Area>();

                        if (a != null)
                        {
                            a.OnDrop(a);
                            break;
                        }
                    }
                    currentCard.value.gameObject.SetActive(true);
                    currentCard.value = null;
                    Setting.gameController.SetState(playerControlState);
                    onPlayerControlState.Raise();
                }
                else
                {
                    for (int i = 0; i < results.Length; i++)
                    {
                        RaycastHit hit = results[i];
                        CardInstance c = hit.transform.gameObject.GetComponentInParent<CardInstance>();
                        if(c != null)
                        {
                            int count = 0;                        
                            bool block = c.CanBeBlocked(currentCard.value, ref count);

                            if(block)
                            {
                                Setting.SetCardForblock(currentCard.value.transform, c.transform, count);                                 
                            }

                            currentCard.value.gameObject.SetActive(true);
                            currentCard.value = null;
                            onPlayerControlState.Raise();
                            Setting.gameController.SetState(playerBlockState);
                            break;
                        }

                    } 
                }
                    return;
            }

            return;
        }
    }

}