using UnityEngine;


using GH.GameCard;
using GH.GameCard.CardState;
using GH.GameTurn;
using GH.Multiplay;

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
        private Phase _PlayerBlockPhase;
        [SerializeField]
        private Phase _PlayerControlPhase;
        [SerializeField]
        private Card_Myfield cardOnField;


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
                        CardInstance c = hit.transform.gameObject.GetComponentInParent<CardInstance>();
                        

                        if (c != null)
                        {
                            int count = 0;
                            bool block = c.CanBeBlocked(_SelectedCard.value, ref count);
                            if (block)
                            {
                                CardInstance thisCard = _SelectedCard.value;
                                MultiplayManager.singleton.PlayerBlocksTargetCard
                                    (thisCard.viz.card.InstId, thisCard.owner.PhotonId,
                                    c.viz.card.InstId, c.owner.PhotonId);
                               
                                    
                                //Setting.SetCardForblock(_SelectedCard.value.transform, c.transform, count);
                            }
                            else
                            {
                                Debug.LogFormat("MouseHoldWithCard_{0}: {1} can't be blocked card.", gc.CurrentPlayer.player, c.viz.card.name);
                            }


                            _SelectedCard.value.gameObject.SetActive(true);
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
                        CardInstance c = hit.transform.gameObject.GetComponentInParent<CardInstance>();
                        GameElements.Area a
                            = hit.transform.gameObject.GetComponentInParent<GameElements.Area>();
                        if(c!=null)
                        {
                            Debug.Log("eheheheh");
                            if (gc.CurrentPlayer.fieldCard.Contains(c))
                            {
                                Debug.LogWarning("MouseHoldWithCardWarning:You Can't Pick Cards On Field");
                                return;
                            }
                            if (c.currentLogic is Card_Myfield)
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
                    _SelectedCard.value.gameObject.SetActive(true);
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