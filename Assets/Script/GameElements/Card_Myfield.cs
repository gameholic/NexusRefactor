using UnityEngine;
using System.Collections;

namespace GH.GameElements
{
    [CreateAssetMenu(menuName = "GameElements/My Field")]
    public class Card_Myfield : Instance_logic
    {
        public GH.GameStates.State battlephase;
        public GH.GameEvent oncurrentcardselected;
        public GH.BattlePhaseStartCheck startbattle;

        public CardVariables currentcard;
        public override void OnClick(CardInstance inst)
        {
            if (startbattle.IsValid())
            {
                currentcard.Set(inst);
                Setting.gameController.SetState(battlephase);
                oncurrentcardselected.Raise();
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
