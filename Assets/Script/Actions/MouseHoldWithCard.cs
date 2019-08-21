using UnityEngine;

using GH.GameCard.CardInfo;
using GH.GameCard;
using GH.GameTurn;
using GH.Multiplay;

namespace GH.GameStates
{

    [CreateAssetMenu(menuName = "Actions/MouseHoldWithCard")]
    public class MouseHoldWithCard : Action
    {
#pragma warning disable 0649
        [SerializeField]
        private CardVariables _SelectedCard;
        [SerializeField]
        private State _PlayerControlState;
        [SerializeField]
        private State _PlayerBlockState;
        [SerializeField]
        private GameEvent _OnPlayerControlState;
        [SerializeField]
        private Phase _PlayerBlockPhase;
        [SerializeField]
        private Phase _PlayerControlPhase;

#pragma warning restore 0649


        public override void Execute(float d)
        {
            bool mouseIsDown = Input.GetMouseButton(0);
            if (!mouseIsDown)
            {
                GameController gc = Setting.gameController;
                RaycastHit[] results = Setting.GetUIObjs();
                Phase currentPhase = gc.GetTurns(gc.turnIndex).CurrentPhase.value;
                if (currentPhase == _PlayerBlockPhase)
                {
                    //Selecting card to block enemy attacking card
                    for (int i = 0; i < results.Length; i++)
                    {
                        RaycastHit hit = results[i];
                        PhysicalAttribute cardInst = hit.transform.gameObject.GetComponentInParent<PhysicalAttribute>();
                        CreatureCard atkCard = (CreatureCard)cardInst.OriginCard;
                        if (atkCard != null)
                        {
                            bool block = true;          //atkCard.CanBeBlocked(_SelectedCard.value, ref count);
                            if (block)
                            {
                                Card thisCard = _SelectedCard.value;
                                MultiplayManager.singleton.PlayerBlocksTargetCard
                                    (thisCard.Data.UniqueId, thisCard.User.InGameData.PhotonId,
                                    atkCard.Data.UniqueId, atkCard.User.InGameData.PhotonId);
                                //Setting.SetCardForblock(_SelectedCard.value.transform, c.transform, count);
                            }
                            else
                            {
                                Debug.LogFormat("MouseHoldWithCard_{0}: {1} can't be blocked card.", 
                                    gc.CurrentPlayer.PlayerProfile.UniqueId, atkCard.Data.Name);
                            }
                            _SelectedCard.value.PhysicalCondition.gameObject.SetActive(true);
                            _SelectedCard.value = null;
                            _OnPlayerControlState.Raise();
                            Setting.gameController.SetState(_PlayerBlockState);
                            break;
                        }

                    }
                }
                else if (currentPhase == _PlayerControlPhase)
                {
                    for (int i = 0; i < results.Length; i++)
                    {
                        RaycastHit hit = results[i];
                        PhysicalAttribute cardInst = hit.transform.gameObject.GetComponentInParent<PhysicalAttribute>();
                        CreatureCard c = (CreatureCard)cardInst.OriginCard;
                        GameElements.Area a
                            = hit.transform.gameObject.GetComponentInParent<GameElements.Area>();
                        if(c!=null)
                        {
                            Debug.Log("eheheheh");
                            if (gc.CurrentPlayer.CardManager.CheckCard(c.Data.UniqueId))
                            {
                                Debug.LogWarning("MouseHoldWithCardWarning:You Can't Pick Cards On Field");
                                return;
                            }
                            if (c.PhysicalCondition.IsOnField())
                            {
                                Debug.LogWarning("EBHBHBHBH");
                                return;
                            }
                            else
                            {
                                Debug.Log("MouseHoldWithCard: You can Pick Card");
                            }
                        }
                        //This is to check if user selectded card that is already on field. but don't work now. Needs to be fixed                   
                        if (a != null)
                        {
                            a.OnDrop(a);
                            break;
                        }
                        else
                        {
                            Debug.LogError("MouseHoldWithCard: Card Can't be placed. Area is null");
                        }
                    }
                    _SelectedCard.value.PhysicalCondition.gameObject.SetActive(true);
                    _SelectedCard.value = null;
                    Setting.gameController.SetState(_PlayerControlState);
                    _OnPlayerControlState.Raise();
                }
                else
                {
                    Debug.LogWarning("You Can't Hold Card At Current Phase: " + currentPhase);
                }
                    return;
            }

            return;
        }
    }

}