using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.EventSystems;
using GH.GameCard;

namespace GH.GameStates
{
    [CreateAssetMenu(menuName = "Actions/SelectBlockCard")]
    public class SelectBlockCard : Action
    {
        [SerializeField]
        private GameEvent _OnCardSelectEvent;
        [SerializeField]
        private CardVariables _SelectedCard;
        [SerializeField]
        private State _HoldingCardState;

        public override void Execute(float d)
        {
            GameController gc = Setting.gameController;
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit[] results = Setting.GetUIObjs();
                CardInstance c = null;

                for (int i = 0; i < results.Length; i++)
                {
                    RaycastHit hit = results[i];
                    c = hit.transform.gameObject.GetComponentInParent<CardInstance>();
                    PlayerHolder enemy = gc.GetOpponentOf(gc.CurrentPlayer);


                    if (c != null)
                    {
                        if(c.owner == enemy)
                        {
                            _SelectedCard.value = c;
                            //c.OnClick();
                            gc.SetState(_HoldingCardState);
                            _OnCardSelectEvent.Raise();
                        }
                        return;
                    }
                }

            }
        }

    }
}

