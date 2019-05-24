using UnityEngine;
using System.Collections;
using GH;
using GH.GameCard;
using GH.GameStates;

namespace GH.GameElements
{
    [CreateAssetMenu(menuName = "GameElements/My Field")]
    public class Card_Myfield : Instance_logic
    {
        public State battlephase;
        public GameEvent onCurrentCardSelected;
        public BattlePhaseStartCheck startBattle;

        public CardVariables currentCard;
        public override void OnClick(CardInstance inst)
        {
            if (startBattle.IsValid())
            {
                currentCard.Set(inst);
                Setting.gameController.SetState(battlephase);
                onCurrentCardSelected.Raise();
            }
            else
            {
                Setting.RegisterLog(this.name + " startbattle is invalid", Color.red);
                return;
            }
        }
        public override void OnHighlight(CardInstance inst)
        {
            
        }
    }
} 
