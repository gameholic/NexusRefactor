using UnityEngine;
using System.Collections;
using GH.GameStates;
using GH.GameCard;
using GH.Multiplay;
namespace GH
{
    [CreateAssetMenu(menuName = "Actions/SelectCardsToAttack")]
    public class SelectCardsToAttack : Action
    {
        //private bool Check = false;
        public override void Execute(float d)
        {

            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit[] results = Setting.GetUIObjs();
                
                for (int i = 0; i < results.Length; i++)
                {
                    RaycastHit hit = results[i];
                    CardInstance inst = hit.transform.gameObject.GetComponentInChildren<CardInstance>();
                    PlayerHolder p = Setting.gameController.CurrentPlayer;
                    //Check inst is one of the current player's card (placed on field)                  
                
                    if(inst!=null)
                    {
                        if (!inst.GetCanAttack() /*|| inst.IsOnAttack*/)
                        {
                            Setting.RegisterLog("This card can't attack. ", Color.black);
                            return;
                        }
                        MultiplayManager.singleton.PlayerTryToUseCard(inst.viz.card.InstId, p.PhotonId, MultiplayManager.CardOperation.setCardToAttack);
                    }
                    else
                    {
                        Setting.RegisterLog("InstIsNullError" + " Obj: " + hit.transform.gameObject, Color.black);
                        return;                        
                        //Above if statements are for checking errors
                    }
                }   
            }
        }
    }
}
