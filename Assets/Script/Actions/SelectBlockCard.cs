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
                        if (c.owner != enemy && 
                            gc.CurrentPlayer.fieldCard.Contains(c) && 
                            c.GetAttackable())
                        {
                            _SelectedCard.value = c;
                            gc.SetState(_HoldingCardState);
                            _OnCardSelectEvent.Raise();                            
                        }
                        else if(!c.GetAttackable())
                        {
                            Debug.LogErrorFormat("SelectBlockCardError: This {0} can't attack now", c.viz.card.name);
                        }
                        else if (!gc.CurrentPlayer.fieldCard.Contains(c))
                        {
                            Debug.LogErrorFormat("SelectBlockCardError: {0} don't have {1}", gc.CurrentPlayer,c.viz.card.name);
                        }
                        return;
                    }
                    else
                    {
                        hit.transform.gameObject.name = "Empty";
                        Debug.LogErrorFormat("SelectBlockCard_This gameobject ({0}) has no instance", hit.transform.gameObject.name);
                    }
                }

            }
        }

    }
}

