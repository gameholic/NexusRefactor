using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using GH.GameTurn;
using GH.GameCard;
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
        private Phase _PlayerBattlePhase;


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
                else
                {
                    for (int i = 0; i < results.Length; i++)
                    {
                        RaycastHit hit = results[i];
                        GameElements.Area a
                            = hit.transform.gameObject.GetComponentInParent<GameElements.Area>();

                        if (gc.CurrentPlayer == _PlayerControlState && a != null)
                            break;

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
                    return;
            }

            return;
        }
    }

}