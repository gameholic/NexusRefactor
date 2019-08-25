using GH.GameCard;
using GH.Player;
using UnityEngine;
using GH.GameCard.CardInfo;

namespace GH.GameStates
{
    //[CreateAssetMenu(menuName = "Actions/SelectBlockCard")]
    //public class SelectBlockCard : Action
    //{
    //    [SerializeField]
    //    private GameEvent _OnCardSelectEvent;
    //    [SerializeField]
    //    private CardVariables _SelectedCard;
    //    [SerializeField]
    //    private State _HoldingCardState;

    //    public override void Execute(float d)
    //    {
    //        GameController gc = Setting.gameController;
    //        if (Input.GetMouseButtonDown(0))
    //        {
    //            RaycastHit[] results = Setting.GetUIObjs();
    //            Card c = null;

    //            for (int i = 0; i < results.Length; i++)
    //            {
    //                RaycastHit hit = results[i];
    //                c = hit.transform.gameObject.GetComponentInParent<PhysicalAttribute>().OriginCard;
    //                PlayerHolder enemy = gc.GetOpponentOf(gc.CurrentPlayer);


    //                if (c != null)
    //                {                        
    //                    if (c.User != enemy && 
    //                        gc.CurrentPlayer.CardManager.CheckCard(c.Data.UniqueId) && 
    //                        c.UseCard())
    //                    {
    //                        _SelectedCard.value = c;
    //                        gc.SetState(_HoldingCardState);
    //                        _OnCardSelectEvent.Raise();                            
    //                    }
    //                    else if(!c.UseCard())
    //                    {
    //                        Debug.LogErrorFormat("SelectBlockCardError: This {0} can't attack now", c.Data.Name);
    //                    }
    //                    else if (!gc.CurrentPlayer.CardManager.CheckCardContainer(CardContainer.Field,c))
    //                    {
    //                        Debug.LogErrorFormat("SelectBlockCardError: {0} don't have {1}", gc.CurrentPlayer,c.Data.Name);
    //                    }
    //                    else if (c.User == enemy)
    //                    {
    //                        Debug.LogErrorFormat("SelectBlockCardError: This card owner is enemy");
    //                    }
    //                    return;
    //                }
    //                else
    //                {
    //                    Debug.LogErrorFormat("SelectBlockCard_This gameobject ({0}) has no instance", hit.transform.gameObject.name);
    //                }
    //            }

    //        }
    //    }

    //}
}

