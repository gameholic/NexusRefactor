using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.EventSystems;
using GH.GameCard;

namespace GH.GameStates
{
    [CreateAssetMenu(menuName = "Actions/OnSelectedCard")]
    public class OnSelectCard : Action
    {
        public GH.GameEvent onCurrentCardSelected;
        public CardVariables currentCard;
        public GameStates.State holdingCard;

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
                    PlayerHolder enemy = gc.GetOpponentOf(gc.currentPlayer);


                    if (c != null)
                    {
                        if(c.owner == enemy)
                        {
                            //Debug.Log(c.viz.name);
                            currentCard.value = c;
                            //c.OnClick();
                            gc.SetState(holdingCard);
                            onCurrentCardSelected.Raise();
                        }
                        return;
                    }
                }

            }
        }

    }
}

