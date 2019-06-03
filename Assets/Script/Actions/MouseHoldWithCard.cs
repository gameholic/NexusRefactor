using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using GH.GameTurn;
using GH.GameCard;

namespace GH.GameStates
{

    [CreateAssetMenu(menuName = "Actions/MouseHoldWithCard")]
    public class MouseHoldWithCard : Action
    {
        [SerializeField]
        private CardVariables _SelectedCard;
        [SerializeField]
        private State _PlayerControlState;
        [SerializeField]
        private State _PlayerBlockState;
        [SerializeField]
        private GameEvent _OnPlayerControlState;
        [SerializeField]
        private Phase blockPhase;


        public override void Execute(float d)
        {
            bool mouseIsDown = Input.GetMouseButton(0);

            if (!mouseIsDown)
            {
                GameController gc = Setting.gameController;
                RaycastHit[] results = Setting.GetUIObjs();

                if (gc.GetTurns(gc.turnIndex).CurrentPhase.value != blockPhase)
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
                    _SelectedCard.value.gameObject.SetActive(true);
                    _SelectedCard.value = null;
                    Setting.gameController.SetState(_PlayerControlState);
                    _OnPlayerControlState.Raise();
                }
                else if(gc.GetTurns(gc.turnIndex).CurrentPhase.value == blockPhase)
                {
                    for (int i = 0; i < results.Length; i++)
                    {
                        RaycastHit hit = results[i];
                        CardInstance c = hit.transform.gameObject.GetComponentInParent<CardInstance>();
                        if(c != null/* && c.currentLogic = Mycard_field*/)
                        {
                            int count = 0;                        
                            bool block = c.CanBeBlocked(_SelectedCard.value, ref count);
                            if(block)
                            {
                                Setting.SetCardForblock(_SelectedCard.value.transform, c.transform, count);                                 
                            }
                            _SelectedCard.value.gameObject.SetActive(true);
                            _SelectedCard.value = null;
                            _OnPlayerControlState.Raise();
                            Setting.gameController.SetState(_PlayerBlockState);
                            break;
                        }

                    } 
                }
                else
                {
                    Debug.Log("MouseHoldCardException");
                }
                    return;
            }

            return;
        }
    }

}