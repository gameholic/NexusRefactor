using UnityEngine;
using System.Collections;
using GH.GameStates;

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
                    //Get card instance
                    CardInstance inst = hit.transform.gameObject.GetComponentInParent<CardInstance>();
                    PlayerHolder p = Setting.gameController.currentPlayer;

                    //Check inst is one of the current player's card (placed on field)                  
                    if (!inst == p.fieldCard.Contains(inst))
                    {
                        //Action for card on field
                        return;
                    }

                    if(inst == null)
                    {
                        Setting.RegisterLog("InstIsNullError", Color.black);
                        Setting.RegisterLog("---------------", Color.black);
                        Setting.RegisterLog("Player: "+p.name, Color.black);
                        Setting.RegisterLog("Obj: "+hit.transform.gameObject, Color.black);
                        return;

                    }
                    if (!inst.CanAttack() || inst.GetIsOnAttack())
                    {
                        if (!inst.CanAttack())
                            Setting.RegisterLog("This card can't attack. ", Color.black);
                        else if (inst.GetIsOnAttack())
                            Setting.RegisterLog("This card is already on attack", Color.black);
                        return;
                      
                    }
                    //Above if statements are for checking errors
                    else
                    {
                        //Card can attack.
                        p.attackingCards.Add(inst);
                        //p.fieldCard.Remove(inst); // remove card from fieldcard
                        //Debug.Log("Attacking card added Player: "+p.name);
                        p.currentCardHolder.SetCardOnBattleLine(inst);
                    }
                }

            }
        }
    }
}
