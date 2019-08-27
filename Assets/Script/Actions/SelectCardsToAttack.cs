//using GH.GameCard;
//using GH.GameCard.CardInfo;
//using GH.GameStates;
//using GH.Multiplay;
//using GH.Player;
//using UnityEngine;
//namespace GH
//{
//    [CreateAssetMenu(menuName = "Actions/SelectCardsToAttack")]
//    public class SelectCardsToAttack : Action
//    {
//        //private bool Check = false;
//        public override void Execute(float d)
//        {

//            if (Input.GetMouseButtonDown(0))
//            {
//                RaycastHit[] results = Setting.GetUIObjs();
//                //Raycast all objects that user clicked.
//                // If user selects card that can attack enemy, send card instance id and player's photon id to MultiplayManager.
//                //   In MultiplayManager.PlayerTryToUseCard, Card on field move to 'BattleLine'Obj and 'attackingCards' list of playerholder for 'BattleResolvePhase'
//                for (int i = 0; i < results.Length; i++)
//                {
//                    RaycastHit hit = results[i];
//                    Card inst = hit.transform.gameObject.GetComponentInChildren<PhysicalAttribute>().OriginCard;

//                    //Get Current player to send its photon id                   
//                    PlayerHolder p = Setting.gameController.CurrentPlayer;
                
//                    //Check 'CardInstance' existance to let player can select card objects only
//                    if(inst!=null)
//                    {
//                        //If selected card can't perform attack in whatever reason, finish codes with error message
//                        if (!inst.CanUseCard())
//                        {
//                            Debug.LogWarningFormat("{0} can't attack.", inst.Data.Name);
//                            return;
//                        }
//                        MultiplayManager.singleton.PlayerTryToUseCard(inst.Data.UniqueId, p.InGameData.PhotonId, MultiplayManager.CardOperation.setCardToAttack);
              
//                    }
//                    else
//                    {
//                        //Send error message if 'CardInstance' is null
//                        Debug.LogErrorFormat("Card Instance ({0}) is null", inst.Data.Name);
//                        return;            
//                    }
//                }   
//            }
//        }
//    }
//}
